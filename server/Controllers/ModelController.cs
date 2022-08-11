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
    public IActionResult GetAll(string model)
    {
        try
        {
            var fields = _service.GetFields(model);

            return Ok(new { data = fields });
        }
        catch (Exception e)
        {
            return BadRequest(new { message = e.Message });
        }

    }
}