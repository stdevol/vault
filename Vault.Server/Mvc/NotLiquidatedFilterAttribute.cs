using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Vault.Core.DaData;
using Vault.Core.Organizations;

namespace Vault.Server.Mvc
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class NotLiquidatedFilterAttribute : Attribute, IAsyncResultFilter
    {
        public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            if (context.Cancel)
                return;
            
            if (!(context.Result is ObjectResult objectResult) || !(objectResult.Value is Organization[] query))
                throw new Exception($"Actions marked with {nameof(NotLiquidatedFilterAttribute)} should return array of {nameof(Organization)}");

            var result = query.Where(x => x.Status != PartyStatus.Liquidated);
            context.Result = new ObjectResult(result);
            await next();
        }
    }
}
