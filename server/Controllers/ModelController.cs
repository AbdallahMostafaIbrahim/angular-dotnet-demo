using Microsoft.AspNetCore.Mvc;
using TodoApi.Models;
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
      using (var writer = new StringWriter())
      using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
      {
        csv.WriteRecords(data.data);

        var stream = new MemoryStream(Encoding.UTF8.GetBytes(writer.ToString() ?? ""));
        return File(stream, "text/csv");
      }
    }
    catch (Exception e)
    {
      Console.WriteLine(e);
      return BadRequest(new { message = e.Message });
    }
  }
}

public record ModelInput(List<string> models);
