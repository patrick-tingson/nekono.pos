using Nekono.App.Pos.Models;
using Nekono.App.Pos.Utility;
using Nekono.App.Pos.Views;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace Nekono.App.Pos.Services
{
    public class ItemsServices : IItemsServices
    {
        private readonly HttpClient httpClient = new HttpClient();
        private string API_POS_ITEMS = "v1/item/pos";
        private string API_POS_INVENTORY = "v1/inventory/collectionreceipt";

        public ItemsServices()
        {
        }

        public async Task<List<ItemInPOSDetails>> GetItemInPOSDetails()
        {
            var result = new List<ItemInPOSDetails>();

            try
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", SecureStorage.GetAsync("access_token").Result);
                var apiResult = await httpClient.GetAsync($"{Variables.APIEndpoint()}{API_POS_ITEMS}");

                if (apiResult.IsSuccessStatusCode)
                {
                    result = JsonConvert.DeserializeObject<List<ItemInPOSDetails>>(
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

        public async Task<List<InventoryDetails>> GetItemsInReceipt(string collectionReceipt)
        {
            var result = new List<InventoryDetails>();

            try
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", SecureStorage.GetAsync("access_token").Result);
                var apiResult = await httpClient.GetAsync($"{Variables.APIEndpoint()}{API_POS_INVENTORY}/{collectionReceipt}");

                if (apiResult.IsSuccessStatusCode)
                {
                    result = JsonConvert.DeserializeObject<List<InventoryDetails>>(
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
    }
}
