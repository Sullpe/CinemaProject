using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cinema.Database.Models
{
    [Table("Session")]

    public class Session
    {
        [Key]
        public int Id { get; set; }

        public Movie Movie { get; set; }

        public Customer Customer { get; set; }

        public Hall Hall { get; set; }

        public DateTime SaleDate { get; set; } = DateTime.Now;
    }
}