using TodoApi.Models;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Collections;
using System.Linq.Dynamic.Core;
using TodoApi.Helpers;
using System.Text;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace TodoApi.Services
{
  public class Data
  {
    public List<object> data { get; set; } = new List<object>();
    public int count { get; set; }
  }
  public class Field
  {
    public string name { get; set; } = string.Empty;
    public string? displayName { get; set; }
    public string type { get; set; } = string.Empty;
    public bool? nullable { get; set; }
    public string? foreignModel { get; set; }
  }
  public class Navigation
  {
    public string name { get; set; } = string.Empty;
    public string reference { get; set; } = string.Empty;
    public string type { get; set; } = string.Empty;
    public string foreignKey { get; set; } = string.Empty;
  }
  public class ModelMetadata
  {
    public string name { get; set; } = string.Empty;
    public List<Field> fields { get; set; } = new List<Field>();

    public List<Navigation> navigations { get; set; } = new List<Navigation>();
  }
  public interface IModelService
  {
    List<string> GetModels();
    List<ModelMetadata> GetAllRelatedModelsMetadata(string model);
    Data GetData(string model, string? where = null, List<string>? whereParams = null, List<string>? includes = null, int take = 0, int skip = 0, string? orderBy = null);
  }
  public class ModelService : IModelService
  {
    private readonly Hashtable _types = new Hashtable() {
            {"Boolean", "boolean"},
            {"Int64", "number"},
            {"Int32", "number"},
            {"Int16", "number"},
            {"UInt64", "number"},
            {"UInt32", "number"},
            {"UInt16", "number"},
            {"IntPtr", "number"},
            {"UIntPtr", "number"},
            {"Double", "number"},
            {"Decimal", "number"},
            {"Single", "number"},
            {"Char", "string"},
            {"String", "string"},
        };
    private readonly string _modelsNamespace = "TodoApi.Models";
    private readonly TodoDBContext _context;

    public ModelService(TodoDBContext context)
    {
      _context = context;
    }

    // Lists All Distinct models
    public List<string> GetModels()
    {
      return _context.Model.GetEntityTypes()
      .Select(t => t.ShortName())
      .Distinct()
      .ToList();
    }

    // Parse Type from C# to javascript
    private string ParseType(string type)
    {
      if (_types.ContainsKey(type))
      {
        return _types[type]?.ToString()!;
      }
      return type;
    }

    // Make Name More readable by addding spaces and capitalizing letters
    private string? ParseName(string name)
    {
      StringBuilder builder = new StringBuilder();
      for (int i = 0; i < name.Length; i++)
      {
        if ((Char.IsUpper(name[i])) && builder.Length > 0 && (!Char.IsUpper(name[i - 1]))) builder.Append(' ');
        if (Char.Equals(name[i], '_'))
        {
          builder.Append(' ');
          Console.WriteLine(i);

          // if(name.Length < i + 1){
          builder.Append(Char.ToUpper(name[i + 1]));
          i += 2;
          // }
        }
        builder.Append(name[i]);
      }
      name = builder.ToString();

      if (name.Length > 1)
        name = Char.ToUpper(name[0]) + name.Substring(1);

      return name;
    }

    // Gets all unique Models that are somehow linked to the given model
    private HashSet<string> GetRelatedNavigations(IEntityType model, HashSet<string>? navigations = null)
    {
      if (navigations == null)
        navigations = new HashSet<string>();
      foreach (var navigation in model.GetNavigations())
      {
        var exists = navigations?.Where(n => n == navigation.TargetEntityType.ShortName()).FirstOrDefault();
        navigations?.Add(navigation.TargetEntityType.ShortName());
        if (exists == null)
        {
          navigations?.UnionWith(
            GetRelatedNavigations(navigation.TargetEntityType, navigations)
          );
        }
      }
      return navigations;
    }

    // Fetch Fields for Given Model
    private ModelMetadata GetModelMetadata(IEntityType model)
    {
      List<Field> fields = new List<Field>();

      foreach (var property in model.GetProperties()!)
      {
        var field = new Field { name = property.Name, displayName = ParseName(property.Name), type = ParseType(property.GetTypeMapping().ClrType.Name) };
        if (property.ClrType.Name.Contains("Nullable"))
        {
          field.nullable = true;
        }
        if (property.IsForeignKey())
        {
          var fk = property.GetContainingForeignKeys().ToList().First();
          if (fk == null) continue;
          var principalModel = fk.PrincipalEntityType.ShortName();
          field.foreignModel = principalModel;
        }

        fields.Add(field);
      };

      List<Navigation> navigations = new List<Navigation>();
      foreach (var navigation in model.GetNavigations())
      { 
        var nav = new Navigation { name = navigation.Name, reference = navigation.TargetEntityType.ShortName(), type = navigation.IsCollection ? "Collection" : "Reference", foreignKey = navigation.ForeignKey.Properties.ToList()[0].Name };
        navigations.Add(nav);
      }

      return new ModelMetadata { name = model.ShortName(), fields = fields, navigations = navigations, };
    }

    // Fetches Fields for the given model and all related models
    public List<ModelMetadata> GetAllRelatedModelsMetadata(string model)
    {
      if (!model.StartsWith(_modelsNamespace)) model = _modelsNamespace + "." + model;
      var type = _context.Model.FindEntityType(model);
      if (type == null)
      {
        throw new Exception("Model Not Found");
      }

      var result = new List<ModelMetadata>();
      var navigations = new HashSet<string>();
      navigations.Add(type.ShortName());
      navigations.UnionWith(GetRelatedNavigations(type));

      foreach (var navigation in navigations)
      {
        var metadata = GetModelMetadata(_context.Model.FindEntityType(_modelsNamespace + "." + navigation)!);
        result.Add(metadata);
      }

      return result;
    }

    private string GenerateSelectStatement(List<string> includes, IEntityType model, string nSpace = "")
    {
      var navigations = new HashSet<string>();
      var fields = new HashSet<string>();
      for (var i = 0; i < includes.Count; i++)
      {
        if (includes[i].Contains('.'))
        {
          navigations.Add(includes[i].Split('.').First());
        }
        else
          fields.Add(includes[i]);
      }

      string select = "new { ";
      foreach (var field in fields)
      {
        select += nSpace + field + ", ";
      }
      foreach (var nav in navigations)
      {
        var isCollection = model?.GetNavigations().Where(n => n.Name == nav).FirstOrDefault()?.IsCollection;
        if (isCollection == true)
        {
          // throw new Exception("Collection Navigation not supported");
          select += $"{nSpace}{nav}.Select(";
        }
        var newModel = model?.GetNavigations().Where(n => n.Name == nav).FirstOrDefault()?.TargetEntityType!;
        select += GenerateSelectStatement(
          includes.Where((i) => i.StartsWith(nav)).Select((i) => string.Join('.', i.Split('.').Skip(1))).ToList(),
          newModel,
          isCollection == true ? "" : $"{nSpace}{nav}."
        );
        if (isCollection == true)
        {
          select += ")";
        }
        select += " as " + nav + ", ";
      }
      select = select.Substring(0, select.Length - 2);
      select += " }";
      return select;
    }

    public Data GetData(string model, string? where = null, List<string>? whereParams = null, List<string>? includes = null, int take = 0, int skip = 0, string? orderBy = null)
    {
      if (!model.StartsWith(_modelsNamespace)) model = _modelsNamespace + "." + model;
      var type = Assembly.GetExecutingAssembly().GetTypes().FirstOrDefault((t) => t.FullName == model);
      var entityType = _context.Model.FindEntityType(model);
      var typeDisplayNametype = type?.ShortDisplayName();
      if (type == null || entityType == null)
      {
        throw new Exception("Model Not Found");
      }

      var setMethod = _context.GetType().GetMethods().Where(m => m.Name == "Set").FirstOrDefault()?.MakeGenericMethod(type)!;

      var set = ((IQueryable<object>)setMethod.Invoke(_context, null)!);
      var countQuery = set;


      if (!string.IsNullOrEmpty(where))
      {
        set = set.Where((where), whereParams?.ToArray());
        countQuery.Where((where), whereParams?.ToArray());
      }

      if (!string.IsNullOrEmpty(orderBy))
        set = set.OrderBy(orderBy);



      if (includes != null && includes.Count > 0)
      {
        var select = GenerateSelectStatement(includes, entityType);
        Console.WriteLine(select);
        set = (IQueryable<object>)set.Select(select);
      }

      if (skip > 0)
        set = set.Skip(skip);
      if (take > 0)
        set = set.Take(take);


      return new Data { data = set.ToDynamicList(), count = countQuery.Count() };
    }
  }
}

// new { totalPrice }
// URL: http://localhost:4000/api/Model/data/Cart
// Request Body
// {
//     "where": "User.email.contains(@0)",
//     "values": ["gmail.com"],
//     "orderBy": "User.email desc",
//     "skip": 0,
//     "take": 3,
//     "includes": ["User.username", "User.email", "userId", "totalPrice", "CartItems.Product.Category.description", "CartItems.Product.name", "CartItems.Product.description"]
// }
// Test Request to get data