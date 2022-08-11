using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace TodoApi.Models
{
    public class CartItem
    {
        [Key]
        public int Id { get; set; }
        public int cartId { get; set; }
        public Cart? Cart { get; set; }

        public int? productId { get; set; }
        public Product? Product { get; set; }
    }
}