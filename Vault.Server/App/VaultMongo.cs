using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace Vault.Server.App
{
    public static class VaultMongo
    {
        public static IServiceCollection AddMongo(this IServiceCollection services, string mongoConnectionString)
            => services
                .AddSingleton(s =>
                {
                    var murl = new MongoUrl(mongoConnectionString);
                    var mongoClient = new MongoClient(murl);
                    return mongoClient.GetDatabase(murl.DatabaseName);
                });
    }
}
