using System;
using System.Net.Http.Headers;
using IdentityServer4.AccessTokenValidation;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Vault.Core;
using Vault.Core.DaData;
using Vault.Core.Exceptions;
using Vault.Core.Organizations;
using Vault.Server.App;
using Vault.Server.Mvc;

namespace Vault.Server
{
    [UsedImplicitly(ImplicitUseTargetFlags.Members)]
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public VaultOptions Options { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            Options = new VaultOptions();
            configuration.Bind(Options);
        }

        public void ConfigureServices(IServiceCollection services)
        {
            BsonClassMapper.Tune();

            services
                .AddSingleton(Options)
                .AddSingleton(MapperConfigurator.CreateMapper())
                .AddMongo(Options.Mongo)
                .AddScoped<OrganizationRepository>()
                .AddScoped<DaDataOrgService>()
                .AddScoped<DaDataOrgRepository>();

            services
                .AddHealthChecks();

            services
                .AddVaultCors()
                .AddVaultAuthorization()
                .AddHttpClient<DaDataClient>(x =>
                {
                    x.BaseAddress = new Uri(Options.DaDataServer);
                    x.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", Options.DaDataToken);
                });

            services
                .Configure<MvcJsonOptions>(o =>
                {
                    o.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    o.SerializerSettings.Converters.Add(new StringEnumConverter());
                })
                .AddMvcCore()
                .AddFormatterMappings()
                .AddDataAnnotations()
                .AddJsonFormatters()
                .AddApiExplorer()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddMvcOptions(o => o.Filters.Add(new ExceptionStatusCodeFilter<VaultNotFoundException>(404, 10)));

            services
                .AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
                .AddIdentityServerAuthentication(options =>
                {
                    options.Authority = "https://auth.site.com";
                    options.ClaimsIssuer = "https://auth.site.com";

                    options.SupportedTokens = SupportedTokens.Jwt;
                    options.EnableCaching = true;
                });
        }

        public void Configure(IApplicationBuilder app)
        {
            app
                .UseHealthChecks("/health/status")
                .UseExceptionHandler(errApp => errApp
                    .UseMiddleware<JsonExceptionMiddleware>()
                    .UseCors(VaultCors.Errors))
                .UseAuthentication()
                .UseMvc()
                .Build();
        }
    }

    public class VaultOptions
    {
        public string Mongo { get; set; }
        public string DaDataServer { get; set; }
        public string DaDataToken { get; set; }
    }
}