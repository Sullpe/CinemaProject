using Cinema.Database;
using Cinema.Database.Models;
using System.Windows;
using System.Windows.Controls;

namespace Cinema.Views.Pages.Cards
{
    public partial class HallCardPage : Page
    {
        private Hall hall;
        bool isCreating;
        public HallCardPage(Hall _hall, bool isCreating = false)
        {
            this.isCreating = isCreating;
            hall = _hall;
            InitializeComponent();
        }

        private void BackClick(object sender, RoutedEventArgs e)
        {
            if (isCreating) DatabaseContext.Instance.Halls.Remove(hall);
            Navigation.SetPage(new TablePage("Кинозалы"));
        }

        private void SaveClick(object sender, RoutedEventArgs e)
        {
            if (placesBox.Text.Trim() == "")
            {
                MessageBox.Show("Заполните все поля");
                return;
            }

            if (!int.TryParse(placesBox.Text, out int places))
            {
                MessageBox.Show("Не удалось преобразовать в число");
                return;
            }

            hall.Places = places;
            DatabaseContext.Instance.SaveChanges();
            BackClick(null, null);
        }

        private void PageLoaded(object sender, RoutedEventArgs e)
        {
            placesBox.Text = hall.Places.ToString();
        }
    }
}
