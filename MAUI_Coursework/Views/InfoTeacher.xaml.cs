using MAUI_Coursework.Data;
using MAUI_Coursework.Models;
using System.ComponentModel;

namespace MAUI_Coursework.Views;
public class ViewModelTeacher : INotifyPropertyChanged
{
    //bool red = true;
    CourseworkDatebase _courseworkDatebase;
    private List<TeachersImage> _myDataList;
    private List<Teachers> _myDataListDB;
    public List<Teachers> MyDataListDB
    {
        get => _myDataListDB;
        set
        {
            _myDataListDB = value;
            OnPropertyChanged(nameof(MyDataListDB));
        }
    }
    public List<TeachersImage> MyDataList
    {
        get => _myDataList;
        set
        {
            _myDataList = value;
            OnPropertyChanged(nameof(MyDataList));
        }
    }

    public ViewModelTeacher()
    {
        _courseworkDatebase = new CourseworkDatebase();
        // _courseworkDatebase.PropertyChanged += OnCourseworkDatabasePropertyChanged;
    }
    private async void OnCourseworkDatabasePropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(Teachers))
        {
            // Обновляем список при изменении данных
            await DataSetter();
        }
    }
    public async Task DataSetter()
    {
        MyDataListDB = await _courseworkDatebase.GetTeachersAsync();
        RealList();
    }
    public event PropertyChangedEventHandler PropertyChanged;

    public virtual void OnPropertyChanged(string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    public void RealList()
    {
        MyDataList = new List<TeachersImage>();
        foreach (var item in MyDataListDB)
        {
            TeachersImage teachersImage = new();
            teachersImage.Name = item.Name;
            teachersImage.Surname = item.Surname;
            teachersImage.Patronymic = item.Patronymic;
            teachersImage.ID_user = item.ID_user;
            if (item.Photo != null) { teachersImage.Photo = ImageSource.FromStream(() => new MemoryStream(item.Photo)); } else { teachersImage.Photo = "not_photo.jpg"; }
            MyDataList.Add(teachersImage);
        }
    }
}
[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class InfoTeacher : ContentPage
{
	public InfoTeacher()
	{
		InitializeComponent();

        TListView.ItemTapped += (sender, e) =>
        {
            if (e.Item != null)
            {
                TListView.SelectedItem = null; // Сбрасываем выбор элемента
            }
        };

        TListView.ItemSelected += (sender, e) =>
        {
            if (e.SelectedItem != null && e.SelectedItem is TeachersImage selectedData)
            {
                // Здесь можно применить стиль или изменить фон элемента на основе выбранного элемента
                // Пример:
                foreach (var cell in TListView.TemplatedItems)
                {
                    var viewCell = cell as ViewCell;
                    if (viewCell.View != null)
                    {
                        viewCell.View.BackgroundColor = (viewCell.BindingContext == selectedData) ? Color.FromArgb("#FF1F40") : Color.FromArgb("#FFFFFF");
                    }
                }
            }
        };
        LoadDataAsync();
    }
    ViewModelTeacher viewModel = new();
    private async void LoadDataAsync()
    {
        await viewModel.DataSetter();
        BindingContext = viewModel;
    }

    private void OnItemTapped(object sender, ItemTappedEventArgs e)
    {
        if (e.Item is TeachersImage teacher)
        {

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
}