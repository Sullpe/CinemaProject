using Cinema.Database;
using Cinema.Database.Models;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Cinema.Views.Pages.Cards
{
    public partial class MovieCardPage : Page
    {
        private Movie movie;
        bool isCreating = true;
        public MovieCardPage(Movie _movie, bool isCreating = false)
        {
            this.isCreating = isCreating;
            movie = _movie;
            InitializeComponent();
        }

        private void BackClick(object sender, RoutedEventArgs e)
        {
            if (isCreating) DatabaseContext.Instance.Movies.Remove(movie);
            Navigation.SetPage(new TablePage("Фильмы"));
        }

        private void SaveClick(object sender, RoutedEventArgs e)
        {
            if (nameBox.Text.Trim() == "" || priceBox.Text.Trim() == "" || dateBox.SelectedDate == null || genreBox.SelectedItem == null)
            {
                MessageBox.Show("Заполните все поля");
                return;
            }

            if (!int.TryParse(priceBox.Text, out int price))
            {
                MessageBox.Show("Не удалось преобразовать строку в число!");
                return;
            }

            movie.Name = nameBox.Text;
            movie.Price = price;
            movie.ShowDate = dateBox.SelectedDate.Value;
            movie.Genre = genreBox.SelectedItem as Genre;
            DatabaseContext.Instance.SaveChanges();
            Navigation.SetPage(new TablePage("Фильмы"));
        }

        private void PageLoaded(object sender, RoutedEventArgs e)
        {
            nameBox.Text = movie.Name;
            priceBox.Text = movie.Price.ToString();
            dateBox.SelectedDate = movie.ShowDate;
            genreBox.ItemsSource = DatabaseContext.Instance.Genres.ToList();
            genreBox.SelectedItem = movie.Genre;
        }
    }
}
