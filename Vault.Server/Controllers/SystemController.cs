using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace Vault.Server.Controllers
{
    [Route("system"), AllowAnonymous]
    public class SystemController : ControllerBase
    {
        [HttpGet, Route("info")]
        public object Info([FromServices] IHostingEnvironment env)
            => new { Environment = env.EnvironmentName };
    }
}
