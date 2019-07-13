using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Vault.Core.DaData;
using Vault.Core.Organizations;
using Vault.Helpers;
using Vault.Server.App;
using Vault.Server.Mvc;

namespace Vault.Server.Controllers
{
    //[Authorize]
    [AllowAnonymous, EnableCors(VaultCors.Sites)]
    [Route("organization")]
    public class OrganizationController : ControllerBase
    {
        private readonly DaDataOrgService _daDataOrgService;

        public OrganizationController(DaDataOrgService daDataOrgService)
            => _daDataOrgService = Guard.NotNull(daDataOrgService, nameof(daDataOrgService));

        [HttpGet("id/{id}")]
        public Task<Organization> GetOrganizationsByInn(string id)
            => _daDataOrgService.GetByIdAsync(id);

        [HttpGet("inn/{inn}"), NotLiquidatedFilter]
        public Task<Organization[]> GetOrganizationsByInn(string inn, [FromQuery]PartyBranchType? branchType)
            => _daDataOrgService.GetByInnAsync(inn, branchType);

        [HttpGet("ogrn/{ogrn}"), NotLiquidatedFilter]
        public Task<Organization[]> GetOrganizationsByOgrn(string ogrn, [FromQuery]PartyBranchType? branchType)
            => _daDataOrgService.GetByOgrnAsync(ogrn, branchType);

        [HttpGet("query"), NotLiquidatedFilter]
        public Task<Organization[]> GetOrganizationsByQuery([FromQuery]string search)
            => _daDataOrgService.GetByQueryAsync(search);
    }
}
