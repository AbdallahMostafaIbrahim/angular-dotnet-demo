using System.ComponentModel.DataAnnotations;


namespace TodoApi.Models
{
  public class Category
  {
    [Key]
    public int Id { get; set; }
    public string? name { get; set; }
    public string? description { get; set; }
    public int? imageId { get; set; }
    public Image? Image { get; set; }
    public ICollection<Product>? Products { get; set; }
  }
}

