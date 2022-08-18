using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc.Formatters;
using System.ComponentModel.DataAnnotations;

namespace TodoApi.Models
{
  public class User
  {
    public int Id { get; set; }
    public string username { get; set; } = string.Empty;
    public string email { get; set; } = string.Empty;

    [JsonIgnore]
    public string password { get; set; } = string.Empty;

    public ICollection<Cart>? Carts { get; set; }
  }
}