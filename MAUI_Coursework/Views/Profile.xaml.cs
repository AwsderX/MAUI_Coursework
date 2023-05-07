using MAUI_Coursework.Data;
using MAUI_Coursework.Models;

namespace MAUI_Coursework.Views;

public partial class Profile : ContentPage
{
    CourseworkDatebase _courseworkDatebase = new();
    Teachers teachers;
    Students students;
    Label labelGroupS;
    ListView listViewLessons;

    Entry entryName;
    Entry entryGroup;
    TimeSpan ts = TimeSpan.Zero;
    string weekday;
    Button buttonAddLesson;

    DatePicker datePicker = new();
    TimePicker timePicker = new();
    Button button = new();

    public Profile()
	{
		InitializeComponent();
        LoadData();
    }

    public async void LoadData()
    {
        buttonAddLesson = new();
        buttonAddLesson.Text = "Добавить урок";
        buttonAddLesson.Clicked += ButtonAddLesson_Clicked;
        if (MauiProgram.roleGlobal == 2)
        {
            teachers = await _courseworkDatebase.GetTeacherAsync(MauiProgram.idGlobal);
            labelName.Text = teachers.Name;
            labelSurname.Text = teachers.Surname;
            labelPatronymic.Text = teachers.Patronymic;
            if (teachers.Photo != null)
            {
                imagePhoto.Source = ImageSource.FromStream(() => new MemoryStream(teachers.Photo)); ;
            }
            listViewLessons = new();

            listViewLessons.ItemSelected += async (sender, e) =>
            {
                if (e.SelectedItem != null && e.SelectedItem is Lessons selectedData)
                {
                    foreach (var cell in listViewLessons.TemplatedItems)
                    {
                        var viewCell = cell as ViewCell;
                        if (viewCell.View != null)
                        {
                            viewCell.View.BackgroundColor = (viewCell.BindingContext == selectedData) ? Color.FromArgb("#A82038") : Color.FromArgb("#FFFFFF");
                        }
                    }
                }
                bool result = await DisplayAlert("Внимание", "Вы действительно хотите удалить урок?", "ОК", "Отмена");

                if (result)
                {
                    if (e.SelectedItem is Lessons selectedLesson)
                    {
                        if (await _courseworkDatebase.DeleteLessonAsync(selectedLesson) != 0)
                        {
                            await Task.Delay(100);
                            StackL.Clear();
                        }

                    }
                }
                else
                {
                    // Пользователь выбрал кнопку "Отмена"
                }
            };

            listViewLessons.ItemsSource = await _courseworkDatebase.GetLessonsAsync(MauiProgram.idGlobal);

            // Создаем шаблон для отображения элементов в ListView
            DataTemplate dataTemplate = new DataTemplate(() =>
            {
                Label labelName = new Label();
                Label labelGroup = new Label();
                Label labelWeekday = new Label();
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
                    new ColumnDefinition { Width = GridLength.Star },
                    new ColumnDefinition { Width = 80 },
                }
                };

                labelName.SetBinding(Label.TextProperty, "Name");
                labelGroup.SetBinding(Label.TextProperty, "Group");
                labelWeekday.SetBinding(Label.TextProperty, "Weekday");
                labelTime.SetBinding(Label.TextProperty, "TimeL");

                grid.Children.Add(labelName);
                grid.Children.Add(labelGroup);
                grid.Children.Add(labelWeekday);
                grid.Children.Add(labelTime);

                // Установка позиций элементов внутри сетки
                Grid.SetColumn(labelName, 0);
                Grid.SetRow(labelName, 0);
                Grid.SetColumnSpan(labelName, 3);
                Grid.SetColumn(labelWeekday, 0);
                Grid.SetRow(labelWeekday, 1);
                Grid.SetColumn(labelGroup, 2);
                Grid.SetRow(labelGroup, 1);
                Grid.SetColumn(labelTime, 1);
                Grid.SetRow(labelTime, 1);

                return new ViewCell
                {
                    View = grid
                };
            });
            listViewLessons.ItemTemplate = dataTemplate;
            StackL.Add(listViewLessons);

            StackL.Add(buttonAddLesson);

        }
        else if (MauiProgram.roleGlobal == 3) //студент
        {
            students = await _courseworkDatebase.GetStudentAsync(MauiProgram.idGlobal);
            labelName.Text = students.Name;
            labelSurname.Text = students.Surname;
            labelPatronymic.Text = students.Patronymic;
            if (students.Photo != null)
            {
                imagePhoto.Source = ImageSource.FromStream(() => new MemoryStream(students.Photo)); ;
            }
            if (labelGroupS == null)
            {
                labelGroupS = new()
                {
                    Text = $"Учебная группа {students.Group}",
                    FontSize = 18,
                    Margin = 10
                };
                StackL.Add(labelGroupS);
            }
        }
    }

    private async void ButtonAddLesson_Clicked(object sender, EventArgs e)
    {
        if (buttonAddLesson.Text == "Добавить урок")
        {
            entryName = new()
            {
                Margin = 5,
                FontSize = 18,
                Placeholder = "Название дисциплины",
                Text = ""
            };
            entryGroup = new()
            {
                Margin = 5,
                FontSize = 18,
                Placeholder = "Группа",
                Text = ""
            };
            datePicker = new()
            {
                Margin = 5,
                FontSize = 18
            };
            timePicker = new()
            {
                Margin = 5,
                FontSize = 18,
                Format = "HH:mm"
            };
            datePicker.DateSelected += (sender, e) =>
            {
                DateTime date = e.NewDate;
                weekday = date.DayOfWeek.ToString();
                // Обработка выбранной даты
            };
            timePicker.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == nameof(TimePicker.Time))
                {
                    ts = timePicker.Time;
                    while (ts.Minutes % 5 != 0)
                    {
                        ts = ts.Add(TimeSpan.FromMinutes(1));
                    }
                    timePicker.Time = ts;
                    // Обработка выбранного времени
                }
            };
            button = new()
            {
                Text = "Сохранить",
                FontSize = 14,
                Margin = 5
            };
            button.Clicked += ButtonSaveLesson_Clicked;
            buttonAddLesson.IsEnabled = false;
            #pragma warning disable 4014
            List<View> views = new() { entryName, entryGroup, datePicker, timePicker, button };
            foreach (var view in views)
            {
                view.Opacity = 0;
                view.Scale = 0.5;
                StackL.Add(view);
                await Task.Delay(100); // Добавьте небольшую задержку для полного отображения элементов
                await ScrollV.ScrollToAsync(view, ScrollToPosition.End, true);
                view.FadeTo(1, 150, Easing.Linear);
                view.ScaleTo(1, 150, Easing.SpringOut);
            }
            #pragma warning restore 4014
            buttonAddLesson.IsEnabled = true;

            buttonAddLesson.Text = "Скрыть";
        } else
        {
            buttonAddLesson.Text = "Добавить урок";

            buttonAddLesson.IsEnabled = false;
            #pragma warning disable 4014
            List<View> views = new() { button, timePicker, datePicker, entryGroup, entryName };
            foreach (var view in views)
            {
                view.Opacity = 1;
                view.Scale = 1;
                view.FadeTo(0, 150, Easing.Linear);
                view.ScaleTo(0.8, 150, Easing.SpringOut);
                await ScrollV.ScrollToAsync(0, ScrollV.ScrollY - 52, true);
                await Task.Delay(150);
                StackL.Children.Remove(view);
            }
            #pragma warning restore 4014
            buttonAddLesson.IsEnabled = true;
    
        }

    }

    private async void ButtonSaveLesson_Clicked(object sender, EventArgs e)
    {
        if (entryName.Text != "" && entryGroup.Text != "")
        {
            Lessons lessons = new Lessons();
            lessons.Name = entryName.Text;
            lessons.Group = entryGroup.Text;
            lessons.Weekday = weekday;
            lessons.TimeL = ts;
            lessons.ID_teacher = MauiProgram.idGlobal;
            if (await _courseworkDatebase.SaveLessonAsync(lessons) != 0)
            {
                entryName.Text = string.Empty;
                entryGroup.Text = string.Empty;
            }
        }
        else await DisplayAlert("Ошибка","Не все поля заполнены","Ок");
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        var result = await FilePicker.PickAsync(new PickOptions
        {
            FileTypes = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
            {
                { DevicePlatform.iOS, new[] { "public.image" } },
                { DevicePlatform.Android, new[] { "image/*" } },
                { DevicePlatform.WinUI, new[] { ".jpg", ".jpeg", ".png", ".bmp" } },
            }),
            PickerTitle = "Select an image",
        });
        if (result != null)
        {
            // Загрузка выбранного изображения
            var stream = await result.OpenReadAsync();
            using (var ms = new MemoryStream())
            {
                await stream.CopyToAsync(ms);
                byte[] imageBytes = ms.ToArray();
                //var imageSource = ImageSource.FromStream(() => new MemoryStream(imageBytes));
                if (MauiProgram.roleGlobal == 2)
                {
                    teachers.Photo = imageBytes;
                    await _courseworkDatebase.UpdateTeacher(teachers);
                } else if (MauiProgram.roleGlobal == 3) //студент
                {
                    students.Photo = imageBytes;
                    await _courseworkDatebase.UpdateStudent(students);
                }
            }
        }
        LoadData();
    }

    private void ScrollV_Scrolled(object sender, ScrolledEventArgs e)
    {
        if (e.ScrollY == 0)
        {
            StackL.Clear();
            LoadData();
        }
    }
}