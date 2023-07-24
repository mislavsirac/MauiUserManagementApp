
using MobileApp.Models;
using Newtonsoft.Json;
using System.IO;
using System.Net.Http;
using System.Text;

namespace MobileApp;

public partial class AddUser : ContentPage
{
    public List<Country> Countries { get; set; }
    private static readonly HttpClient httpClient = new HttpClient();
    public byte[] ClientImage { get; set; }
    public AddUser()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadCountries();
        Picker.ItemsSource = Countries;
    }

    void BackButton_Clicked(object sender, EventArgs e)
    {
        App.Current.MainPage = new NavigationPage(new MainPage());
    }

    public async void ImageButton_Clicked(object sender, EventArgs e)
    {
        await DoImageButtonClicked();
    }

    public async Task DoImageButtonClicked()
    {
        if (MediaPicker.Default.IsCaptureSupported)
        {
            FileResult photo = await MediaPicker.Default.PickPhotoAsync();

            if (photo != null)
            {
                FileName.Text = "Image name: " + photo.FileName;
                FileName.IsVisible = true;

                using Stream sourceStream = await photo.OpenReadAsync();
                using var memoryStream = new MemoryStream();
                await sourceStream.CopyToAsync(memoryStream);
                ClientImage = memoryStream.ToArray();
            }
        }
    }



    public async Task LoadCountries()
    {
        var response = await httpClient.GetAsync("https://localhost:44304/Country");

        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            Countries = JsonConvert.DeserializeObject<List<Country>>(content);
        }
        else
        {
            string toastMessage = $"Error: {response.StatusCode}";
            await DisplayAlert("Something went wrong", toastMessage, "Ok");
        }
    }

    public async void Submit_CLicked(object sender, EventArgs e)
    {
        try
        {
            User user = new User()
            {
                FirstName = FirstNameInput.Text,
                LastName = LastNameInput.Text,
                Username = Username.Text,
                Email = Email.Text,
                Password = Password.Text,
                CountryID = ((Country)Picker.SelectedItem).Id,
                ProfileImage = ClientImage
            };


            var jsonUser = JsonConvert.SerializeObject(user);
            HttpContent httpContent = new StringContent(jsonUser, Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync("https://localhost:44304/User", httpContent);


            if (response.IsSuccessStatusCode)
            {

                await this.DisplayAlert("Success", "User succesfully added", "Ok");
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
}