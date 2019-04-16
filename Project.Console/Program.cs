using IdentityModel.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Project.Console
{
    class Program
    {
        public static void Main(string[] args) => MainAsync().GetAwaiter().GetResult();

        private static async Task MainAsync()
        {
            //discover all end points using metadata if Identity Server
            var discoRO = await DiscoveryClient.GetAsync("http://localhost:5000");
            if (discoRO.IsError)
            {
                System.Console.WriteLine(discoRO.Error);
                return;
            }

            // Grab a Bearer Token using Resource Owner Password
            var tokenClientRO = new TokenClient(discoRO.TokenEndpoint, "client", "secret");
            var tokenResponseRO = await tokenClientRO.RequestResourceOwnerPasswordAsync("tifa", "password", "projectApi");

            if (tokenResponseRO.IsError)
            {
                System.Console.WriteLine(tokenResponseRO.Error);
                return;
            }

            System.Console.WriteLine(tokenResponseRO.Json);
            System.Console.WriteLine("\n\n");







            //discover all end points using metadata if Identity Server
            var disco = await DiscoveryClient.GetAsync("http://localhost:5000");
            if (disco.IsError)
            {
                System.Console.WriteLine(disco.Error);
                return;
            }

            // Grab a Bearer Token
            var tokenClient = new TokenClient(disco.TokenEndpoint, "client", "secret");
            var tokenResponse = await tokenClient.RequestClientCredentialsAsync("projectApi");

            if (tokenResponse.IsError)
            {
                System.Console.WriteLine(tokenResponse.Error);
                return;
            }

            System.Console.WriteLine(tokenResponse.Json);
            System.Console.WriteLine("\n\n");

            //Consume our Customer API
            var client = new HttpClient();
            client.SetBearerToken(tokenResponse.AccessToken);

            var customerInfo = new StringContent(
                JsonConvert.SerializeObject(
                    new
                    {
                        Id = 1,
                        FirstName = "Mostafa",
                        LastName = "Bayomi"
                    }
                    ), Encoding.UTF8, "application/json");

            var createCustomerResponse = await client.PostAsync("http://localhost:49779/api/customers", customerInfo);

            if (!createCustomerResponse.IsSuccessStatusCode)
            {
                System.Console.WriteLine(createCustomerResponse.StatusCode);
            }

            var getCustomerResponse = await client.GetAsync("http://localhost:49779/api/customers");

            if (!getCustomerResponse.IsSuccessStatusCode)
            {
                System.Console.WriteLine(getCustomerResponse.StatusCode);
            }
            else
            {
                var content = await getCustomerResponse.Content.ReadAsStringAsync();
                System.Console.WriteLine(JArray.Parse(content));
            }

            System.Console.Read();
        }
    }
}
