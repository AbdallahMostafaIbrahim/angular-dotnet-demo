using System.ComponentModel.DataAnnotations;

namespace TodoApi.Models
{
    public class Image
    {
        [Key]
        public int Id { get; set; }

        public string imageUrl { get; set; } = string.Empty;

        public string name { get; set; } = string.Empty;
    }
}