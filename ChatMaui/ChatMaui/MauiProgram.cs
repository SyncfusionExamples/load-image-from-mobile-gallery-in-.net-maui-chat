using AndroidX.Lifecycle;
using Microsoft.Extensions.Logging;
using Syncfusion.Maui.Core.Hosting;

namespace ChatMaui
{
    public static class MauiProgram
    {
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

#if DEBUG
            builder.Logging.AddDebug();
#endif
            builder.ConfigureSyncfusionCore();
            // Register the ImageService with the DI container
            //builder.Services.AddSingleton<IImageService, ImageService>();

            // Register your ViewModel
            //builder.Services.AddTransient<ViewModel>();
            
            return builder.Build();
        }
    }
}
