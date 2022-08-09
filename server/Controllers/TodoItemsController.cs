using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi.Models;
using TodoApi.Services;

namespace TodoApi.Controllers
{
  public record TodoInput(string name, bool isComplete);

  [Route("api/todos")]
  [Authorize]
  [ApiController]
  public class TodoItemsController : ControllerBase
  {
    private readonly ITodoService _service;

    public TodoItemsController(ITodoService service)
    {
      _service = service;
    }

    // GET: api/TodoItems
    [HttpGet]
    public ActionResult GetTodoItems()
    {
      return Ok(new { status = 200, todos = _service.GetTodos() });
    }

    // GET: api/TodoItems/5
    [HttpGet("{id}")]
    public ActionResult GetTodoItem(int id)
    {
      return Ok(new { status = 200, todo = _service.GetTodo(id) });
    }

    [HttpPost("toggle/{id}")]
    public IActionResult ToggleTodo(int id)
    {
      _service.ToggleTodo(id);
      return Ok(new { status = 200 });
    }

    [HttpPost]
    public ActionResult<Todo> PostTodoItem(TodoInput todoItem)
    {
      var newTodo = _service.AddTodo(todoItem);
      return Ok(new { status = 200, todo = newTodo });
    }

    // DELETE: api/TodoItems/5
    [HttpDelete("{id}")]
    public IActionResult DeleteTodoItem(int id)
    {
      _service.DeleteTodo(id);
      return NoContent();
    }

    private bool TodoItemExists(int id)
    {
      if (_service.GetTodo(id) == null)
        return false;
      else
        return true;
    }
  }
}
