using MAUI_Coursework.Data;
using MAUI_Coursework.Models;
using System.ComponentModel;

namespace MAUI_Coursework.Views;

public partial class GradesView : ContentPage, INotifyPropertyChanged
{
	string group;
    string mode="Н";
	int Id_lesson;
    DateTime dt;
    ListView listView = new();
    CourseworkDatebase _courseworkDatebase = new();
#pragma warning disable 0114
#pragma warning disable 0108
    // Реализация интерфейса INotifyPropertyChanged
    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
#pragma warning restore 0114
#pragma warning restore 0108
    public List<Students> students = new();
	public GradesView(string group, int Id_lesson, DateTime dt)
	{
		InitializeComponent();
        this.dt = dt;
		this.group = group;
        this.Id_lesson = Id_lesson;

        LoadData();
    }

    List<GradesStud> gradesStuds;
    private void UpdateGradesList(List<GradesStud> updatedList)
    {
        gradesStuds = updatedList;
        OnPropertyChanged(nameof(gradesStuds));
    }
    public async void LoadData()
	{
        //students.Clear();
        //List<string> students = await _courseworkDatebase.GetStudentsAsync(group);

        gradesStuds = await _courseworkDatebase.GetGradesListAsync(group,dt);
        UpdateGradesList(gradesStuds);

        listView.ItemsSource = gradesStuds;
        // Создаем шаблон для отображения элементов в ListView
        DataTemplate dataTemplate = new DataTemplate(() =>
        {
            Label labelName = new();
            labelName.FontSize = 16;
            Label labelSurname = new();
            labelSurname.FontSize = 16;
            Label labelPatronymic = new();
            labelPatronymic.FontSize = 16;
            labelPatronymic.HorizontalOptions = LayoutOptions.Center;
            Label labelEst = new();
            labelEst.FontSize = 30;
            Label labelAtt = new();
            labelAtt.FontSize = 30;
            Grid grid = new Grid
            {
                Margin = new Thickness(1),
                RowDefinitions =
                {
                    new RowDefinition { Height = GridLength.Star },
                    new RowDefinition { Height = GridLength.Star },
                },
                ColumnDefinitions =
                {
                    new ColumnDefinition { Width = new GridLength(2, GridUnitType.Star) },
                    new ColumnDefinition { Width = new GridLength(2, GridUnitType.Star) },
                    new ColumnDefinition { Width = GridLength.Star },
                    new ColumnDefinition { Width = GridLength.Star },
                }
            };

            labelName.SetBinding(Label.TextProperty, "Name");
            labelSurname.SetBinding(Label.TextProperty, "Surname");
            labelPatronymic.SetBinding(Label.TextProperty, "Patronymic");
            labelEst.SetBinding(Label.TextProperty, "Est");
            labelAtt.SetBinding(Label.TextProperty, "Att");

            grid.Children.Add(labelName);
            grid.Children.Add(labelSurname);
            grid.Children.Add(labelPatronymic);
            grid.Children.Add(labelEst);
            grid.Children.Add(labelAtt);

            // Установка позиций элементов внутри сетки
            Grid.SetColumn(labelName, 0);
            Grid.SetRow(labelName, 0);
            Grid.SetColumn(labelSurname, 1);
            Grid.SetRow(labelSurname, 0);
            Grid.SetColumn(labelPatronymic, 0);
            Grid.SetRow(labelPatronymic, 1);
            Grid.SetColumnSpan(labelPatronymic, 2);

            Grid.SetColumn(labelEst, 2);
            Grid.SetRow(labelEst, 0);
            Grid.SetRowSpan(labelEst, 2);

            Grid.SetColumn(labelAtt, 3);
            Grid.SetRow(labelAtt, 0);
            Grid.SetRowSpan(labelAtt, 2);


            return new ViewCell
            {
                View = grid
            };
        });

        listView.ItemTemplate = dataTemplate;

        listView.ItemSelected += async (sender, e) =>
        {
            if (e.SelectedItem != null && e.SelectedItem is GradesStud selectedData)
            {
                foreach (var cell in listView.TemplatedItems)
                {
                    var viewCell = cell as ViewCell;
                    if (viewCell.View != null)
                    {
                        viewCell.View.BackgroundColor = (viewCell.BindingContext == selectedData) ? Color.FromArgb("#FA78E6") : Color.FromArgb("#FFFFFF");
                    }
                }
                
                if (mode=="1" || mode == "2" || mode == "3" || mode == "4" || mode == "5")
                {
                    if (selectedData.Est == null && selectedData.Att == null)
                    {
                        selectedData.Est = mode;
                        Grades grades = new()
                        {
                            ID_lesson = Id_lesson,
                            ID_student = selectedData.ID_user,
                            Date_lesson = dt,
                            Est = selectedData.Est,
                            Att = null
                        };
                        await _courseworkDatebase.SaveGradesAsync(grades);
                    }
                    else
                    {
                        selectedData.Est = mode;
                        await _courseworkDatebase.UpdateGradesEstAsync(selectedData.ID, selectedData.Est);
                    }
                } else
                {
                    if (selectedData.Est == null && selectedData.Att == null)
                    {
                        selectedData.Att = mode;
                        Grades grades = new()
                        {
                            ID_lesson = Id_lesson,
                            ID_student = selectedData.ID_user,
                            Date_lesson = dt,
                            Att = selectedData.Att,
                            Est = null
                        };
                        await _courseworkDatebase.SaveGradesAsync(grades);
                    }
                    else
                    {
                        selectedData.Att = mode;
                        await _courseworkDatebase.UpdateGradesAttAsync(selectedData.ID, selectedData.Att);
                    }
                }
                gradesStuds = await _courseworkDatebase.GetGradesListAsync(group, dt);
                UpdateGradesList(gradesStuds);

                listView.ItemsSource = gradesStuds;
            }
        };

        StackL.Add(listView);
    }


    private void Button_Clicked(object sender, EventArgs e)
    {
        if (sender is Button bttn)
        {
            switch (bttn.Text)
            {
                case "1": { mode = "1"; break; }
                case "2": { mode = "2"; break; }
                case "3": { mode = "3"; break; }
                case "4": { mode = "4"; break; }
                case "5": { mode = "5"; break; }
                case "Н": { mode = "Н"; break; }
                case "О": { mode = "О"; break; }
                case "Б": { mode = "Б"; break; }
                case "+": { mode = "+"; break; }
                default:
                    break;
            }
            AllBttnsBack();
            bttn.BackgroundColor = Color.FromArgb("#086AAF"); //03933D #086AAF
        }
    }
    private void AllBttnsBack()
    {
        Button1.BackgroundColor = Color.FromArgb("#9b0965");
        Button2.BackgroundColor = Color.FromArgb("#9b0965");
        Button3.BackgroundColor = Color.FromArgb("#9b0965");
        Button4.BackgroundColor = Color.FromArgb("#9b0965");
        Button5.BackgroundColor = Color.FromArgb("#9b0965");
        ButtonN.BackgroundColor = Color.FromArgb("#9b0965");
        ButtonO.BackgroundColor = Color.FromArgb("#9b0965");
        ButtonB.BackgroundColor = Color.FromArgb("#9b0965");
        ButtonPlus.BackgroundColor = Color.FromArgb("#9b0965");
    }
}