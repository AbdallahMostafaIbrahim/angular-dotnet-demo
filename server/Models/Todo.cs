using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace TodoApi.Models
{
  public class Todo
  {
    [Key]
    public int Id { get; set; }
    public string? name { get; set; }
    public bool isComplete { get; set; }

    public int userId { get; set; }
    public User? User { get; set; }
  }
}