using Nekono.App.Pos.Models;
using Nekono.App.Pos.Services;
using Nekono.App.Pos.Utility;
using Nekono.App.Pos.Views;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Nekono.App.Pos.ViewModels
{

    public class LoginViewModel : BaseViewModel
    {
        public Command LoginCommand { get; set; }

        string username = string.Empty;
        public string Username
        {
            get { return username; }
            set { SetProperty(ref username, value); }
        }

        string password = string.Empty;
        public string Password
        {
            get { return password; }
            set { SetProperty(ref password, value); }
        }

        public LoginViewModel()
        {
            LoginCommand = new Command(async () => await ExecuteLoginCommand());
        }

        async Task ExecuteLoginCommand()
        {
            IsBusy = true;

            try
            {
                if(Username.Length == 0 || Username.Length < 5)
                {
                    await App.Current.MainPage.DisplayAlert("Error Encountered", "Invalid username.", "Ok");

                    return;
                } else if (Password.Length == 0 || Password.Length < 5)
                {
                    await App.Current.MainPage.DisplayAlert("Error Encountered", "Invalid password.", "Ok");
                    
                    return;
                }

                var hashedPassword = Signature.SHA1(Password);

                var loginRequest = new LoginRequest
                {
                    Password = hashedPassword,
                    Username = Username,
                    Signature = Signature.Create(Username, hashedPassword)
                };

                var authenticateResult = await LoginServices.Authenticate(loginRequest);

                if(authenticateResult.AccessToken != null)
                {
                    SecureStorage.RemoveAll();

                    await SecureStorage.SetAsync("access_token", authenticateResult.AccessToken);
                    await SecureStorage.SetAsync("username", Username);

                    App.Current.MainPage = new MainPage();
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("Error Encountered", authenticateResult.Error, "Ok");
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Error Encountered", ex.Message, "Ok");
            }
            finally
            {
                IsBusy = false;
            }
        }

        public async Task ExecuteCheckExistingToken()
        {
            IsBusy = true;

            try
            {
                var getExisitngAccessToken = await SecureStorage.GetAsync("access_token");

                if (getExisitngAccessToken != null)
                {
                    await ItemsStore.GetItemsAsync();

                    App.Current.MainPage = new MainPage();
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Error Encountered", ex.Message, "Ok");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}