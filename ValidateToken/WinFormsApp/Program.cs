using Data.Context;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using System.Threading.Tasks;
using System;
using ValidateToken.Application;
using ValidateToken.Repository;
using ValidateToken.Service;
using System.Windows.Forms;

namespace WinFormsApp 
{
    public class Program
    {
        private static ServiceProvider _serviceProvider;

        [STAThread]
        public static async Task Main(string[] strings)
        {
            ApplicationConfiguration.Initialize();

            var configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                                                          .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                                                          .AddEnvironmentVariables()
                                                          .Build();

            var services = new ServiceCollection();
            services.AddOptions();
            services.AddSingleton<IConfiguration>(configuration);
            services.AddScoped<DataContext>();
            services.AddScoped<IValidateTokenRepository, ValidateTokenRepository>();
            services.AddScoped<IValidateTokenService, ValidateTokenService>();
            services.AddScoped<IWorker, Worker>();

            _serviceProvider = services.BuildServiceProvider();

            Application.Run(new FormMain());

            await _serviceProvider.DisposeAsync();
        }

        public static ServiceProvider GetServiceProvider()
        {
            return _serviceProvider;
        }
    }

}