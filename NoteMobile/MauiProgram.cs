using Camera.MAUI;
using Microsoft.Extensions.Logging;
using NoteMobile.DataAccess;
using NoteMobile.ViewModels;
using NoteMobile.Views;
using About = NoteMobile.Views.About;
using AllNotes = NoteMobile.ViewModels.AllNotes;

namespace NoteMobile;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.UseMauiCameraView()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});
		builder.Services.AddTransient<AllNotes>();
		builder.Services.AddTransient<Note>();
		builder.Services.AddTransient<Views.AllNotes>();
		builder.Services.AddTransient<NotePage>();
		builder.Services.AddSingleton<About>();
		builder.Services.AddDbContext<AppDbContext>();
		
#if DEBUG
		builder.Logging.AddDebug();
#endif

		var app = builder.Build();

		using (var scope = 
		       app.Services.CreateScope())
		using (var context = scope.ServiceProvider.GetService<AppDbContext>())
			context?.Database.EnsureCreated();
		
		return app;
	}
}