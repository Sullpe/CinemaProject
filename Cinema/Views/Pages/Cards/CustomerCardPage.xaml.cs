using Cinema.Database;
using Cinema.Database.Models;
using System.Windows;
using System.Windows.Controls;

namespace Cinema.Views.Pages.Cards
{
    public partial class CustomerCardPage : Page
    {
        private Customer customer;
        bool isCreating;
        public CustomerCardPage(Customer _customer, bool isCreating = false)
        {
            this.isCreating = isCreating;
            customer = _customer;
            InitializeComponent();
        }

        private void BackClick(object sender, RoutedEventArgs e)
        {
            if (isCreating) DatabaseContext.Instance.Customers.Remove(customer);
            Navigation.SetPage(new TablePage("Покупатели"));
        }

        private void SaveClick(object sender, RoutedEventArgs e)
        {
            if (nameBox.Text.Trim() == "" || lastnameBox.Text.Trim() == "")
            {
                MessageBox.Show("Заполните все поля");
                return;
            }

            customer.FirstName = nameBox.Text;
            customer.LastName = lastnameBox.Text;
            DatabaseContext.Instance.SaveChanges();
            Navigation.SetPage(new TablePage("Покупатели"));
        }

        private void PageLoaded(object sender, RoutedEventArgs e)
        {
            nameBox.Text = customer.FirstName;
            lastnameBox.Text = customer.LastName;
        }
    }
}
