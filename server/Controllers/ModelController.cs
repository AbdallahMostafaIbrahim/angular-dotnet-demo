using Microsoft.AspNetCore.Mvc;
using TodoApi.Services;
using CsvHelper;
using System.Globalization;
using System.Text;
namespace TodoApi.Controllers;

public class FilterInput
{
  public string? where { get; set; }
  public List<string>? values { get; set; }
  public List<string>? includes { get; set; }
  public int? take { get; set; } = 0;
  public int? skip { get; set; } = 0;
  public string? orderBy { get; set; }
}


[ApiController]
[Route("api/[controller]")]
public class ModelController : ControllerBase
{
  private readonly IModelService _service;

  public static object GetPropertyValue(object src, string propName)
  {
    if (src == null) throw new ArgumentException("Value cannot be null.", "src");
    if (propName == null) throw new ArgumentException("Value cannot be null.", "propName");

    if (propName.Contains("."))//complex type nested
    {
      var temp = propName.Split(new char[] { '.' }, 2);
      return GetPropertyValue(GetPropertyValue(src, temp[0]), temp[1]);
    }
    else
    {
      var prop = src.GetType().GetProperty(propName);
      return prop != null ? prop.GetValue(src, null) : null;
    }
  }

  public ModelController(IModelService service)
  {
    _service = service;
  }

  [HttpGet]
  public IActionResult GetModels()
  {
    return Ok(new { models = _service.GetModels() });
  }

  [HttpGet("fields/{model}")]
  public IActionResult GetAllFields(string model)
  {
    try
    {
      var data = _service.GetAllRelatedModelsMetadata(model);
      return Ok(new { data });
    }
    catch (Exception e)
    {
      Console.WriteLine(e);
      return BadRequest(new { message = e.Message });
    }

  }

  [HttpPost("data/{model}")]
  public IActionResult GetData(string model, FilterInput filter)
  {
    try
    {
      var data = _service.GetData(model, filter.where, filter.values, filter.includes, filter.take ?? 0, filter.skip ?? 0, filter.orderBy);
      return Ok(data);
    }
    catch (Exception e)
    {
      Console.WriteLine(e);
      return BadRequest(new { message = e.Message });
    }
  }

  [HttpPost("export/{model}")]
  public IActionResult ExportModel(string model, FilterInput filter)
  {
    try
    {
      var data = _service.GetData(model, filter.where, filter.values, filter.includes, filter.take ?? 0, filter.skip ?? 0, filter.orderBy);

      var records = new List<Dictionary<string, dynamic>>();

      var writer = new StringWriter();
      var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);

      foreach (var header in filter.includes!)
      {
        csv.WriteField(header);
      }
      csv.NextRecord();

      foreach (var row in data.data)
      {
        foreach (var field in filter.includes!)
        {
          var value = GetPropertyValue(row, field);
          csv.WriteField(value);
        }
        csv.NextRecord();
      }


      var stream = new MemoryStream(Encoding.UTF8.GetBytes(writer.ToString() ?? ""));

      return File(stream, "text/csv");
    }

    catch (Exception e)
    {
      Console.WriteLine(e);
      return BadRequest(new { message = e.Message });
    }
  }
}

public record ModelInput(List<string> models);
