using Cinema.Database;
using Cinema.Database.Models;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Cinema.Views.Pages.Cards
{
    public partial class UserCardPage : Page
    {
        private User user;
        bool isCreating = true;
        public UserCardPage(User _user, bool isCreating = false)
        {
            this.isCreating = isCreating;
            user = _user;
            InitializeComponent();
        }

        private void BackClick(object sender, RoutedEventArgs e)
        {
            
            if (isCreating) DatabaseContext.Instance.Users.Remove(user);
            Navigation.SetPage(new TablePage("Пользователи"));
        }

        private void SaveClick(object sender, RoutedEventArgs e)
        {
            User loginOwner = DatabaseContext.Instance.Users.FirstOrDefault(u => u.Login == loginBox.Text);

            if (nameBox.Text.Trim() == "" || lastnameBox.Text.Trim() == "" || roleBox.SelectedItem == null || loginBox.Text == "" || (passwordBox.Password ==  "" && loginOwner == null))
            {
                MessageBox.Show("Заполните все поля");
                return;
            }


            if (loginOwner != null && loginOwner.Id != user.Id)
            {
                MessageBox.Show("Данный логин уже используется", "Логин занят", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            user.FirstName = nameBox.Text;
            user.LastName = lastnameBox.Text;
            user.Login = loginBox.Text;
            if (passwordBox.Password.Length > 0)
            {
                user.Password = passwordBox.Password;
            }
            user.Role = DatabaseContext.Instance.Roles.Find((roleBox.SelectedItem as Role).Id);
            DatabaseContext.Instance.SaveChanges();
            Navigation.SetPage(new TablePage("Пользователи"));
        }

        private void PageLoaded(object sender, RoutedEventArgs e)
        {
            nameBox.Text = user.FirstName;
            lastnameBox.Text = user.LastName;
            roleBox.ItemsSource = DatabaseContext.Instance.Roles.Where(r => r.Id != 1).ToList();
            roleBox.SelectedItem = user.Role;
            loginBox.Text = user.Login;
        }
    }
}
