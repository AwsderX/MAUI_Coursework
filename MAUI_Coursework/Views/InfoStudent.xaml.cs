using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using MAUI_Coursework.Data;
using MAUI_Coursework.Models;
using Microsoft.Maui.Controls;

namespace MAUI_Coursework.Views;

#region Work1
//public class ViewModelStudent
//{
//    CourseworkDatebase _courseworkDatebase;
//    private List<StudentsImage> _myDataList;
//    private List<Students> _myDataListDB;
//    public List<Students> MyDataListDB
//    {
//        get => _myDataListDB;
//        set
//        {
//            _myDataListDB = value;
//            //OnPropertyChanged();
//        }
//    }
//    public List<StudentsImage> MyDataList
//    {
//        get => _myDataList;
//        set
//        {
//            _myDataList = value;
//            //OnPropertyChanged();
//        }
//    }

//    public ViewModelStudent()
//    {
//        _courseworkDatebase = new CourseworkDatebase();
//        //_courseworkDatebase.PropertyChanged += OnCourseworkDatabasePropertyChanged;
//    }
//    private async void OnCourseworkDatabasePropertyChanged(object sender, PropertyChangedEventArgs e)
//    {
//        if (e.PropertyName == nameof(Students))
//        {
//            // Обновляем список при изменении данных
//            await DataSetter();
//        }
//    }
//    public async Task DataSetter()
//    {
//        MyDataListDB = await _courseworkDatebase.GetStudentsAsync();
//        RealList();
//    }
//    //public event PropertyChangedEventHandler PropertyChanged;

//    //protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
//    //{
//    //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
//    //    //DataSetter();
//    //    //RealList();
//    //}
//    public void RealList()
//    {
//        MyDataList = new List<StudentsImage>();
//        foreach (var item in MyDataListDB)
//        {
//            StudentsImage studentsImage = new();
//            studentsImage.Name = item.Name; 
//            studentsImage.Surname = item.Surname;
//            studentsImage.Patronymic = item.Patronymic;
//            studentsImage.Group = item.Group;
//            studentsImage.ID_user = item.ID_user;
//            if (item.Photo != null) { studentsImage.Photo = ImageSource.FromStream(() => new MemoryStream(item.Photo)); } else { studentsImage.Photo = ""; }
//            MyDataList.Add(studentsImage);
//        }
//    }
//}
//[XamlCompilation(XamlCompilationOptions.Compile)]
//public partial class InfoStudent : ContentPage
//{

//    public InfoStudent()
//    {
//        InitializeComponent();

//        LoadDataAsync();
//    }
//    private async void LoadDataAsync()
//    {
//        ViewModelStudent viewModel = new();
//        await viewModel.DataSetter();
//        BindingContext = viewModel;

//    }

//    private void OnItemTapped(object sender, ItemTappedEventArgs e)
//    {
//        if (e.Item is StudentsImage student)
//        {
//            DisplayAlert("Информация о пользователе",
//                         $"Логин: {student.Name}\nПароль: {student.Surname}\nРоль: {student.Patronymic}",
//                         "Ок");
//        }
//    }
//}
#endregion

//public partial class InfoStudent : ContentPage
//{
//    CourseworkDatebase _courseworkDatebase = new();
//    private ObservableCollection<StudentsImage> MyDataList { get; set; } = new();
//    private ObservableCollection<Students> MyDataListDB { get; set; } = new();
//    public InfoStudent()
//    {
//        InitializeComponent();

//        //_courseworkDatebase = courseworkDatebase;
//        BindingContext = this;
//        UpDt();

//    }
//    private void LoadDataAsync(CourseworkDatebase courseworkDatebase)
//    {
//        //ViewModelStudent viewModel = new();
//        //await viewModel.DataSetter();
//        //_courseworkDatebase = courseworkDatebase;
//        //BindingContext = this;

//    }
//    public async void UpDt()
//    {
//        var items = await _courseworkDatebase.GetStudentsAsync();
//        //MainThread.BeginInvokeOnMainThread(() =>
//        //{
//        MyDataListDB.Clear();
//        foreach (var item in items)
//            MyDataListDB.Add(item);
//        MyDataList.Clear();
//        foreach (var item in MyDataListDB)
//        {
//            StudentsImage studentsImage = new();
//            studentsImage.Name = item.Name;
//            studentsImage.Surname = item.Surname;
//            studentsImage.Patronymic = item.Patronymic;
//            studentsImage.Group = item.Group;
//            studentsImage.ID_user = item.ID_user;
//            if (item.Photo != null) { studentsImage.Photo = ImageSource.FromStream(() => new MemoryStream(item.Photo)); } else { studentsImage.Photo = ""; }
//            MyDataList.Add(studentsImage);
//        }
//        // });
//    }

//    private void OnItemTapped(object sender, ItemTappedEventArgs e)
//    {
//        if (e.Item is StudentsImage student)
//        {
//            DisplayAlert("Информация о пользователе",
//                         $"Логин: {student.Name}\nПароль: {student.Surname}\nРоль: {student.Patronymic}",
//                         "Ок");
//        }
//    }
//    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
//    {
//        base.OnNavigatedTo(args);
//        var items = await _courseworkDatebase.GetStudentsAsync();
//        MainThread.BeginInvokeOnMainThread(() =>
//        {
//            MyDataListDB.Clear();
//            foreach (var item in items)
//                MyDataListDB.Add(item);
//            MyDataList.Clear();
//            foreach (var item in MyDataListDB)
//            {
//                StudentsImage studentsImage = new();
//                studentsImage.Name = item.Name;
//                studentsImage.Surname = item.Surname;
//                studentsImage.Patronymic = item.Patronymic;
//                studentsImage.Group = item.Group;
//                studentsImage.ID_user = item.ID_user;
//                if (item.Photo != null) { studentsImage.Photo = ImageSource.FromStream(() => new MemoryStream(item.Photo)); } else { studentsImage.Photo = ""; }
//                MyDataList.Add(studentsImage);
//            }
//        });
//    }
//    public void RealList()
//    {
//        MyDataList.Clear();
//        foreach (var item in MyDataListDB)
//        {
//            StudentsImage studentsImage = new();
//            studentsImage.Name = item.Name;
//            studentsImage.Surname = item.Surname;
//            studentsImage.Patronymic = item.Patronymic;
//            studentsImage.Group = item.Group;
//            studentsImage.ID_user = item.ID_user;
//            if (item.Photo != null) { studentsImage.Photo = ImageSource.FromStream(() => new MemoryStream(item.Photo)); } else { studentsImage.Photo = ""; }
//            MyDataList.Add(studentsImage);
//        }
//    }
//}


public class ViewModelStudent : INotifyPropertyChanged
{
    //bool red = true;
    CourseworkDatebase _courseworkDatebase;
    private List<StudentsImage> _myDataList;
    private List<Students> _myDataListDB;
    public List<Students> MyDataListDB
    {
        get => _myDataListDB;
        set
        {
            _myDataListDB = value;
            OnPropertyChanged(nameof(MyDataListDB));
        }
    }
    public List<StudentsImage> MyDataList
    {
        get => _myDataList;
        set
        {
            _myDataList = value;
            OnPropertyChanged(nameof(MyDataList));
        }
    }

    public ViewModelStudent()
    {
        _courseworkDatebase = new CourseworkDatebase();
       // _courseworkDatebase.PropertyChanged += OnCourseworkDatabasePropertyChanged;
    }
    private async void OnCourseworkDatabasePropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(Students))
        {
            // Обновляем список при изменении данных
            await DataSetter();
        }
    }
    public async Task DataSetter()
    {
        MyDataListDB = await _courseworkDatebase.GetStudentsAsync();
        RealList();
    }
    public event PropertyChangedEventHandler PropertyChanged;

    public virtual void OnPropertyChanged(string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    //protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    //{
    //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    //    //DataSetter();
    //    //RealList();
    //}
    public void RealList()
    {
        MyDataList = new List<StudentsImage>();
        foreach (var item in MyDataListDB)
        {
            StudentsImage studentsImage = new();
            studentsImage.Name = item.Name;
            studentsImage.Surname = item.Surname;
            studentsImage.Patronymic = item.Patronymic;
            studentsImage.Group = item.Group;
            studentsImage.ID_user = item.ID_user;
            if (item.Photo != null) { studentsImage.Photo = ImageSource.FromStream(() => new MemoryStream(item.Photo)); } else { studentsImage.Photo = ""; }
            MyDataList.Add(studentsImage);
        }
    }
}
[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class InfoStudent : ContentPage
{

    public InfoStudent()
    {
        InitializeComponent();
       
        LoadDataAsync();
    }
    ViewModelStudent viewModel = new();
    private async void LoadDataAsync()
    {
        await viewModel.DataSetter();
        BindingContext = viewModel;

    }
    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        await viewModel.DataSetter();
    }
    private void OnItemTapped(object sender, ItemTappedEventArgs e)
    {
        if (e.Item is StudentsImage student)
        {

            //DisplayAlert("Информация о пользователе",
            //             $"Логин: {student.Name}\nПароль: {student.Surname}\nРоль: {student.Patronymic}",
            //             "Ок");
        }
    }

    private void ScrollView_Scrolled(object sender, ScrolledEventArgs e)
    {
        if (e.ScrollY == 0)
        {
            viewModel = new();
            LoadDataAsync();
            viewModel.OnPropertyChanged(nameof(viewModel.MyDataList));
        }
    }

    private void ScrollView_ScrollToRequested(object sender, ScrollToRequestedEventArgs e)
    {
        if (e.Mode == ScrollToMode.Position && e.ScrollY == 0)
        {
            viewModel = new();
            LoadDataAsync();
            viewModel.OnPropertyChanged(nameof(viewModel.MyDataList));
        }
    }
}