using Nekono.App.Pos.Models;
using Nekono.App.Pos.Utility;
using Nekono.App.Pos.Views;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace Nekono.App.Pos.Services
{
    public class CollectionReceiptServices : ICollectionReceiptServices
    {
        private readonly HttpClient httpClient = new HttpClient();
        private string API_RECEIPT_TODAY = "v1/collectionreceipt/today";
        private string API_RECEIPT_SALE = "v1/collectionreceipt/sale";
        private string API_RECEIPT_VOID = "v1/collectionreceipt/void";

        public CollectionReceiptServices()
        {
        }

        public async Task<List<CollectionReceiptDetails>> GetToday()
        {
            var result = new List<CollectionReceiptDetails>();

            try
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", SecureStorage.GetAsync("access_token").Result);
                var apiResult = await httpClient.GetAsync($"{Variables.APIEndpoint()}{API_RECEIPT_TODAY}");

                if (apiResult.IsSuccessStatusCode)
                {
                    result = JsonConvert.DeserializeObject<List<CollectionReceiptDetails>>(
                        await apiResult.Content.ReadAsStringAsync());
                }
                else if (apiResult.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    Functions.GoToLogin();
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("Error Encountered",
                        $"{apiResult.StatusCode} {await apiResult.Content.ReadAsStringAsync()}",
                        "Ok");
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Error Encountered", $"{ex.Message}", "Ok");
            }

            return result;
        }

        public async Task<string> Sale(CollectionReceiptDetails receiptDetails)
        {
            var result = "";

            try
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", SecureStorage.GetAsync("access_token").Result);
                var httpContent = new StringContent(JsonConvert.SerializeObject(receiptDetails), Encoding.UTF8, "application/json");

                var apiResult = await httpClient.PostAsync($"{Variables.APIEndpoint()}{API_RECEIPT_SALE}", httpContent);

                if (apiResult.IsSuccessStatusCode)
                {
                    result = await apiResult.Content.ReadAsStringAsync();
                }
                else if (apiResult.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    Functions.GoToLogin();
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("Error Encountered",
                        $"{apiResult.StatusCode} {await apiResult.Content.ReadAsStringAsync()}",
                        "Ok");
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Error Encountered", $"{ex.Message}", "Ok");
            }

            return result;
        }

        public async Task<bool> Void(DeleteRequest voidRequest)
        {
            var result = false;

            try
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", SecureStorage.GetAsync("access_token").Result);
                var httpContent = new StringContent(JsonConvert.SerializeObject(voidRequest), Encoding.UTF8, "application/json");

                var apiResult = await httpClient.PostAsync($"{Variables.APIEndpoint()}{API_RECEIPT_VOID}", httpContent);

                if (apiResult.IsSuccessStatusCode)
                {
                    result = true;
                }
                else if (apiResult.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    Functions.GoToLogin();
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("Error Encountered",
                        $"{apiResult.StatusCode} {await apiResult.Content.ReadAsStringAsync()}",
                        "Ok");
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Error Encountered", $"{ex.Message}", "Ok");
            }

            return result;
        } 
    }
}
