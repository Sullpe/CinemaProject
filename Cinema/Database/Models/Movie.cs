using Cinema.Database.Models;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cinema.Database.Models
{
    [Table("Movie")]
    public class Movie
    {
        [Key]
        public int Id { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        public int Price { get; set; }

        public DateTime ShowDate { get; set; } = DateTime.Now;

        public Genre Genre { get; set; }
    }
}
