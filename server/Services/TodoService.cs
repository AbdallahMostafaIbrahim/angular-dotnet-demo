using TodoApi.Models;
using TodoApi.Controllers;

namespace TodoApi.Services
{
  public interface ITodoService
  {
    TodoItem[] GetTodos();
    TodoItem GetTodo(int id);
    void AddTodo(TodoInput todo);
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

    public TodoItem[] GetTodos()
    {
      var user = ((User)_httpContextAccessor.HttpContext?.Items?["User"]!)!;
      var todos = _context.TodoItems.Where(t => t.userId == user.Id).ToArray();
      return todos;
    }
    public TodoItem GetTodo(int id)
    {
      var user = ((User)_httpContextAccessor.HttpContext?.Items?["User"]!)!;
      var todo = _context.TodoItems.Find(id);
      if (todo == null || todo.userId != user.Id)
      {
        throw new Exception("Todo not found");
      }
      return todo;
    }

    public void AddTodo(TodoInput todo)
    {
      var newTodo = new TodoItem();
      newTodo.name = todo.name;
      newTodo.isComplete = todo.isComplete;
      newTodo.userId = ((User)_httpContextAccessor.HttpContext?.Items?["User"]!).Id!;
      _context.TodoItems.Add(newTodo);
      _context.SaveChanges();
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