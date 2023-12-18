using Cinema.Database;
using Cinema.Database.Models;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Cinema.Views.Pages.Cards
{
    public partial class SessionCardPage : Page
    {
        private Session session;
        bool isCreating = true;
        public SessionCardPage(Session _session, bool isCreating = false)
        {
            this.isCreating = isCreating;
            session = _session;
            InitializeComponent();
        }

        private void BackClick(object sender, RoutedEventArgs e)
        {
            if (isCreating) DatabaseContext.Instance.Sessions.Remove(session);
            Navigation.SetPage(new TablePage("Сеансы"));
        }

        private void SaveClick(object sender, RoutedEventArgs e)
        {
            if (movieBox.SelectedItem == null || customerBox.SelectedItem == null || hallBox.SelectedItem == null)
            {
                MessageBox.Show("Заполните все поля");
                return;
            }

            session.Customer = customerBox.SelectedItem as Customer;
            session.Hall = hallBox.SelectedItem as Hall;
            session.Movie = movieBox.SelectedItem as Movie;
            DatabaseContext.Instance.SaveChanges();
            Navigation.SetPage(new TablePage("Сеансы"));
        }

        private void PageLoaded(object sender, RoutedEventArgs e)
        {
            movieBox.ItemsSource = DatabaseContext.Instance.Movies.ToList();
            customerBox.ItemsSource = DatabaseContext.Instance.Customers.ToList();
            hallBox.ItemsSource = DatabaseContext.Instance.Halls.ToList();

            movieBox.SelectedItem = session.Movie;
            customerBox.SelectedItem = session.Customer;
            hallBox.SelectedItem = session.Hall;
        }
    }
}
