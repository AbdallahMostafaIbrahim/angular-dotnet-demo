using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc.Formatters;
using System.ComponentModel.DataAnnotations;

namespace TodoApi.Models
{
  public class Student
  {
    public int Id { get; set; }
    public string username { get; set; } = string.Empty;
    public string email { get; set; } = string.Empty;
    public int classId { get; set; }
    public Class? Class { get; set; }
  }
}