using MAUI_Coursework.Data;
using MAUI_Coursework.Models;
namespace MAUI_Coursework.Views;

public partial class Schedule : ContentPage
{
    CourseworkDatebase _courseworkDatebase = new();
    public Schedule()
	{
		InitializeComponent(); 
        //LoadData();

    }
    protected override void OnAppearing()
    {
        base.OnAppearing();

        StackL.Clear();
        LoadData();
    }
    public async void LoadData()
    {       
        for (int i = 0; i < 7; i++)
        {
            Grid grid = new Grid
            {              
                RowDefinitions =
                {
                    new RowDefinition { Height = GridLength.Auto },
                    new RowDefinition { Height = GridLength.Auto },
                },
                ColumnDefinitions =
                {
                    new ColumnDefinition { Width = GridLength.Star },
                }
            };

            Label labelA = new Label()
            {
                FontSize = 20,
                HorizontalOptions = LayoutOptions.Center,
                TextDecorations = TextDecorations.Underline
            };
            string str = "";
            switch (i)
            {
                case 0: { labelA.Text = "Понедельник"; str = "Monday"; break; }
                case 1: { labelA.Text = "Вторник"; str = "Tuesday"; break; }
                case 2: { labelA.Text = "Среда"; str = "Wednesday"; break; }
                case 3: { labelA.Text = "Четверг"; str = "Thursday"; break; }
                case 4: { labelA.Text = "Пятница"; str = "Friday"; break; }
                case 5: { labelA.Text = "Суббота"; str = "Saturday"; break; }
                case 6: { labelA.Text = "Воскресенье"; str = "Sunday"; break; }
                default:
                    break;
            }
            Grid.SetColumn(labelA, 0);
            Grid.SetRow(labelA, 0);
            grid.Children.Add(labelA);


            ListView listView = new();
            if (MauiProgram.idGlobal == 2)
            {
                listView.ItemsSource = await _courseworkDatebase.GetLessonsScheduleAsync(MauiProgram.idGlobal, str);
            } else if (MauiProgram.idGlobal == 3)
            {
                string StGroup = await _courseworkDatebase.GetStudentGroupAsync(MauiProgram.idGlobal);
                listView.ItemsSource = await _courseworkDatebase.GetLessonsScheduleAsync(StGroup, str);
            }
            // Создаем шаблон для отображения элементов в ListView
            DataTemplate dataTemplate = new DataTemplate(() =>
            {
                Label labelName = new()
                {
                    HorizontalOptions = LayoutOptions.Start,
                    FontSize = 16,
                };
                Label labelGroup = new Label();
                Label labelTime = new Label();
                Grid grid = new Grid
                {
                    RowDefinitions =
                {
                    new RowDefinition { Height = GridLength.Auto },
                    new RowDefinition { Height = GridLength.Auto },
                },
                    ColumnDefinitions =
                {
                    new ColumnDefinition { Width = GridLength.Star },
                    new ColumnDefinition { Width = 80 },
                }
                };

                labelName.SetBinding(Label.TextProperty, "Name");
                labelGroup.SetBinding(Label.TextProperty, "Group");
                labelTime.SetBinding(Label.TextProperty, "TimeL");

                grid.Children.Add(labelName);
                grid.Children.Add(labelGroup);
                grid.Children.Add(labelTime);

                // Установка позиций элементов внутри сетки
                Grid.SetColumn(labelName, 0);
                Grid.SetRow(labelName, 0);
                Grid.SetColumnSpan(labelName, 2);
                Grid.SetColumn(labelGroup, 1);
                Grid.SetRow(labelGroup, 1);
                Grid.SetColumn(labelTime, 0);
                Grid.SetRow(labelTime, 1);

                return new ViewCell
                {
                    View = grid
                };
            });
            listView.ItemTemplate = dataTemplate;
            listView.ItemTapped += ListView_ItemTapped;

            Grid.SetColumn(listView, 0);
            Grid.SetRow(listView, 1);
            grid.Children.Add(listView);

            StackL.Add(grid);
        }
       
    }

    private async void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
    {
        GradesView gradesView = new("100", 1);
        await Navigation.PushAsync(gradesView);
        if (sender is Lessons x) 
        {
            await DisplayAlert("1","1","1");
            //GradesView gradesView = new(x.Group, x.ID);
           //await Navigation.PushAsync(gradesView);  
        }
    }

    private void ScrollV_Scrolled(object sender, ScrolledEventArgs e)
    {
        if (e.ScrollY == 0)
        {

        }
    }
}