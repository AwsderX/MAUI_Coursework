using MAUI_Coursework.Data;
using MAUI_Coursework.Models;

namespace MAUI_Coursework.Views;

public partial class Profile : ContentPage
{
    CourseworkDatebase _courseworkDatebase = new();
    Teachers teachers;
    Students students;
    Label labelGroup;
    ListView listViewLessons;

    Entry entryName;
    Entry entryGroup;
    DateTime dt = DateTime.Now;
    Button buttonAddLesson;

    public Profile()
	{
		InitializeComponent();
        LoadData();
        buttonAddLesson = new();
        buttonAddLesson.Text = "Добавить урок";
        buttonAddLesson.Clicked += ButtonAddLesson_Clicked;
    }

    public async void LoadData()
    {
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
            listViewLessons.ItemsSource = await _courseworkDatebase.GetLessonsAsync(MauiProgram.idGlobal);

            // Создаем шаблон для отображения элементов в ListView
            DataTemplate dataTemplate = new DataTemplate(() =>
            {
                Label labelName = new Label();
                Label labelGroup = new Label();
                Label labelDateTime = new Label();
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
                labelDateTime.SetBinding(Label.TextProperty, "TimeL");

                grid.Children.Add(labelName);
                grid.Children.Add(labelGroup);
                grid.Children.Add(labelDateTime);

                // Установка позиций элементов внутри сетки
                Grid.SetColumn(labelName, 0);
                Grid.SetRow(labelName, 0);
                Grid.SetColumnSpan(labelName, 2);
                Grid.SetColumn(labelDateTime, 0);
                Grid.SetRow(labelDateTime, 1);
                Grid.SetColumn(labelGroup, 1);
                Grid.SetRow(labelGroup, 1);

                return new ViewCell
                {
                    View = grid
                };
            });
            listViewLessons.ItemTemplate = dataTemplate;
            StackL.Add(listViewLessons);

            StackL.Add(buttonAddLesson);

        }
        else if (MauiProgram.roleGlobal == 3)//студент
        {
            students = await _courseworkDatebase.GetStudentAsync(MauiProgram.idGlobal);
            labelName.Text = students.Name;
            labelSurname.Text = students.Surname;
            labelPatronymic.Text = students.Patronymic;
            if (students.Photo != null)
            {
                imagePhoto.Source = ImageSource.FromStream(() => new MemoryStream(students.Photo)); ;
            }

            labelGroup = new()
            {
                Text = $"Учебная группа {students.Group}",
                FontSize = 18,
                Margin = 10
            };
            StackL.Add(labelGroup);
        }
    }

    private void ButtonAddLesson_Clicked(object sender, EventArgs e)
    {
        entryName = new()
        {
            Margin = 5,
            FontSize = 18,
            Placeholder = "Название дисциплины"
        };
        entryGroup = new()
        {
            Margin = 5,
            FontSize = 18,
            Placeholder = "Группа"
        };
        DatePicker datePicker = new()
        {
            Margin = 5,
            FontSize = 18
        };
        TimePicker timePicker = new()
        {
            Margin = 5,
            FontSize = 18,
            Format = "HH:mm"
        };
        datePicker.DateSelected += (sender, e) =>
        {
            dt = e.NewDate;
            // Обработка выбранной даты
        };
        timePicker.PropertyChanged += (sender, e) =>
        {
            if (e.PropertyName == nameof(TimePicker.Time))
            {
                TimeSpan selectedTime = timePicker.Time;
                dt = new DateTime(dt.Year, dt.Month, dt.Day, selectedTime.Hours, selectedTime.Minutes, selectedTime.Seconds);
                // Обработка выбранного времени
            }
        };
        Button button = new()
        {
            Text = "Сохранить",
            FontSize = 14,
            Margin= 5
        };
        button.Clicked += ButtonSaveLesson_Clicked;
        StackL.Add(entryName);
        StackL.Add(entryGroup);
        StackL.Add(datePicker);
        StackL.Add(timePicker);
        StackL.Add(button);
        buttonAddLesson.IsEnabled = false;

    }

    private async void ButtonSaveLesson_Clicked(object sender, EventArgs e)
    {
        Lessons lessons = new Lessons();
        lessons.Name = entryName.Text;
        lessons.Group=entryGroup.Text;
        lessons.TimeL = dt;
        lessons.ID_teacher = MauiProgram.idGlobal;
        await _courseworkDatebase.SaveLessonAsync(lessons);
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
                } else if (MauiProgram.roleGlobal == 3)//студент
                {
                    students.Photo = imageBytes;
                    await _courseworkDatebase.UpdateStudent(students);
                }
            }
        }
    }
}