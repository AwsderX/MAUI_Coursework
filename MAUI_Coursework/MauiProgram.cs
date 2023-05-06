using MAUI_Coursework.Data;
using MAUI_Coursework.Views;

namespace MAUI_Coursework;

public static class MauiProgram
{
	public static int idGlobal = 0;
	public static int roleGlobal = 0;
	public static string loginGlobal = "";
    public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

		builder.Services.AddSingleton<InfoStudent>();
        builder.Services.AddTransient<Models.TodoItem>();
        builder.Services.AddSingleton<CourseworkDatebase>();

        return builder.Build();
	}
}
