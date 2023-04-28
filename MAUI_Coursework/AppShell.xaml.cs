using MAUI_Coursework.Views;

namespace MAUI_Coursework;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
        RegisterRoutes();
        BindingContext = this;

    }

    private void Start_Clicked(object sender, EventArgs e)
    {

    }

    void RegisterRoutes()
    {
        Routing.RegisterRoute("aboutPage", typeof(AboutPage));
    }
}
