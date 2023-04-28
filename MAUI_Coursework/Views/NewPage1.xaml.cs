using MAUI_Coursework.Data;
using MAUI_Coursework.Models;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace MAUI_Coursework.Views;

public class MyViewModel
{
    private readonly CourseworkDatebase _courseworkDatebase;
    private List<Users> _myDataList;
    public List<Users> MyDataList
    {
        get => _myDataList;
        set
        {
            _myDataList = value;
            OnPropertyChanged();
        }
    }

    public MyViewModel()
    {
        _courseworkDatebase = new CourseworkDatebase();
    }
    public async Task DataSetter()
    {
        MyDataList = await _courseworkDatebase.GetUsersAsync();
    }
    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class NewPage1 : ContentPage
{
    public NewPage1()
    {
        InitializeComponent();
        LoadDataAsync();
    }
    private async void LoadDataAsync()
    {
        var viewModel = new MyViewModel();
        await viewModel.DataSetter();
        BindingContext = viewModel;
    }
    private void OnItemTapped(object sender, ItemTappedEventArgs e)
    {
        if (e.Item is Users user)
        {
            DisplayAlert("Информация о пользователе",
                         $"Логин: {user.Login}\nПароль: {user.Password}\nРоль: {user.Role}",
                         "Ок");
        }
    }
}


#region Work
//public class MyViewModel
//{
//    private readonly CourseworkDatebase _courseworkDatebase;
//    private List<Users> _myDataList;
//    public List<Users> MyDataList
//    {
//        get => _myDataList;
//        set
//        {
//            _myDataList = value;
//            OnPropertyChanged();
//        }
//    }

//    public MyViewModel()
//    {
//        _courseworkDatebase = new CourseworkDatebase();
//    }
//    public async Task DataSetter()
//    {
//        MyDataList = await _courseworkDatebase.GetUsersAsync();
//    }
//    public event PropertyChangedEventHandler PropertyChanged;

//    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
//    {
//        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
//    }
//}
//[XamlCompilation(XamlCompilationOptions.Compile)]
//public partial class NewPage1 : ContentPage
//{
//    public NewPage1()
//    {
//        InitializeComponent();
//        LoadDataAsync();
//    }
//    private async void LoadDataAsync()
//    {
//        var viewModel = new MyViewModel();
//        await viewModel.DataSetter();
//        BindingContext = viewModel;
//    }
//    private void OnItemTapped(object sender, ItemTappedEventArgs e)
//    {
//        if (e.Item is Users user)
//        {
//            DisplayAlert("Информация о пользователе",
//                         $"Логин: {user.Login}\nПароль: {user.Password}\nРоль: {user.role}",
//                         "Ок");
//        }
//    }

//}
#endregion

#region NotWorkWhy
//public class MyViewModel
//{
//    CourseworkDatebase courseworkDatebase = new CourseworkDatebase();
//    public List<Users> MyDataList { get; set; }

//    public MyViewModel()
//    {
//        Task.Run(() => DataSetter());
//    }
//    public async Task DataSetter()
//    {
//        MyDataList = await courseworkDatebase.GetUsersAsync();
//    }
//}
//[XamlCompilation(XamlCompilationOptions.Compile)]
//public partial class NewPage1 : ContentPage
//{
//    public NewPage1()
//    {
//        InitializeComponent();
//        BindingContext = new MyViewModel();
//    }
//    private void OnItemTapped(object sender, ItemTappedEventArgs e)
//    {
//        if (e.Item != null)
//        {
//            DisplayAlert("Ух ты", "Вы нажали на элемент: " + e.Item.ToString(), "Окей");
//        }
//    }
//}
#endregion