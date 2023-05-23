using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using System.Reflection;
using Accomodation.Configuration;
using Grpc.Core;
using Notification.Application.Notification.Support.Grpc.Protos;
using Notification.Application.Notification.Support.Grpc;
using Rs.Ac.Uns.Ftn.Grpc;
using Notification.Domain.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));
builder.Services
    .AddRepositories()
    .AddHandlers();
IServiceProvider serviceProvider = builder.Services.BuildServiceProvider();
Server server = new Server
{
    Services = { GuestNotificationGrpcService.BindService(new ServerGrpcServiceImpl(serviceProvider.GetService<IGuestNotificationRepository>())) },
    Ports = { new ServerPort("localhost", 8787, ServerCredentials.Insecure) }
};
server.Start();
Server server1 = new Server
{
    Services = { HostRequestNotificationGrpcService.BindService(new HostRequestServerGrpcServiceImpl(serviceProvider.GetService<IHostNotificationRepository>())) },
    Ports = { new ServerPort("localhost", 8788, ServerCredentials.Insecure) }
};
server1.Start();
Server server2 = new Server
{
    Services = { HostCancelReservationNotificationGrpcService.BindService(new HostCancelReservationServerGrpcServiceImpl(serviceProvider.GetService<IHostNotificationRepository>())) },
    Ports = { new ServerPort("localhost", 8789, ServerCredentials.Insecure) }
};
server2.Start();
Server server3 = new Server
{
    Services = { HostGradingNotificationGrpcService.BindService(new HostGradingServerGrpcServiceImpl(serviceProvider.GetService<IHostNotificationRepository>())) },
    Ports = { new ServerPort("localhost", 8790, ServerCredentials.Insecure) }
};
server3.Start();
Server server4 = new Server
{
    Services = { AccommodationGradingNotificationGrpcService.BindService(new AccommodationGradingServerGrpcServiceImpl(serviceProvider.GetService<IHostNotificationRepository>())) },
    Ports = { new ServerPort("localhost", 8791, ServerCredentials.Insecure) }
};
server4.Start();
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
            builder => builder.WithOrigins("http://localhost:4200")
                              .AllowAnyHeader()
                              .AllowAnyMethod());
    });
builder.Services.AddGrpc();

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
app.UseEndpoints(endpoints =>
{
    endpoints.MapGrpcService<ServerGrpcServiceImpl>();
    endpoints.MapGrpcService<HostRequestServerGrpcServiceImpl>();
    endpoints.MapGrpcService<HostCancelReservationServerGrpcServiceImpl>();
    endpoints.MapGrpcService<HostGradingServerGrpcServiceImpl>();
    endpoints.MapGrpcService<AccommodationGradingServerGrpcServiceImpl>();
});

app.MapControllers();

app.Run();
