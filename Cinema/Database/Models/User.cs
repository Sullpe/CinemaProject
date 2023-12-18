using SQLite.CodeFirst;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cinema.Database.Models
{
    [Table("User")]

    public class User
    {
        [Key]
        public int Id { get; set; }

        [StringLength(50)]
        public string FirstName { get; set; }

        [StringLength(50)]
        public string LastName { get; set; }

        public Role Role { get; set; }

        [StringLength(64)]
        public string Password { get; set; }

        [StringLength(64)]
        public string Login { get; set; }

        public string FullName { get => $"{LastName} {FirstName}"; }
    }
}
