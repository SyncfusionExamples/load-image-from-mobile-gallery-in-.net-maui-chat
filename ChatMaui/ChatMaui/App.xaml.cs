namespace ChatMaui
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new MainPage();
        }

        //public static IServiceProvider ServiceProvider { get; private set; }

        //public App(IServiceProvider serviceProvider)
        //{
        //    InitializeComponent();
        //    ServiceProvider = serviceProvider;

        //    MainPage = new MainPage();
        //}
    }
}
