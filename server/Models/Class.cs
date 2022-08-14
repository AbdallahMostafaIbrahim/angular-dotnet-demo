using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc.Formatters;
using System.ComponentModel.DataAnnotations;

namespace TodoApi.Models
{
  public class Class
  {
    public int Id { get; set; }
    public string name { get; set; } = string.Empty;
    public ICollection<Student>? Students { get; set; }
  }
}