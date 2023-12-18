using Cinema.Database;
using Cinema.Database.Models;
using Cinema.Views.Pages.Cards;
using Microsoft.Win32;
using OfficeOpenXml;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Cinema.Views.Pages
{
    public partial class TablePage : Page
    {
        private string table;
        public TablePage(string _table)
        {
            table = _table;
            InitializeComponent();

            if (CurrentUser.User.Role.Id == 1)
            {
                AddButton.Visibility = Visibility.Collapsed;
                DeleteButton.Visibility = Visibility.Collapsed;
            }
        }

        public DataGridTextColumn CreateColumn(string header, string bindingString, bool isDate = false)
        {
            Binding binding = new Binding(bindingString);
            if (isDate) binding.StringFormat = "dd.MM.yyyy";
            return new DataGridTextColumn() { Binding = binding, Header = header, IsReadOnly = true, };
        }

        private void UpdateGrid()
        {
            dataGrid.Columns.Clear();
            string searchText = searchTextBox.Text.ToLower().Trim();

            switch (table)
            {
                case "Фильмы":
                    dataGrid.Columns.Add(CreateColumn("Название", "Name"));
                    dataGrid.Columns.Add(CreateColumn("Жанр", "Genre.Name"));
                    dataGrid.Columns.Add(CreateColumn("Цена", "Price"));
                    dataGrid.Columns.Add(CreateColumn("Дата", "ShowDate", true));

                    dataGrid.ItemsSource = DatabaseContext.Instance.Movies.Include("Genre").ToList()
                                            .Where(m => m.Name.ToLower().Trim().Contains(searchText) ||
                                                        m.Price.ToString().Contains(searchText) ||
                                                        m.Genre.Name.ToLower().Trim().Contains(searchText) ||
                                                        m.ShowDate.ToString().Contains(searchText));
                    break;

                case "Кинозалы":
                    dataGrid.Columns.Add(CreateColumn("Название", "FullName"));
                    dataGrid.Columns.Add(CreateColumn("Мест", "Places"));

                    dataGrid.ItemsSource = DatabaseContext.Instance.Halls.ToList()
                                            .Where(h => h.FullName.ToLower().Trim().Contains(searchText) ||
                                                        h.Places.ToString().Contains(searchText));
                    break;

                case "Покупатели":
                    dataGrid.Columns.Add(CreateColumn("Фамилия", "LastName"));
                    dataGrid.Columns.Add(CreateColumn("Имя", "FirstName"));

                    dataGrid.ItemsSource = DatabaseContext.Instance.Customers.ToList()
                                            .Where(c => c.FullName.ToLower().Trim().Contains(searchText));
                    break;

                case "Сеансы":
                    dataGrid.Columns.Add(CreateColumn("Фильм", "Movie.Name"));
                    dataGrid.Columns.Add(CreateColumn("Покупатель", "Customer.FullName"));
                    dataGrid.Columns.Add(CreateColumn("Кинозал", "Hall.FullName"));
                    dataGrid.Columns.Add(CreateColumn("Дата", "SaleDate", true));

                    dataGrid.ItemsSource = DatabaseContext.Instance.Sessions.Include("Movie").Include("Customer").Include("Hall").ToList()
                                            .Where(s => s.Movie.Name.ToLower().Trim().Contains(searchText) ||
                                                        s.Customer.FirstName.ToLower().Trim().Contains(searchText) ||
                                                        s.Hall.FullName.ToLower().Trim().Contains(searchText) ||
                                                        s.SaleDate.ToString().Contains(searchText));
                    break;

                case "Пользователи":
                    dataGrid.Columns.Add(CreateColumn("Имя", "FirstName"));
                    dataGrid.Columns.Add(CreateColumn("Фамилия", "LastName"));
                    dataGrid.Columns.Add(CreateColumn("Роль", "Role.Name"));
                    dataGrid.Columns.Add(CreateColumn("Логин", "Login"));

                    dataGrid.ItemsSource = DatabaseContext.Instance.Users.Include("Role").Where(user => user.Role.Id != 1).ToList()
                                            .Where(u => u.FullName.ToLower().Trim().Contains(searchText) ||
                                                        u.Login.ToLower().Trim().Contains(searchText) ||
                                                        u.Role.Name.ToLower().Trim().Contains(searchText));
                    break;

                case "Жанры":
                    dataGrid.Columns.Add(CreateColumn("Название", "Name"));

                    dataGrid.ItemsSource = DatabaseContext.Instance.Genres.ToList()
                                            .Where(g => g.Name.ToLower().Trim().Contains(searchText));
                    break;
            }
        }

        private void AddClick(object sender, RoutedEventArgs e)
        {
            switch (table)
            {
                case "Фильмы":
                    Movie newMovie = DatabaseContext.Instance.Movies.Add(new Movie());
                    Navigation.SetPage(new MovieCardPage(newMovie, true));
                    break;

                case "Жанры":
                    Genre newGenre = DatabaseContext.Instance.Genres.Add(new Genre());
                    Navigation.SetPage(new GenreCardPage(newGenre, true));
                    break;

                case "Покупатели":
                    Customer newCustomer = DatabaseContext.Instance.Customers.Add(new Customer());
                    Navigation.SetPage(new CustomerCardPage(newCustomer, true));
                    break;

                case "Кинозалы":
                    Hall newHall = DatabaseContext.Instance.Halls.Add(new Hall());
                    Navigation.SetPage(new HallCardPage(newHall, true));
                    break;

                case "Сеансы":
                    Session newSession = DatabaseContext.Instance.Sessions.Add(new Session());
                    Navigation.SetPage(new SessionCardPage(newSession, true));
                    break;

                case "Пользователи":
                    User newUser = DatabaseContext.Instance.Users.Add(new User());
                    Navigation.SetPage(new UserCardPage(newUser, true));
                    break;
            }
        }

        private void DataGridDoubleClick(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedItem == null) return;

            switch (table)
            {
                case "Фильмы":
                    Navigation.SetPage(new MovieCardPage(dataGrid.SelectedItem as Movie));
                    break;

                case "Жанры":
                    Navigation.SetPage(new GenreCardPage(dataGrid.SelectedItem as Genre));
                    break;

                case "Покупатели":
                    Navigation.SetPage(new CustomerCardPage(dataGrid.SelectedItem as Customer));
                    break;

                case "Кинозалы":
                    Navigation.SetPage(new HallCardPage(dataGrid.SelectedItem as Hall));
                    break;

                case "Сеансы":
                    Navigation.SetPage(new SessionCardPage(dataGrid.SelectedItem as Session));
                    break;

                case "Пользователи":
                    Navigation.SetPage(new UserCardPage(dataGrid.SelectedItem as User));
                    break;
            }
        }

        private void DeleteClick(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedItem == null) return;

            if (MessageBox.Show("Действительно хотите удалить?", "Удаление", MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
                return;

            switch (table)
            {
                case "Фильмы":
                    DatabaseContext.Instance.Movies.Remove(dataGrid.SelectedItem as Movie);
                    break;

                case "Жанры":
                    DatabaseContext.Instance.Genres.Remove(dataGrid.SelectedItem as Genre);
                    break;

                case "Покупатели":
                    DatabaseContext.Instance.Customers.Remove(dataGrid.SelectedItem as Customer);
                    break;

                case "Кинозалы":
                    DatabaseContext.Instance.Halls.Remove(dataGrid.SelectedItem as Hall);
                    break;

                case "Сеансы":
                    DatabaseContext.Instance.Sessions.Remove(dataGrid.SelectedItem as Session);
                    break;

                case "Пользователи":
                    if ((dataGrid.SelectedItem as User).Id == CurrentUser.User.Id)
                    {
                        MessageBox.Show("Вы не можете удалить запись с которой вошли", "Операция не возможна", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    DatabaseContext.Instance.Users.Remove(dataGrid.SelectedItem as User);
                    break;
            }

            DatabaseContext.Instance.SaveChanges();
            UpdateGrid();
        }

        private void ExportClick(object sender, RoutedEventArgs e)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (ExcelPackage excelPackage = new ExcelPackage())
            {
                ExcelWorksheet excelWorksheet = excelPackage.Workbook.Worksheets.Add(table);

                for (int i = 0; i < dataGrid.Columns.Count(); i++)
                    excelWorksheet.Cells[1, i + 1].Value = dataGrid.Columns[i].Header;

                for (int row = 0; row < dataGrid.Items.Count; row++)
                {
                    for (int col = 0; col < dataGrid.Columns.Count; col++)
                    {
                        switch (table)
                        {
                            case "Кинозалы":
                                List<Hall> halls = DatabaseContext.Instance.Halls.ToList();
                                excelWorksheet.Cells[row + 2, 1].Value = halls[row].FullName;
                                excelWorksheet.Cells[row + 2, 2].Value = halls[row].Places;
                                break;

                            case "Фильмы":
                                List<Movie> movies = DatabaseContext.Instance.Movies.ToList();
                                excelWorksheet.Cells[row + 2, 1].Value = movies[row].Name;
                                excelWorksheet.Cells[row + 2, 2].Value = movies[row].Genre.Name;
                                excelWorksheet.Cells[row + 2, 3].Value = movies[row].Price;
                                excelWorksheet.Cells[row + 2, 4].Value = movies[row].ShowDate.ToString();
                                break;

                            case "Жанры":
                                List<Genre> genres = DatabaseContext.Instance.Genres.ToList();
                                excelWorksheet.Cells[row + 2, 1].Value = genres[row].Name;
                                break;

                            case "Пользователи":
                                List<User> users = DatabaseContext.Instance.Users.ToList();
                                excelWorksheet.Cells[row + 2, 1].Value = users[row].FirstName;
                                excelWorksheet.Cells[row + 2, 2].Value = users[row].LastName;
                                excelWorksheet.Cells[row + 2, 3].Value = users[row].Role.Name;
                                excelWorksheet.Cells[row + 2, 4].Value = users[row].Login;
                                break;

                            case "Сеансы":
                                List<Session> sessions = DatabaseContext.Instance.Sessions.ToList();
                                excelWorksheet.Cells[row + 2, 1].Value = sessions[row].Movie.Name;
                                excelWorksheet.Cells[row + 2, 1].Value = sessions[row].Customer.FullName;
                                excelWorksheet.Cells[row + 2, 1].Value = sessions[row].Hall.FullName;
                                excelWorksheet.Cells[row + 2, 1].Value = sessions[row].SaleDate.ToString();
                                break;

                            case "Покупатели":
                                List<Customer> customers = DatabaseContext.Instance.Customers.ToList();
                                excelWorksheet.Cells[row + 2, 1].Value = customers[row].FirstName;
                                excelWorksheet.Cells[row + 2, 2].Value = customers[row].LastName;
                                break;
                        }
                    }
                }

                SaveFileDialog saveFileDialog = new SaveFileDialog() { FileName = table, DefaultExt = ".xlsx" };
                if (saveFileDialog.ShowDialog() == true)
                {
                    try
                    {
                        excelPackage.SaveAs(new FileInfo(saveFileDialog.FileName));
                        MessageBox.Show("Успешно сохранено", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    catch
                    {
                        MessageBox.Show("Не удалось сохранить файл", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private void BackClick(object sender, RoutedEventArgs e)
        {
            Navigation.SetPage(new MenuPage());
        }

        private void SearchChange(object sender, TextChangedEventArgs e)
        {
            UpdateGrid();
        }

        private void PageLoaded(object sender, RoutedEventArgs e)
        {
            UpdateGrid();
        }
    }
}
