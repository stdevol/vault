using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Vault.Server.App
{
    public static class VaultCors
    {
        public const string Sites = "Sites";
        public const string Errors = "Errors";

        public static IServiceCollection AddVaultCors(this IServiceCollection services)
            => services
                .ConfigureOptions<ConfigureCorsOptions>()
                .AddCors();

        private class ConfigureCorsOptions : IPostConfigureOptions<CorsOptions>
        {
            private Dictionary<string, string[]> Origins { get; } = new Dictionary<string, string[]>();

            public ConfigureCorsOptions(IConfiguration configuration)
                => Origins.Add(Sites, configuration.Origins("SitesCorsOrigins"));

            public void PostConfigure(string name, CorsOptions cors)
            {
                cors.AddPolicy(Sites, x => x
                    .AllowAnyHeader()
                    .AllowCredentials()
                    .WithMethods("GET", "HEAD", "OPTIONS")
                    .SetPreflightMaxAge(TimeSpan.FromHours(24))
                    .WithOrigins(Origins[Sites]));

                cors.AddPolicy(Errors, x => x
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowAnyOrigin());
            }
        }

        private static string[] Origins(this IConfiguration configuration, string envName)
            => configuration[envName]?
                    .Split(";")
                    .Select(s => s.Trim())
                    .Distinct()
                    .Where(w => !string.IsNullOrWhiteSpace(w))
                    .ToArray();
    }
}