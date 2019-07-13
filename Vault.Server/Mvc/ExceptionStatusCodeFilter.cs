using System.Net;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Vault.Helpers;

namespace Vault.Server.Mvc
{
    public class ExceptionStatusCodeFilter<T> : IExceptionFilter, IOrderedFilter
    {
        private int StatusCode { get; }
        public int Order { get; }

        public ExceptionStatusCodeFilter(int statusCode = StatusCodes.Status400BadRequest, int order = 0)
        {
            StatusCode = Guard.Positive(statusCode, nameof(statusCode));
            Order = order;
        }

        public void OnException(ExceptionContext context)
        {
            if (!(context.Exception is T))
                return;

            var result = new ErrorInfo(StatusCode, context.Exception.Message);
            context.Result = result.ToObjectResult();
            context.ExceptionHandled = true;
        }


        [UsedImplicitly(ImplicitUseTargetFlags.Members)]
        public class ErrorInfo
        {
            public int StatusCode { get; set; }
            public string Message { get; set; }

            public ErrorInfo(int statusCode, string message = null)
            {
                StatusCode = statusCode;
                Message = message ?? ((HttpStatusCode)statusCode).ToString();
            }

            public ObjectResult ToObjectResult()
                => new ObjectResult(this) { StatusCode = StatusCode };
        }
    }
}
