using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace TodoApi.Models
{
    public class Cart
    {
        [Key]
        public int Id { get; set; }
        public decimal totalPrice { get; set; } = 0;
        public int userId { get; set; }
        public User? User { get; set; }
        public ICollection<CartItem>? CartItems { get; set; }
    }
}