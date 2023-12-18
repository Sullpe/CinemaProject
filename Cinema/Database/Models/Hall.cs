using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cinema.Database.Models
{
    [Table("Hall")]

    public class Hall
    {
        [Key]
        public int Id { get; set; }

        public int Places { get; set; }

        public string FullName => $"Зал #{Id}";
    }
}