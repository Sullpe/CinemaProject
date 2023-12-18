using System.Windows;
using System.Windows.Controls;

namespace Cinema.Views.Pages
{
    public partial class MenuPage : Page
    {
        public MenuPage()
        {
            InitializeComponent();

            NameText.Text = $"Имя: {CurrentUser.User.FullName}";
            RoleText.Text = $"Роль: {CurrentUser.User.Role.Name}";

            if (CurrentUser.User.Role.Id == 1)
            {
                CustomerButton.Visibility = Visibility.Collapsed;
                SessionButton.Visibility = Visibility.Collapsed;
                UserButton.Visibility = Visibility.Collapsed;
            }
            else if (CurrentUser.User.Role.Id == 2)
            {
                UserButton.Visibility = Visibility.Collapsed;
            }
        }

        private void QuitClick(object sender, RoutedEventArgs e)
        {
            CurrentUser.User = null;
            Navigation.SetPage(new LoginPage());
        }

        private void TableButtonClick(object sender, RoutedEventArgs e)
        {
            Navigation.SetPage(new TablePage((sender as Button).Content as string));
        }
    }
}
