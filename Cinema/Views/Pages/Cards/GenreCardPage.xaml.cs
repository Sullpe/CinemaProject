using Cinema.Database;
using Cinema.Database.Models;
using System.Windows;
using System.Windows.Controls;

namespace Cinema.Views.Pages.Cards
{
    public partial class GenreCardPage : Page
    {
        private Genre genre;
        bool isCreating;
        public GenreCardPage(Genre _genre, bool isCreating = false)
        {
            this.isCreating = isCreating;
            genre = _genre;
            InitializeComponent();
        }

        private void BackClick(object sender, RoutedEventArgs e)
        {
            if (isCreating) DatabaseContext.Instance.Genres.Remove(genre);
            Navigation.SetPage(new TablePage("Жанры"));
        }

        private void SaveClick(object sender, RoutedEventArgs e)
        {
            if (nameBox.Text.Trim() == "")
            {
                MessageBox.Show("Заполните все поля");
                return;
            }

            genre.Name = nameBox.Text;
            DatabaseContext.Instance.SaveChanges();
            Navigation.SetPage(new TablePage("Жанры"));
        }

        private void PageLoaded(object sender, RoutedEventArgs e)
        {
            nameBox.Text = genre.Name;
        }
    }
}
