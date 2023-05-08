using System.Security.AccessControl;
using System.Security.Cryptography;
using System.Text;

namespace MAUI_Coursework.Views;

public partial class LoginPage : ContentPage
{
    RegistrationPage registrationPage = new();
    Data.CourseworkDatebase courseworkDatebase = new();
    Models.Users users = new();
    public LoginPage()
	{
		InitializeComponent();
        ReCheckAdmin();
    }
    private async void ReCheckAdmin()
    {
        Models.Users userrrr = await courseworkDatebase.GetUserAsync(1);
        if (userrrr == null) 
        {
            userrrr = new Models.Users()
            {
                Login = "admin",
                Password = "8C6976E5B5410415BDE908BD4DEE15DFB167A9C873FC4BB8A81F6F2AB448A918",
                Role = 1
            };
            await courseworkDatebase.SaveUserAsync(userrrr);
        }
    }
    private async void Login_Clicked(object sender, EventArgs e)
    {
        if (login_entry.Text != "" && password_entry.Text != "") 
        {
            users = await courseworkDatebase.GetUserAsync(login_entry.Text);
            if (users != null) 
            {
                using SHA256 sha256Hash = SHA256.Create();
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password_entry.Text));
                string hashedPassword = BitConverter.ToString(bytes).Replace("-", "");
                if (users.Password == hashedPassword)
                {
                    MauiProgram.loginGlobal = users.Login;
                    MauiProgram.roleGlobal = users.Role;
                    MauiProgram.idGlobal = users.ID;
                    //await DisplayAlert($"Привет {MauiProgram.loginGlobal}", $"Твоя роль {MauiProgram.roleGlobal}", "OK");
                    Application.Current.MainPage = new AppShell();
                } else
                {
                    await DisplayAlert("Ошибка", "Неверный пароль", "OK");
                }
            } else
            {
                await DisplayAlert("Ошибка", "Такого пользователя нет", "OK");
            }
        } else
        {
            await DisplayAlert("Ошибка", "Не все поля заполнены", "OK");
        }

    }

    private async void Registration_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(registrationPage);
    }

    private void HardLogin_Clicked(object sender, EventArgs e)
    {
        MauiProgram.loginGlobal = "admin";
        MauiProgram.roleGlobal = 1;
        MauiProgram.idGlobal = 1;
        Application.Current.MainPage = new AppShell();
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        QRPage qrPage = new();
        await Navigation.PushAsync(qrPage);
    }
}