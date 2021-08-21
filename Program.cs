using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace LoggingSample
{
    class Program
    {
        static void Main(string[] args)
        {
            IServiceCollection service = new ServiceCollection();
            service.AddSingleton<IConfiguration>(_ => new ConfigurationBuilder()
                            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                            .AddJsonFile("appSettings.json", optional: true, reloadOnChange: true)
                            .Build());

            ServiceProvider serviceProvider = service.BuildServiceProvider();
            var configuration = serviceProvider.GetService<IConfiguration>();

            service.AddLogging(configure =>
            {
                configure.ClearProviders();

                configure.AddConfiguration(configuration.GetSection("Logging"));
                configure.AddConsole(opt =>
                {
                }).AddConsoleFormatter<CustomTimePrefixingFormatter, CustomWrappingConsoleFormatterOptions>();
            }).AddTransient<MyApplication>()
            .AddTransient<MyApplication01>();

            serviceProvider = service.BuildServiceProvider();
            MyApplication app = serviceProvider.GetService<MyApplication>();
            MyApplication01 app01 = serviceProvider.GetService<MyApplication01>();

            try
            {
                app.Start();
                app01.Start();
            }
            catch (Exception e)
            {
                app.HandleError(e);
                app01.HandleError(e);
            }
            finally
            {
                app.Stop();
                app01.Stop();
            }

            ILoggerFactory loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
            ILogger<Program> logger = loggerFactory.CreateLogger<Program>();

            using (logger.BeginScope("Logging scope"))
            {
                logger.LogInformation("Hello World!");
                logger.LogInformation("The .NET developer community happily welcomes you.");
            }

            logger.LogInformation("Random log \x1B[42mwith green background\x1B[49m message");
        }
    }
}
