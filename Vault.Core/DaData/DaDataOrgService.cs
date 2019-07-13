using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Vault.Core.Organizations;

namespace Vault.Core.DaData
{
    [UsedImplicitly]
    public class DaDataOrgService
    {
        public DaDataOrgService(
            DaDataClient client,
            DaDataOrgRepository repository,
            OrganizationRepository cacheRepository)
        {
            Client = client;
            Repository = repository;
            CacheRepository = cacheRepository;
        }

        private DaDataOrgRepository Repository { get; }
        private OrganizationRepository CacheRepository { get; }
        private DaDataClient Client { get; }

        public async Task<Organization> GetByIdAsync(string id) => await CacheRepository.FindById(id);

        public async Task<Organization[]> GetByInnAsync(string inn, PartyBranchType? branchType)
        {
            var cachedOrgs = await CacheRepository.FindByInn(inn, branchType);

            if (cachedOrgs.Any())
                return cachedOrgs;

            await UpsertAsync(inn, findById: true, branchType: branchType);
            return await CacheRepository.FindByInn(inn, branchType);
        }

        public async Task<Organization[]> GetByOgrnAsync(string ogrn, PartyBranchType? branchType)
        {
            var cachedOrgs = await CacheRepository.FindByOgrn(ogrn, branchType);

            if (cachedOrgs.Any())
                return cachedOrgs;

            await UpsertAsync(ogrn, findById: true, branchType: branchType);
            return await CacheRepository.FindByOgrn(ogrn, branchType);
        }

        public async Task<Organization[]> GetByQueryAsync(string query)
        {
            var cachedOrgs = await CacheRepository.FindByQuery(query);

            if (cachedOrgs.Any())
                return cachedOrgs;
            
            await UpsertAsync(query, findById: false);
            return await CacheRepository.FindByQuery(query);
        }

        private async Task UpsertAsync(string inn, bool findById, PartyBranchType? branchType = null)
        {
            var orgs = await Client.GetOrgsAsync(inn, findById, branchType);

            if (!orgs.Any())
                return;

            await Repository.UpsertAsync(orgs);
            await CacheRepository.UpsertAsync(orgs);
        }


    }
}
