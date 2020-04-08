using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace XeroMachine2Machine.Helpers
{
    public class TenantHelper
    {
        private const string OUTPUT_FILE_NAME = "tenants.txt";

        public static async Task GetTenants(string accessToken)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Authorization", accessToken);
            using (var requestMessage = new HttpRequestMessage(System.Net.Http.HttpMethod.Get, "https://api.xero.com/connections"))
            {
                HttpResponseMessage result = await client.SendAsync(requestMessage);
                string json = await result.Content.ReadAsStringAsync();
                if (result.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    System.IO.File.WriteAllText(OUTPUT_FILE_NAME, json);
                    Console.WriteLine("Tenant Details have been written to " + OUTPUT_FILE_NAME);
                    Console.ReadLine();
                }
                else
                {
                    throw new HttpRequestException(await result.Content.ReadAsStringAsync());
                }
            }

        }
    }
}
