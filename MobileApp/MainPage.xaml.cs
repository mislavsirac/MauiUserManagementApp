using MobileApp.Models;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MobileApp
{
    public partial class MainPage : ContentPage
    {
        List<User> users = new List<User>();
        List<UserListView> usersForDisplay = new List<UserListView>();
        private static readonly HttpClient httpClient = new HttpClient();

        public MainPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await FetchUsersAsync();
            foreach(var user in users)
            {
                UserListView userView = new UserListView();
                userView.ID = user.ID;
                userView.Email = user.Email;
                userView.FullName = user.FirstName + " " + user.LastName;

                byte[] imageAsBytes = user.ProfileImage;
                var stream = new MemoryStream(imageAsBytes);
                Image aImage = new Image
                {
                    Source = ImageSource.FromStream(() => stream)
                };
                userView.ProfileImage = aImage;
                usersForDisplay.Add(userView);
            }
            

            listView.ItemsSource= usersForDisplay;
            
        }

        async void OnAddUserClicked(object sender, EventArgs args)
        {
            App.Current.MainPage = new NavigationPage(new AddUser());
        }

        void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var user = (UserListView)e.SelectedItem;
            App.Current.MainPage = new NavigationPage(new UserDetails(user.ID));
        }

        private async Task FetchUsersAsync()
        {
            try
            {
                var response = await httpClient.GetAsync("https://localhost:44304/User");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    users = JsonConvert.DeserializeObject<List<User>>(content);
                }
                else
                {
                    string toastMessage = $"Error: {response.StatusCode}";
                    await this.DisplayAlert("Something went wrong", toastMessage, "Ok");
                }
            }catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());   
            }
            
        }
    }
}
