using Cinema.Database;
using Cinema.Views.Pages;
using System.Windows;

namespace Cinema.Views.Windows
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            DatabaseContext.Preload();
            InitializeComponent();
            Navigation.frame = frame;
            Navigation.SetPage(new LoginPage());
        }
    }
}
