using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using MongoDB.Driver;
using Vault.Helpers;

namespace Vault.Core.DaData
{
    [UsedImplicitly]
    public class DaDataOrgRepository
    {
        public DaDataOrgRepository(IMongoDatabase database) 
            => Collection = database.GetCollection<DaDataOrganization>("DaDataOrganizations");

        internal IMongoCollection<DaDataOrganization> Collection { get; }

        public async Task<int> UpsertAsync([NotNull] DaDataOrganization[] organizations)
        {
            Guard.NotNull(organizations, nameof(organizations));

            return await organizations
                .Select(org => new ReplaceOneModel<DaDataOrganization>(Filter(org), org) { IsUpsert = true })
                .Apply(x => Collection.BulkWriteAsync(x))
                .Then(x => x.RequestCount);

            FilterDefinition<DaDataOrganization> Filter(DaDataOrganization org)
                => Builders<DaDataOrganization>.Filter.Eq(x => x.Data.Hid, org.Data.Hid);
        }

    }
}