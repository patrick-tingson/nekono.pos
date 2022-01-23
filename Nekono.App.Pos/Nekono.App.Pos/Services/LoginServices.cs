using Nekono.App.Pos.Models;
using Nekono.App.Pos.Utility;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Nekono.App.Pos.Services
{
    public class LoginServices : ILoginServices
    {
        private readonly HttpClient httpClient = new HttpClient();
        private string API_LOGIN = "v1/login";

        public LoginServices()
        {
            Debug.WriteLine("Start");
        }

        public async Task<LoginResponse> Authenticate(LoginRequest loginRequest)
        {
            var result = new LoginResponse();

            try
            {
                var httpContent = new StringContent(JsonConvert.SerializeObject(loginRequest), Encoding.UTF8, "application/json");

                var authenticateResult = await httpClient.PostAsync($"{Variables.APIEndpoint()}{API_LOGIN}", httpContent);

                if (authenticateResult.IsSuccessStatusCode)
                {
                    result = JsonConvert.DeserializeObject<LoginResponse>(
                        await authenticateResult.Content.ReadAsStringAsync());
                }
                else
                {
                    result.Error = $"{authenticateResult.StatusCode} {await authenticateResult.Content.ReadAsStringAsync()}";
                }
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }

            return result;
        }
    }
}
