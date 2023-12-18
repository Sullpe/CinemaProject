using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Data.SQLite.EF6.Migrations;
using System.IO;
using System.Linq;
using Cinema.Database.Models;

namespace Cinema.Database
{
    public class DatabaseContext : DbContext
    {
        public static bool Preload()
        {
            try
            {
                Instance.Roles.ToList();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool Exists()
        {
            return File.Exists("Cinema.db");
        }

        public static void DeleteDB()
        {
            File.Delete("Cinema.db");
        }

        public DatabaseContext() : base("name=Cinema")
        {

            if (!File.Exists("Cinema.db"))
            {
                System.Data.Entity.Database.SetInitializer(new MigrateDatabaseToLatestVersion<DatabaseContext, MigrationConfiguration>(true));
            }
        }

        public static DatabaseContext Instance = new DatabaseContext();

        public DbSet<Movie> Movies { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<Hall> Halls { get; set; }

        public DbSet<Session> Sessions { get; set; }

        public DbSet<Genre> Genres { get; set; }

    }

    internal sealed class MigrationConfiguration : DbMigrationsConfiguration<DatabaseContext>
    {
        public MigrationConfiguration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
            SetSqlGenerator("System.Data.SQLite", new SQLiteMigrationSqlGenerator());
        }

        protected override void Seed(DatabaseContext context)
        {
            List<Genre> genres = new List<Genre>
            {
                new Genre { Name = "Драма" },
                new Genre { Name = "Боевик" },
                new Genre { Name = "Ужасы" },
            };
            List<Movie> movies = new List<Movie>
            {
                new Movie { Name = "Драйв", Price = 350, ShowDate = DateTime.Now.AddDays(3), Genre = genres[0] },
                new Movie { Name = "Джон Уик 4", Price = 400, ShowDate = DateTime.Now.AddDays(2), Genre = genres[0] },
                new Movie { Name = "Тихое место 2", Price = 265, ShowDate = DateTime.Now.AddDays(1), Genre = genres[0] }
            };

            List<Role> roles = new List<Role>
            {
                new Role { Name = "Гость", Id = 1 },
                new Role { Name = "Сотрудник", Id = 2 },
                new Role { Name = "Администратор", Id = 3 }
            };

            List<User> users = new List<User>
            {
                new User { FirstName = "Администратор", LastName = "", Role = roles[2], Login = "admin", Password = "admin"},
                new User { FirstName = "Гость", Role = roles[0], LastName = "", Login = "guest", Password = "guest"}
            };

            List<Customer> customers = new List<Customer>
            {
                new Customer { FirstName = "Антон", LastName = "Румянцев" },
                new Customer { FirstName = "Никита", LastName = "Мельников" },
                new Customer { FirstName = "Дмитрий", LastName = "Евграфов" }
            };

            List<Hall> halls = new List<Hall>
            {
                new Hall { Places = 52 },
                new Hall { Places = 44 },
                new Hall { Places = 77 }
            };

            List<Session> sessions = new List<Session>
            {
                new Session { Customer = customers[0], Hall = halls[1], Movie = movies[2], SaleDate = DateTime.Now }
            };


            context.Movies.AddRange(movies);
            context.Roles.AddRange(roles);
            context.Users.AddRange(users);
            context.Customers.AddRange(customers);
            context.Halls.AddRange(halls);
            context.Sessions.AddRange(sessions);
            context.Genres.AddRange(genres);
            context.SaveChanges();
        }
    }
}
