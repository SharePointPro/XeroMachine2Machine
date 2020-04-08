using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using XeroMachine2Machine.Model;

namespace XeroMachine2Machine.Helpers
{
    public class TokenHelper
    {
        private readonly string _clientId;

        public TokenHelper(string clientId)
        {
            this._clientId = clientId;
        }

        /// <summary>
        /// POST Xero to refresh token
        /// </summary>
        /// <param name="refreshToken"></param>
        /// <returns></returns>
        public async Task<RefreshTokenResponse> Refresh(string refreshToken)
        {
            var client = new HttpClient();

            HttpContent content = new FormUrlEncodedContent(new[]
             {
                new KeyValuePair<string, string>("grant_type", "refresh_token"),
                new KeyValuePair<string, string>("client_id", _clientId),
                new KeyValuePair<string, string>("refresh_token", refreshToken)
            });

            using (var requestMessage = new HttpRequestMessage(System.Net.Http.HttpMethod.Post, "https://identity.xero.com/connect/token"))
            {
                requestMessage.Content = content;
                HttpResponseMessage result = await client.SendAsync(requestMessage);
                string json = await result.Content.ReadAsStringAsync();
                if (result.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var refreshTokenResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<RefreshTokenResponse>(json);
                    //Give 5 minute grace period
                    refreshTokenResponse.expire_date = DateTime.Now.AddSeconds(refreshTokenResponse.expires_in).AddMinutes(-5);
                    return refreshTokenResponse;
                }
                else
                {
                    throw new HttpRequestException(await result.Content.ReadAsStringAsync());
                }
            }
        }

        /// <summary>
        /// Get token details from file, if access token is expired, refresh it and save back to file
        /// return valid access token
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetAccessToken()
        {
            var tokenDetails = FileHelper.ReadFile();
            if (tokenDetails.expire_date > DateTime.Now)
            {
               tokenDetails = await Refresh(tokenDetails.refresh_token);
               FileHelper.WriteFile(tokenDetails);
            }
            return tokenDetails.access_token;
        }
 

    }
}
