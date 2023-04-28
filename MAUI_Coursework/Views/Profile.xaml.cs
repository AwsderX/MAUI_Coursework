using MAUI_Coursework.Data;
using MAUI_Coursework.Models;

namespace MAUI_Coursework.Views;

public partial class Profile : ContentPage
{
    CourseworkDatebase _courseworkDatebase = new();
	public Profile()
	{
		InitializeComponent();
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
            // «агрузка выбранного изображени€
            var stream = await result.OpenReadAsync();
            using (var ms = new MemoryStream())
            {
                await stream.CopyToAsync(ms);
                byte[] imageBytes = ms.ToArray();
                //var imageSource = ImageSource.FromStream(() => new MemoryStream(imageBytes));
                if (MauiProgram.roleGlobal == 2)
                {
                    Teachers teachers;
                    teachers = await _courseworkDatebase.GetTeacherAsync(MauiProgram.idGlobal); 
                    teachers.Photo = imageBytes;
                    await _courseworkDatebase.UpdateTeacher(teachers);
                } else if (MauiProgram.roleGlobal == 3)//студент
                {
                    Students students;
                    students = await _courseworkDatebase.GetStudentAsync(MauiProgram.idGlobal);
                    students.Photo = imageBytes;
                    await _courseworkDatebase.UpdateStudent(students);
                }
            }
        }
    }
}