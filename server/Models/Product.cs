using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace TodoApi.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(50)]
        public string name { get; set; } = string.Empty;

        [MaxLength(50)]
        public string description { get; set; } = string.Empty;

        public decimal? price { get; set; } = 0;

        public int imageId { get; set; }
        public Image? Image { get; set; }

        public int categoryId { get; set; }
        public Category? Category { get; set; }
    }
}