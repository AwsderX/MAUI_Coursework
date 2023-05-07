using MAUI_Coursework.Data;
using MAUI_Coursework.Models;
using System.ComponentModel;

namespace MAUI_Coursework.Views;

public partial class GradesView : ContentPage, INotifyPropertyChanged
{
	string group;
	int Id_lesson;
	CourseworkDatebase _courseworkDatebase = new();
	public List<Students> students = new();
	public GradesView(string group, int Id_lesson)
	{
		InitializeComponent();
		this.group = group;
        this.Id_lesson = Id_lesson;

        BindingContext = this;
        LoadData();
    }
	public async void LoadData()
	{
		//students.Clear();
        List<Students> newStudents = await _courseworkDatebase.GetStudentsAsync(group);
        foreach (var student in newStudents)
        {
            students.Add(student);
        }
        OnPropertyChanged(nameof(students));
    }

    private void OnItemTapped(object sender, ItemTappedEventArgs e)
    {

    }
}