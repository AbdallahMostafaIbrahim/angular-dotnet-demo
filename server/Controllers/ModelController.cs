using Microsoft.AspNetCore.Mvc;
using TodoApi.Models;
using TodoApi.Services;
namespace TodoApi.Controllers;

public record ModelInput(List<string> models);

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
            var fields = _service.GetAllRelatedModelsMetadata(model);
            return Ok(new { fields });
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest(new { message = e.Message });
        }

    }

    [HttpGet("data/{model}")]
    public IActionResult GetData(string model, string? where = null, string? values = null, string? includes = null, int take = 0, int skip = 0, string? orderBy = null)
    {
        try
        {
            var data = _service.GetData(model, where: where, includes: includes, whereParams: values, take: take, skip: skip, orderBy: orderBy);
            return Ok(new { data });
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest(new { message = e.Message });
        }

    }
}
// Parse fields like FullName - done
// Nullable fields - done
// Get data
// Extension