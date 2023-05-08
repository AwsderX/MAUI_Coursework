using MAUI_Coursework.Data;
using MAUI_Coursework.Views;
using ZXing.Net.Maui;
using ZXing.Net.Maui.Controls;

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
            .UseBarcodeReader()
            .ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			})
        #region
            .ConfigureMauiHandlers(h =>
            {
                h.AddHandler(typeof(ZXing.Net.Maui.Controls.CameraBarcodeReaderView), typeof(CameraBarcodeReaderViewHandler));
                h.AddHandler(typeof(ZXing.Net.Maui.Controls.CameraView), typeof(CameraViewHandler));
                h.AddHandler(typeof(ZXing.Net.Maui.Controls.BarcodeGeneratorView), typeof(BarcodeGeneratorViewHandler));
            });
        #endregion

        builder.Services.AddSingleton<InfoStudent>();
        builder.Services.AddTransient<Models.TodoItem>();
        builder.Services.AddSingleton<CourseworkDatebase>();

        return builder.Build();
	}

}
