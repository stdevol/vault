using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace Vault.Helpers
{
    public static class HttpResponseExtensions
    {
        public static async Task<HttpResponseMessage> EnsureSuccessStatusCodeAsync(this HttpResponseMessage response, string errorMessage = null)
        {
            try
            {
                if (response.IsSuccessStatusCode)
                    return response;

                var statusText = $"{(int)response.StatusCode} ({response.StatusCode})";

                if (response.Content == null)
                    throw new HttpRequestException($"{statusText}: {response.ReasonPhrase}");

                using (response.Content)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    if (string.IsNullOrEmpty(json))
                        throw new HttpRequestException($"{statusText}: {response.ReasonPhrase}");

                    var error = JsonConvert.DeserializeObject<ErrorResponse>(json);
                    var exceptionText = $"{errorMessage}" +
                                        $"\r\nRequest URL: {response.RequestMessage.RequestUri}" +
                                        $"\r\n{error}";

                    throw new HttpRequestException(exceptionText);
                }
            }
            catch (JsonReaderException exception)
            {
                throw new HttpRequestException(
                    $"\r\nAn error occurred while deserializing object of type \"{typeof(ErrorResponse)}\": {exception.Message}" +
                    $"\r\nRequest URL: {response.RequestMessage.RequestUri}" +
                    $"\r\nStatus Code: {(int)response.StatusCode} {response.ReasonPhrase}");
            }
        }

        [PublicAPI]
        public class ErrorResponse
        {
            public string Message { get; set; }

            public string[] StackTrace { get; set; }

            public override string ToString()
                => $"Message: {Message}" +
                   $"\r\nStackTrace: {StackTrace?.Aggregate(new StringBuilder(StackTrace.Length / 2), (acc, x) => acc.AppendLine(x))}";
        }
    }
}
