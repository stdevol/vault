using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Vault.Helpers;

namespace Vault.Core.DaData
{
    [PublicContract]
    public class DaDataClient
    {
        public DaDataClient(HttpClient client) 
            => Client = client;

        private HttpClient Client { get; }

        public async Task<DaDataOrganization[]> GetOrgsAsync(string query, bool findById, PartyBranchType? branchType = null)
        {
            var url = findById ? "findById/party" : "suggest/party";
            var response = await Client.PostAsync(url, GetStringContent());

            await response.EnsureSuccessStatusCodeAsync("Не удалось отправить запрос в DaData");

            var partyResponse = await response.Content.ReadAsStringAsync()
                .Then(x => JsonConvert.DeserializeObject<SuggestPartyResponse>(x, GetJsonSerializerSettings()));

            return partyResponse?.Suggestions ?? Array.Empty<DaDataOrganization>();

            JsonSerializerSettings GetJsonSerializerSettings()
                => new JsonSerializerSettings
                {
                    ContractResolver = new DefaultContractResolver { NamingStrategy = new SnakeCaseNamingStrategy() }
                };

            StringContent GetStringContent()
                => new StringContent(
                    JsonConvert.SerializeObject(new { query, branch_type = branchType, count = 20 }),
                    Encoding.UTF8,
                    "application/json");
        }
    }
}
