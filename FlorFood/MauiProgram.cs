using Microsoft.Extensions.Logging;
using Syncfusion.Maui.Core.Hosting;

namespace FlorFood;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
            .ConfigureSyncfusionCore()
            .UseMauiMaps()
            .ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
				fonts.AddFont("Montserrat-Light.tts", "Titulo");
				fonts.AddFont("Rubik-SemiBoldItalic.ttf", "Rubik");
			});

		builder.Logging.AddDebug();

		return builder.Build();
	}
}
