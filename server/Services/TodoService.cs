using TodoApi.Models;
using TodoApi.Controllers;

namespace TodoApi.Services
{
  public interface ITodoService
  {
    List<Todo> GetTodos();
    Todo GetTodo(int id);
    Todo? AddTodo(TodoInput todo);
    void DeleteTodo(int id);
    void ToggleTodo(int id);

  }
  public class TodoService : ITodoService
  {
    private readonly TodoDBContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;


    public TodoService(TodoDBContext context, IHttpContextAccessor httpContextAccessor)
    {
      _context = context;
      _httpContextAccessor = httpContextAccessor;
    }

    public List<Todo> GetTodos()
    {
      var user = ((User)_httpContextAccessor.HttpContext?.Items?["User"]!)!;
      var todos = _context.TodoItems.Where(t => t.userId == user.Id).Select(t => new Todo { Id = t.Id, name = t.name, isComplete = t.isComplete }).ToList();
      return todos;
    }
    public Todo GetTodo(int id)
    {
      var user = ((User)_httpContextAccessor.HttpContext?.Items?["User"]!)!;
      var todo = _context.TodoItems.Find(id);
      if (todo == null || todo.userId != user.Id)
      {
        throw new Exception("Todo not found");
      }
      return todo;
    }

    public Todo? AddTodo(TodoInput todo)
    {
      var newTodo = new Todo();
      newTodo.name = todo.name;
      newTodo.isComplete = todo.isComplete;
      newTodo.userId = ((User)_httpContextAccessor.HttpContext?.Items?["User"]!).Id!;
      _context.TodoItems.Add(newTodo);
      _context.SaveChanges();
      return newTodo;
    }

    public void DeleteTodo(int id)
    {
      var user = ((User)_httpContextAccessor.HttpContext?.Items?["User"]!)!;
      var todo = _context.TodoItems.Find(id);
      if (todo == null || todo.userId != user.Id)
      {
        throw new Exception("Todo not found");
      }
      _context.TodoItems.Remove(todo);
      _context.SaveChanges();
    }

    public void ToggleTodo(int id)
    {
      var user = ((User)_httpContextAccessor.HttpContext?.Items?["User"]!)!;
      var todo = _context.TodoItems.Find(id);
      if (todo == null || todo.userId != user.Id)
      {
        throw new Exception("Todo not found");
      }
      todo.isComplete = !todo.isComplete;
      _context.SaveChanges();
    }
  }
}