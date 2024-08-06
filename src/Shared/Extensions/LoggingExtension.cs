using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace Shared.Extensions;

public static class LoggingExtension
{
    public static void ConfigureLogstash(this
        WebApplicationBuilder builder)
    {
        builder.Host
            .UseSerilog((context, configuration) =>
            {
                var env = context.HostingEnvironment;
                var configurationRoot = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional:true)
                    .Build();

                configuration
                    .ReadFrom.Configuration(configurationRoot)
                    .WriteTo.Http(configurationRoot["LogstashConfiguration:Uri"], null);
            });
    }
}