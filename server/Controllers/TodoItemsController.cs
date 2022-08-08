using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi.Models;
using TodoApi.Services;

namespace server.Controllers
{
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
        public ActionResult GetTodoItem(string id)
        {
            return Ok(new { status = 200, todo = _service.GetTodo(id) });
        }

        [HttpPost("toggle/{id}")]
        public IActionResult ToggleTodo(string id)
        {
            _service.ToggleTodo(id);
            return Ok(new { status = 200 });
        }

        [HttpPost]
        public ActionResult<TodoItem> PostTodoItem(TodoItem todoItem)
        {
            try
            {
                _service.AddTodo(todoItem);
            }
            catch (DbUpdateException)
            {
                if (TodoItemExists(todoItem.id!))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return Ok(new { status = 200, todo = todoItem });
        }

        // DELETE: api/TodoItems/5
        [HttpDelete("{id}")]
        public IActionResult DeleteTodoItem(string id)
        {
            _service.DeleteTodo(id);
            return NoContent();
        }

        private bool TodoItemExists(string id)
        {
            if (_service.GetTodo(id) == null)
                return false;
            else
                return true;
        }
    }
}
