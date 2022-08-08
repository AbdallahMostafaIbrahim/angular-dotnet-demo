using TodoApi.Models;

namespace TodoApi.Services
{
  public interface ITodoService
  {
    TodoItem[] GetTodos();
    TodoItem GetTodo(string id);
    void AddTodo(TodoItem todo);
    void DeleteTodo(string id);
    void ToggleTodo(string id);

  }
  public class TodoService : ITodoService
  {
    private readonly TodoDBContext _context;

    public TodoService(TodoDBContext context)
    {
      _context = context;
    }

    public TodoItem[] GetTodos()
    {
      var todos = _context.TodoItems.ToArray();
      return todos;
    }
    public TodoItem GetTodo(string id)
    {
      var todo = _context.TodoItems.Find(id);
      if (todo == null)
      {
        throw new Exception("Todo not found");
      }
      return todo;
    }

    public void AddTodo(TodoItem todo)
    {
      _context.TodoItems.Add(todo);
      _context.SaveChanges();
    }

    public void DeleteTodo(string id)
    {
      var todo = _context.TodoItems.Find(id);
      if (todo == null)
      {
        throw new Exception("Todo not found");
      }
      _context.TodoItems.Remove(todo);
      _context.SaveChanges();
    }

    public void ToggleTodo(string id)
    {
      var todo = _context.TodoItems.Find(id);
      if (todo == null)
      {
        throw new Exception("Todo not found");
      }
      todo.isComplete = !todo.isComplete;
      _context.SaveChanges();
    }


  }
}