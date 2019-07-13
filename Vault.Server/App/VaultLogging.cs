using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Extensions.Logging;

namespace Vault.Server.App
{
    public static class VaultLogging
    {
        public static void AddVault(this ILoggingBuilder builder)
        {
            var logger = new LoggerConfiguration()
                .WriteTo.LiterateConsole()
                .CreateLogger();

            builder.AddProvider(new SerilogLoggerProvider(logger, true));
        }
    }
}
