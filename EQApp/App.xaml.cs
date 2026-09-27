using EQApp.Views;

namespace EQApp;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        // Исправленный способ установки MainPage
        MainPage = new NavigationPage(new MainPage());
    }
}