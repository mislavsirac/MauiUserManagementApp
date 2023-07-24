using MobileApp.Models;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

namespace MobileApp
{
    public partial class UserDetails : ContentPage
    {
        private static readonly HttpClient httpClient = new HttpClient();
        User user = new User();
        public UserDetails(int userId)
        {
            InitializeComponent();
            LoadUserDetails(userId);
        }

        private async void LoadUserDetails(int userId)
        {
            var response = await httpClient.GetAsync($"https://localhost:44304/User/{userId}");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                user = JsonConvert.DeserializeObject<User>(content);
                var responseCountry = await httpClient.GetAsync($"https://localhost:44304/Country/{user.CountryID}");
                var contentCountry = await responseCountry.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    var country = JsonConvert.DeserializeObject<Country>(contentCountry);
                    CountryLabel.Text = country.Name;
                }

                FirstNameLabel.Text = user.FirstName;
                LastNameLabel.Text = user.LastName;
                UsernameLabel.Text = user.Username;
                EmailLabel.Text = user.Email;
                ProfileImage.Source = ImageSource.FromStream(() => new MemoryStream(user.ProfileImage));

            }
            else
            {
                await DisplayAlert("Error", $"Failed to load user details: {response.StatusCode}", "OK");
            }
        }
        async void DeleteButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                var response = await httpClient.DeleteAsync($"https://localhost:44304/User/{user.ID}");


                if (response.IsSuccessStatusCode)
                {

                    await this.DisplayAlert("Success", "User succesfully deleted, returning to main screen", "Ok");
                    Thread.Sleep(600);
                    App.Current.MainPage = new NavigationPage(new MainPage());
                }
                else
                {
                    string toastMessage = $"Error: {response.StatusCode}";
                    await this.DisplayAlert("Something went wrong", toastMessage, "Ok");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        async void BackButton_Clicked(object sender, EventArgs e)
        {
            App.Current.MainPage = new NavigationPage(new MainPage());
        }
    }
}
