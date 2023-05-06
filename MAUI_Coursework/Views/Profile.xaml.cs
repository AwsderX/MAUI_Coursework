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
    TimeSpan ts = TimeSpan.Zero;
    string weekday;
    Button buttonAddLesson;

    public Profile()
	{
		InitializeComponent();
        LoadData();
        buttonAddLesson = new();
        buttonAddLesson.Text = "�������� ����";
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

            // ������� ������ ��� ����������� ��������� � ListView
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

                // ��������� ������� ��������� ������ �����
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
        else if (MauiProgram.roleGlobal == 3)//�������
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
                Text = $"������� ������ {students.Group}",
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
            Placeholder = "�������� ����������"
        };
        entryGroup = new()
        {
            Margin = 5,
            FontSize = 18,
            Placeholder = "������"
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
            DateTime date = e.NewDate;
            weekday = date.DayOfWeek.ToString();
            // ��������� ��������� ����
        };
        timePicker.PropertyChanged += (sender, e) =>
        {
            if (e.PropertyName == nameof(TimePicker.Time))
            {
                ts = timePicker.Time;              
                // ��������� ���������� �������
            }
        };
        Button button = new()
        {
            Text = "���������",
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
        lessons.Group = entryGroup.Text;
        lessons.Weekday = weekday;
        lessons.TimeL = ts;
        lessons.ID_teacher = MauiProgram.idGlobal;
        if (await _courseworkDatebase.SaveLessonAsync(lessons) != 0)
        {
            //������� �� � �������� �� �����������
        }
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
            // �������� ���������� �����������
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
                } else if (MauiProgram.roleGlobal == 3)//�������
                {
                    students.Photo = imageBytes;
                    await _courseworkDatebase.UpdateStudent(students);
                }
            }
        }
    }
}