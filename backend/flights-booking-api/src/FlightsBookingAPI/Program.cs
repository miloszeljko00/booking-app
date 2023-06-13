using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Hosting;

namespace FlightsBookingAPI
{
    /// <summary>
    /// Program
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Main
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        /// <summary>
        /// Create the host builder.
        /// </summary>
        /// <param name="args"></param>
        /// <returns>IHostBuilder</returns>
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                //    if (webBuilder.GetSetting("environment") == "Cloud")
                //    {
                //        webBuilder.ConfigureKestrel((context, options) =>
                //        {
                //            options.ListenAnyIP(int.Parse(context.Configuration["HttpPort"]));
                //            options.ListenAnyIP(int.Parse(context.Configuration["HttpsPort"]));
                //            options.ListenAnyIP(int.Parse(context.Configuration["GrpcDruzina:Letici:Port"]), listenOptions =>
                //            {
                //                listenOptions.Protocols = HttpProtocols.Http2;
                //            });
                //        });
                //    }
                    webBuilder.UseStartup<Startup>();
                });
    }
}
