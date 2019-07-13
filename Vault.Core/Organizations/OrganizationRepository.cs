using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using JetBrains.Annotations;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Vault.Core.DaData;
using Vault.Core.Exceptions;
using Vault.Helpers;

namespace Vault.Core.Organizations
{
    [UsedImplicitly]
    public class OrganizationRepository
    {
        public OrganizationRepository(IMongoDatabase database, IMapper mapper)
        {
            Mapper = mapper;
            Collection = database.GetCollection<Organization>("Organizations");
        }

        private IMongoCollection<Organization> Collection { get; }
        private IMapper Mapper { get; }

        public async Task<Organization> FindById(string id)
            => await Collection
                   .AsQueryable()
                   .FirstOrDefaultAsync(x => x.Id == id)
               ?? throw new VaultNotFoundException($"Организация {id} не найдена");

        public async Task<Organization[]> FindByInn(string inn, PartyBranchType? branchType)
            => await Collection
                .AsQueryable()
                .Where(x => x.Inn == inn)
                .WhereIf(branchType != null, w => w.BranchType == branchType)
                .ToArrayAsync();

        public async Task<Organization[]> FindByOgrn(string ogrn, PartyBranchType? branchType)
            => await Collection
                .AsQueryable()
                .Where(x => x.Ogrn == ogrn)
                .WhereIf(branchType != null, w => w.BranchType == branchType)
                .ToArrayAsync();

        public async Task<Organization[]> FindByQuery(string query)
        {
            Guard.NotNull(query, nameof(query));

            var search = query
                .Split(' ')
                .Select(s => $"\"{s}\"")
                .Apply(x => string.Join(" ", x));

            return await Builders<Organization>
                .Filter
                .Text(search)
                .Apply(x => Collection.Find(x))
                .Apply(x => x.ToListAsync())
                .Then(x => x.ToArray());
        }

        public async Task<int> UpsertAsync(DaDataOrganization[] sourceOrgs)
        {
            Guard.NotNull(sourceOrgs, nameof(sourceOrgs));

            return await Mapper
                .Map<Organization[]>(sourceOrgs.Select(x => x.Data))
                .Apply(x => x.Select(org => new ReplaceOneModel<Organization>(FilterByHid(org), org) { IsUpsert = true }))
                .Apply(x => Collection.BulkWriteAsync(x))
                .Then(x => x.RequestCount);

            FilterDefinition<Organization> FilterByHid(Organization org)
                => Builders<Organization>.Filter.Eq(x => x.Hid, org.Hid);
        }
    }
}
