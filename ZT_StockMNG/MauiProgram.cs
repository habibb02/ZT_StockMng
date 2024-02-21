using Microsoft.Extensions.Logging;
using ZT_StockMNG.View.StockArticle;
using ZT_StockMNG.ViewModel;

namespace ZT_StockMNG
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
                    //default
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");

                    //added
                    fonts.AddFont("CenturyGothic-Regular.TTF", "CenturyGothic");
                    fonts.AddFont("CenturyGothic-Bold.TTF", "CenturyGothicBold");
                });

#if DEBUG
            builder.Logging.AddDebug();
#endif

            builder.Services.AddSingleton<MainPageViewModel>();
            builder.Services.AddSingleton<NewStockArticleViewModel>();

            builder.Services.AddSingleton<MainPage>();
            builder.Services.AddSingleton<NewStockArticle>();

            return builder.Build();
        }
    }
}