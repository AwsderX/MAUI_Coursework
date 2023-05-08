using MAUI_Coursework.Data;
using MAUI_Coursework.Models;
using System.ComponentModel;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace MAUI_Coursework.Views;

public partial class UserInfo : ContentPage
{
    int ID_user;
    int Role_user;
    ListView listViewLessons;
    CourseworkDatebase _courseworkDatebase = new();

    Users users;
    Teachers teachers;
    Students students;
    public UserInfo(int id_user, int role_user)
	{
		InitializeComponent();
        this.ID_user = id_user;
        this.Role_user = role_user;
        LoadData();

    }
	
	public async void LoadData()
	{
        if (MauiProgram.roleGlobal != 1)
        {
            StackL.Remove(BttnSave);
        }

        users = await _courseworkDatebase.GetUserAsync(ID_user);
        LoginEntry.Text = users.Login;
        if (Role_user == 2)
        {
            teachers = await _courseworkDatebase.GetTeacherAsync(ID_user); 
            NameEntry.Text = teachers.Name; 
            SunameEntry.Text = teachers.Surname;
            PatronymicEntry.Text = teachers.Patronymic;
            StackL.Remove(GroupEntry);


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
                if (MauiProgram.roleGlobal == 1)
                {
                    bool result = await DisplayAlert("Внимание", "Вы действительно хотите удалить урок?", "ОК", "Отмена");

                    if (result)
                    {
                        if (e.SelectedItem is Lessons selectedLesson)
                        {
                            if (await _courseworkDatebase.DeleteLessonAsync(selectedLesson) != 0)
                            {
                                await Task.Delay(100);
                                await RefreshListView();
                            }

                        }
                    }
                    else
                    {
                        // Пользователь выбрал кнопку "Отмена"
                    }
                }
            };

            listViewLessons.ItemsSource = await _courseworkDatebase.GetLessonsAsync(ID_user);

            // Создаем шаблон для отображения элементов в ListView
            DataTemplate dataTemplate = new DataTemplate(() =>
            {
                Label labelName = new Label();
                labelName.FontSize = 18;
                Label labelGroup = new Label();
                labelGroup.FontSize = 18;
                Label labelWeekday = new Label();
                labelWeekday.FontSize = 18;
                Label labelTime = new Label();
                labelTime.FontSize = 18;
                Grid grid = new Grid
                {
                    Margin = 1,
                    RowDefinitions =
                    {
                    new RowDefinition { Height = GridLength.Star },
                    new RowDefinition { Height = GridLength.Star },
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
        } else if (Role_user == 3)
        {
            students = await _courseworkDatebase.GetStudentAsync(ID_user);
            NameEntry.Text = students.Name;
            SunameEntry.Text = students.Surname;
            PatronymicEntry.Text = students.Patronymic;
            GroupEntry.Text = students.Group;

            List<LessonsInfoGrades> lessonsInfoGrades = await _courseworkDatebase.GetLessonsInfoGradesAsync(ID_user);
            ListView listView = new ListView
            {
                ItemsSource = lessonsInfoGrades
            };

            DataTemplate dataTemplate = new DataTemplate(() =>
            {
                Grid grid = new Grid
                {
                    Margin = 1,
                    RowDefinitions =
                    {
                    new RowDefinition { Height = GridLength.Star },
                    new RowDefinition { Height = GridLength.Star }
                    },
                    ColumnDefinitions =
                    {
                    new ColumnDefinition { Width = new GridLength(4, GridUnitType.Star) },
                    new ColumnDefinition { Width = GridLength.Star },
                    new ColumnDefinition { Width = GridLength.Star }
                    }
                };

                Label labelName = new Label();
                labelName.FontSize = 18;
                Label labelDate = new Label();
                labelDate.FontSize = 18;
                Label labelEst = new Label();
                labelEst.FontSize = 26;
                Label labelAtt = new Label();
                labelAtt.FontSize = 26;

                grid.Children.Add(labelName);
                grid.Children.Add(labelDate);
                grid.Children.Add(labelEst);
                grid.Children.Add(labelAtt);

                labelName.SetBinding(Label.TextProperty, new Binding("Name", converter: new ToStringConverter()));
                labelDate.SetBinding(Label.TextProperty, new Binding("Date_lesson", converter: new DateWithoutTimeConverter()));
                labelEst.SetBinding(Label.TextProperty, new Binding("Est", converter: new ToStringConverter()));
                labelAtt.SetBinding(Label.TextProperty, new Binding("Att", converter: new ToStringConverter()));

                Grid.SetColumn(labelName, 0);
                Grid.SetRow(labelName, 0);
                Grid.SetColumn(labelDate, 0);
                Grid.SetRow(labelDate, 1);
                Grid.SetColumn(labelEst, 1);
                Grid.SetRow(labelEst, 0);
                Grid.SetRowSpan(labelEst, 2);
                Grid.SetColumn(labelAtt, 2);
                Grid.SetRow(labelAtt, 0);
                Grid.SetRowSpan(labelAtt, 2);

                return new ViewCell
                {
                    View = grid
                };
            });

            listView.ItemTemplate = dataTemplate;

            StackL.Add(listView);
        }

    }
    private async Task RefreshListView()
    {
        // Очистка текущего списка элементов в ListView
        listViewLessons.ItemsSource = null;
        // Получение обновленных данных из базы данных
        listViewLessons.ItemsSource = await _courseworkDatebase.GetLessonsAsync(MauiProgram.idGlobal);
    }

    private async void BttnSave_Clicked(object sender, EventArgs e)
    {
        if (PasswordEntry.Text != "")
        {
            using SHA256 sha256Hash = SHA256.Create();
            byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(PasswordEntry.Text));
            string hashedPassword = BitConverter.ToString(bytes).Replace("-", "");
            users.Password = hashedPassword;
        } 
        users.Login = LoginEntry.Text;

        await _courseworkDatebase.UpdateUser(users);
        if (Role_user == 2)
        {
            teachers.Name = NameEntry.Text;
            teachers.Surname = SunameEntry.Text;
            teachers.Patronymic = PatronymicEntry.Text;
            await _courseworkDatebase.UpdateTeacher(teachers);
        } else if (Role_user == 3)
        {
            students.Name = NameEntry.Text;
            students.Surname = SunameEntry.Text;
            students.Patronymic = PatronymicEntry.Text;
            students.Group = GroupEntry.Text;
            await _courseworkDatebase.UpdateStudent(students);
        }
    }
}
public class ToStringConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value?.ToString(); // Преобразование значения в строку
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
public class DateWithoutTimeConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is DateTime dateTime)
        {
            return dateTime.ToString("dd.MM.yyyy"); // Форматируйте дату согласно вашим требованиям
        }

        return string.Empty;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}