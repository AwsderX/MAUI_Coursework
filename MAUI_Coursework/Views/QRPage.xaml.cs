using ZXing.Net.Maui;

namespace MAUI_Coursework.Views;

public partial class QRPage : ContentPage
{
    Data.CourseworkDatebase courseworkDatebase = new();
    string main;
    string login;
    string password;
    bool b = true;
    Models.Users users;
    public QRPage()
	{
		InitializeComponent();
	}

    private void CameraBarcodeReaderView_BarcodesDetected(object sender, ZXing.Net.Maui.BarcodeDetectionEventArgs e)
    {
        Dispatcher.Dispatch(() =>
        {
            if (b)
            {
                b = false;
                main = $"{e.Results[0].Value}";
                if (main.Contains("login"))
                {
                    string[] parts = main.Split("\r\n");
                    // Поиск и извлечение значения для login
                    foreach (string part in parts)
                    {
                        if (part.StartsWith("login:"))
                        {
                            login = part.Substring(6);
                            break;
                        }
                    }

                    // Поиск и извлечение значения для password
                    foreach (string part in parts)
                    {
                        if (part.StartsWith("password:"))
                        {
                            password = part.Substring(9);
                            break;
                        }
                    }
                    //VSL.Clear();
                    userCreate();
                }
                else
                {
                    b = true;
                }
            }
        });
    }

    public async void userCreate()
    {
        users = await courseworkDatebase.GetUserAsync(login);
        if (users != null)
        {
            if (users.Password == password)
            {
                MauiProgram.loginGlobal = users.Login;
                MauiProgram.roleGlobal = users.Role;
                MauiProgram.idGlobal = users.ID;
                VSL.Clear();
                Application.Current.MainPage = new AppShell();
            }
            else
            {
                barcodeResult.Text = $"Неверный код";
                b = true;
            }
        } else
        {
            barcodeResult.Text = $"Неверный код";
            b = true;
        }
    }
}