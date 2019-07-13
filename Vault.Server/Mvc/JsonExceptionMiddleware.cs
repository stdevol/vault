using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Vault.Helpers;

namespace Vault.Server.Mvc
{
    public class JsonExceptionMiddleware
    {
        private RequestDelegate Next { get; }
        private JsonSerializerSettings SerializerSettings { get; }
        private IHostingEnvironment Environment { get; }

        public JsonExceptionMiddleware(RequestDelegate next, IOptions<MvcJsonOptions> options, IHostingEnvironment environment)
        {
            Next = next;
            SerializerSettings = options.Value.SerializerSettings;
            Environment = environment;
        }

        public async Task Invoke(HttpContext context, ILogger<JsonExceptionMiddleware> logger)
        {
            var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;
            if (exception == null)
            {
                await Next(context);
                return;
            }

            logger.LogError(exception, "Unhandled exception in {Method} {RequestPath}: {ExceptionType} - {ExceptionMessage}",
                context.Request.Method, context.Request.Path, exception.GetType().Name, exception.Message);

            var result = ExceptionInfo.Create(exception, !Environment.IsProduction());
            var json = JsonConvert.SerializeObject(result, SerializerSettings);

            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "application/json; charset=utf-8";
            await context.Response.WriteAsync(json, Encoding.UTF8);
        }

        [PublicContract]
        private class ExceptionInfo
        {
            public static ExceptionInfo Create(Exception exception, bool includeStackTrace)
                => new ExceptionInfo
                {
                    Type = exception.GetType().Name,
                    Message = exception.Message.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries),
                    StackTrace = includeStackTrace ? Split(exception) : null,
                };

            private static string[] Split(Exception exception)
            {
                try
                {
                    return exception
                        .StackTrace
                        .Split('\n', '\r')
                        .Where(x => !x.Contains("Microsoft.AspNetCore.Mvc.Internal"))
                        .Where(x => !string.IsNullOrWhiteSpace(x))
                        .ToArray();
                }
                catch (Exception e)
                {
                    return new[] { $"No StackTrace: {e.Message}" };
                }
            }


            public string Type { get; set; }
            public string[] Message { get; set; }

            [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
            public string[] StackTrace { get; set; }
        }
    }
}
