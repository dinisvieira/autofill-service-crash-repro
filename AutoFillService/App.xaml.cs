namespace AutoFillService
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            // Does not crash
            //MainPage = new AppShell();

            // This one and other non-shell TabbedPage/NavigationPage/Page combinations all crash
            MainPage = new NavigationPage(new MainPage());

            //Also crashes
            //MainPage = new MainPage(); //Also crashes

            //Also crashes
            /*
            MainPage = new TabbedPage()
            {
                Children = { new NavigationPage(new MainPage()) }
            };
            */
        }
    }
}
