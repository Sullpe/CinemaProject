using Cinema.Database;
using Cinema.Database.Models;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Cinema.Views.Pages
{
    public partial class LoginPage : Page
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        private void Authorization(string login, string password)
        {

            User user = DatabaseContext.Instance.Users.Include("Role").SingleOrDefault(u => u.Login == login && u.Password == password);

            if (user == null)
            {
                MessageBox.Show("Неверный логин и / или пароль");
                return;
            }

            CurrentUser.User = user;
            Navigation.SetPage(new MenuPage());
        }

        private void LoginAsGuest(object sender, RoutedEventArgs e)
        {
            Authorization("guest", "guest");
        }

        private void Login(object sender, RoutedEventArgs e)
        {
            Authorization(loginBox.Text, passwordBox.Password);
        }
    }
}
