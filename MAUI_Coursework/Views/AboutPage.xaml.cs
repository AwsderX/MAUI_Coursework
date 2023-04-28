using MAUI_Coursework.Views;
using MAUI_Coursework.Models;
using MAUI_Coursework.Data;

namespace MAUI_Coursework.Views;

public partial class AboutPage : ContentPage
{
	public AboutPage(CourseworkDatebase todoItemDatabase)
	{
		InitializeComponent();
        database = todoItemDatabase;
    }

    TodoItem item = new TodoItem();
    CourseworkDatebase database;

    private async void LearnMore_Clicked(object sender, EventArgs e)
    {
        item.Name = "223";
        item.Notes = "5555";
        item.Done = true;
        await database.SaveItemAsync(item);
    }
}