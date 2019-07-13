using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Vault.Server.App;

namespace Vault.Server
{
    public class Program
    {
        public static async Task Main()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            await new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .ConfigureLogging(ConfigureLogging)
                .ConfigureAppConfiguration(ConfigureAppConfiguration)
                .UseStartup<Startup>()
                .Build()
                .RunAsync();
        }

        private static void ConfigureLogging(WebHostBuilderContext context, ILoggingBuilder builder)
            => builder
                .AddFilter("IdentityModel", LogLevel.Information)
                .AddFilter(typeof(ExceptionHandlerMiddleware).FullName, LogLevel.Error)
                .AddFilter(typeof(XmlKeyManager).FullName, LogLevel.Error)
                .AddVault();


        private static void ConfigureAppConfiguration(WebHostBuilderContext context, IConfigurationBuilder builder)
            => builder
                .SetBasePath(context.HostingEnvironment.ContentRootPath)
                .AddEnvironmentVariables();
    }
}