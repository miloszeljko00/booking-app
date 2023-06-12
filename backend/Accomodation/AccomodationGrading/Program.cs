using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using System.Reflection;
using AccomodationGrading.Configuration;
using Grpc.Core;
using MediatR;
using Rs.Ac.Uns.Ftn.Grpc;
using AccomodationGradingApplication.Grading.Support.Grpc;
using AccomodationGradingDomain.Interfaces;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using OpenTracing;
using Jaeger.Reporters;
using Jaeger;
using Jaeger.Samplers;
using OpenTracing.Contrib.NetCore.Configuration;
using OpenTracing.Util;
using Prometheus;
using Jaeger.Senders.Thrift;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Configuration
        .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: false, reloadOnChange: true)
        .AddEnvironmentVariables();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));
builder.Services
    .AddRepositories()
    .AddInfrastructure(builder.Configuration)
    .AddHandlers();
IServiceProvider serviceProvider = builder.Services.BuildServiceProvider();
var env = builder.Environment.EnvironmentName;

if (env != null && env == "Cloud")
{
    builder.WebHost.ConfigureKestrel(options =>
    {
        options.ListenAnyIP(int.Parse(builder.Configuration["HttpPort"]));
        options.ListenAnyIP(int.Parse(builder.Configuration["HttpsPort"]));
        options.ListenAnyIP(int.Parse(builder.Configuration["GrpcDruzina:AccommodationGrading:Port"]), listenOptions =>
        {
            listenOptions.Protocols = HttpProtocols.Http2;
        });
    });
    Server server = new Server
    {
        Services = { HostGradeGrpcService.BindService(new HostGradeServerGrpcServiceImpl(serviceProvider.GetRequiredService<IHostGradingRepository>())) },
    };
    server.Start();
}
else
{
    Server server = new Server
    {
        Services = { HostGradeGrpcService.BindService(new HostGradeServerGrpcServiceImpl(serviceProvider.GetRequiredService<IHostGradingRepository>())) },
        Ports = { new ServerPort("0.0.0.0", int.Parse(builder.Configuration["GrpcDruzina:AccommodationGrading:Port"]), ServerCredentials.Insecure) }
    };
    server.Start();
}


builder.Services.AddControllers();
builder.Services.AddSwaggerGen(c =>
{
    // KeyCloak
    c.CustomSchemaIds(type => type.ToString());
    var securityScheme = new OpenApiSecurityScheme
    {
        Name = "KEYCLOAK",
        Type = SecuritySchemeType.OAuth2,
        In = ParameterLocation.Header,
        BearerFormat = "JWT",
        Scheme = "bearer",
        Flows = new OpenApiOAuthFlows
        {
            AuthorizationCode = new OpenApiOAuthFlow
            {
                AuthorizationUrl = new Uri(builder.Configuration["Jwt:AuthorizationUrl"]),
                TokenUrl = new Uri(builder.Configuration["Jwt:TokenUrl"]),
                Scopes = new Dictionary<string, string> { }
            }
        },
        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }
    };
    c.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);
    c.AddSecurityRequirement(new OpenApiSecurityRequirement{
                                                {securityScheme, new string[] { }}
                                            });
});

// KeyCloak
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

}).AddJwtBearer(o =>
{
    o.Authority = builder.Configuration["Jwt:Authority"];
    o.Audience = builder.Configuration["Jwt:Audience"];
    o.RequireHttpsMetadata = false;
    o.Events = new JwtBearerEvents()
    {
        OnAuthenticationFailed = c =>
        {
            c.NoResult();

            c.Response.StatusCode = 500;
            c.Response.ContentType = "text/plain";

            // Debug only for security reasons
            // return c.Response.WriteAsync(c.Exception.ToString());

            return c.Response.WriteAsync("An error occured processing your authentication.");
        }
    };
});

// Cross-Origin 
builder.Services
    .AddCors(options =>
    {
        options.AddPolicy("AllowOrigin",
            builder => builder.WithOrigins("*")
                              .AllowAnyHeader()
                              .AllowAnyMethod());
    });

builder.Services.AddGrpc();

builder.Services.AddOpenTracing();

builder.Services.AddSingleton<ITracer>(sp =>
{
    var serviceName = sp.GetRequiredService<IWebHostEnvironment>().ApplicationName;
    var loggerFactory = sp.GetRequiredService<ILoggerFactory>();
    var reporter = new RemoteReporter.Builder()
                    .WithLoggerFactory(loggerFactory)
                    .WithSender(new UdpSender("host.docker.internal", 6831, 0))
                    .Build();
    var tracer = new Tracer.Builder(serviceName)
        // The constant sampler reports every span.
        .WithSampler(new ConstSampler(true))
        // LoggingReporter prints every reported span to the logging framework.
        .WithLoggerFactory(loggerFactory)
        .WithReporter(reporter)
        .Build();
    GlobalTracer.Register(tracer);
    return tracer;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "MyAppAPI");
        c.OAuthClientId(builder.Configuration["Jwt:ClientId"]);
        c.OAuthClientSecret(builder.Configuration["Jwt:ClientSecret"]);
        c.OAuthRealm(builder.Configuration["Jwt:Realm"]);
        c.OAuthAppName("KEYCLOAK");
    });
}

app.UseHttpsRedirection();

app.UseCors("AllowOrigin");

app.UseAuthentication();
app.UseAuthorization();
app.UseRouting();
app.UseGrpcWeb(new GrpcWebOptions { DefaultEnabled = true });

app.UseMetricServer();
app.UseHttpMetrics();

app.UseEndpoints(endpoints =>
{
    endpoints.MapMetrics();
    endpoints.MapGrpcService<HostGradeServerGrpcServiceImpl>();
});

app.MapControllers();

app.Run();
