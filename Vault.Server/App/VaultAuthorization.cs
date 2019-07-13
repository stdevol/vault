using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace Vault.Server.App
{
    public static class VaultAuthorization
    {
        public static IServiceCollection AddVaultAuthorization(this IServiceCollection services)
            => services
                .AddAuthorization(ConfigureAuthorization);

        private static void ConfigureAuthorization(AuthorizationOptions auth)
        {
            auth.DefaultPolicy = new AuthorizationPolicyBuilder()
                .RequireClaim("scope", "vault")
                .Build();
        }
    }
}