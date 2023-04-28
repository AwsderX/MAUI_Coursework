using System.Security.Cryptography;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace MAUI_Coursework.Views;

public partial class RegistrationPage : ContentPage
{
    Data.CourseworkDatebase courseworkDatebase = new();
    Models.Users user;
    Models.Teachers teacher;
    Models.Students student;
    Entry group_entry = new();

    public RegistrationPage()
	{
		InitializeComponent();
        group_entry.Placeholder = "Группа";
        //group_entry.Margin = 20;
        group_entry.FontSize = 18;
        group_entry.Text = "";

    }

    private async void Register_Clicked(object sender, EventArgs e)
    {
        user = new();
        if (login_entry.Text != "" && password_entry.Text != "" && password_repeat_entry.Text != ""
            && name_entry.Text != "" && surname_entry.Text != "" && patronymic_entry.Text != ""
            && ((role_Switch.IsToggled && group_entry.Text != "") || !role_Switch.IsToggled))
        {
            if (password_entry.Text == password_repeat_entry.Text)
            {
                user.Login = login_entry.Text;
                using SHA256 sha256Hash = SHA256.Create();
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password_entry.Text));
                string hashedPassword = BitConverter.ToString(bytes).Replace("-", "");
                user.Password = hashedPassword;
                if (role_Switch.IsToggled) user.Role = 3; else user.Role = 2;
                if (await courseworkDatebase.GetUserAsync(user.Login) != null) { await DisplayAlert("Ошибка", "Логин занят", "OK"); return; }
                int a = await courseworkDatebase.SaveUserAsync(user);
                if (role_Switch.IsToggled)
                {
                    student = new()
                    {
                        ID_user = a,
                        Name = name_entry.Text,
                        Surname = surname_entry.Text,
                        Patronymic = patronymic_entry.Text,
                        Group = group_entry.Text,
                        Photo = null
                    };
                    a = await courseworkDatebase.SaveStudentAsync(student);
                } else
                {
                    teacher = new()
                    {
                        ID_user = a,
                        Name = name_entry.Text,
                        Surname = surname_entry.Text,
                        Patronymic = patronymic_entry.Text,
                        Photo = null
                    };                    
                    a = await courseworkDatebase.SaveTeacherAsync(teacher);
                }
                if (a != 0)
                {
                    login_entry.Text = "";
                    password_entry.Text = "";
                    password_repeat_entry.Text = "";
                    name_entry.Text = "";
                    surname_entry.Text = "";
                    patronymic_entry.Text = "";
                    if (role_Switch.IsToggled) group_entry.Text = "";
                    role_Switch.IsToggled = false;
                    await DisplayAlert("Добавлено!", "Регистрация успешна", "OK");
                }

            }
            else await DisplayAlert("Ошибка", "Пароли не совпадают", "OK");
        }
        else await DisplayAlert("Ошибка", "Не все данные заполнены", "OK");
    }

    private async void role_Switch_Toggled(object sender, ToggledEventArgs e)
    {

        if (role_Switch.IsToggled)
        {
            // Анимация перемещения элементов под новым элементом вниз
            var moveDownEntryAnimation = new Animation(
                d => grid_switch.TranslationY = d,
                start: grid_switch.TranslationY,
                end: grid_switch.TranslationY + 64,
                easing: Easing.SinInOut);


            // Анимация перемещения элементов над новым элементом вверх
            var moveUpEntryAnimation = new Animation(
                d => grid_switch.TranslationY = d,
                start: group_entry.TranslationY,
                end: group_entry.TranslationY,
                easing: Easing.SinInOut);
            // Запускаем анимации перемещения элементов

            moveDownEntryAnimation.Commit(CP, "moveDownEntryAnimation", length: 500);
            await Task.Delay(500);

            var appearAnimation = new Animation(
            d => group_entry.Opacity = d,
            start: 0,
            end: 1,
            easing: Easing.SinInOut);

            VSL.Insert(7, group_entry);

            appearAnimation.Commit(
            owner: group_entry,
            name: "AppearAnimation",
            length: 500,
            finished: (d, b) => group_entry.Opacity = 1);

            moveUpEntryAnimation.Commit(CP, "moveUpEntryAnimation", length: 0);

        } else
        {
            // Анимация перемещения элементов под новым элементом вниз
            var moveDownEntryAnimation = new Animation(
                d => grid_switch.TranslationY = d,
                start: grid_switch.TranslationY,
                end: grid_switch.TranslationY + 64,
                easing: Easing.SinInOut);


            // Анимация перемещения элементов над новым элементом вверх
            var moveUpEntryAnimation = new Animation(
                d => grid_switch.TranslationY = d,
                start: grid_switch.TranslationY+64,
                end: password_repeat_entry.TranslationY,
                easing: Easing.SinInOut);

            var appearAnimation = new Animation(
                d => group_entry.Opacity = d,
                start: 1,
                end: 0,
                easing: Easing.SinInOut);

            appearAnimation.Commit(
                owner: group_entry,
                name: "AppearAnimation",
                length: 300,
                finished: (d, b) => group_entry.Opacity = 0);

            await Task.Delay(300);
            moveDownEntryAnimation.Commit(CP, "moveDownEntryAnimation", length: 0);
            VSL.Remove(group_entry);
            moveUpEntryAnimation.Commit(CP, "moveUpEntryAnimation", length: 500);

        }
    }
}