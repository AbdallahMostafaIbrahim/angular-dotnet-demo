using TodoApi.Models;
using Isopoh.Cryptography.Argon2;
using Microsoft.EntityFrameworkCore.Metadata;


namespace TodoApi.Services
{
  public class Field
  {
    public string name { get; set; } = string.Empty;
    public string type { get; set; } = string.Empty;
    public string? foreignModel { get; set; }
  }

  public class ModelMetadata
  {
    public string name { get; set; } = string.Empty;
    public List<Field> fields { get; set; } = new List<Field>();
  }


  public interface IModelService
  {
    List<string> GetModels();
    ModelMetadata GetFields(IEntityType model);
    List<ModelMetadata> GetModelMetadata(string model);
  }
  public class ModelService : IModelService
  {
    private readonly string _modelsNamespace = "TodoApi.Models";

    private readonly TodoDBContext _context;

    public ModelService(TodoDBContext context)
    {
      _context = context;
    }

    public List<string> GetModels()
    {
      return _context.Model.GetEntityTypes()
      .Select(t => t.ShortName())
      .Distinct()
      .ToList();
    }

    // Gets all unique Models that are somehow linked to the given model
    public HashSet<string> GetRelatedNavigations(IEntityType model, HashSet<string>? navigations = null)
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

    // Fetches Fields for the given model and all related models
    public List<ModelMetadata> GetModelMetadata(string model)
    {
      if (!model.StartsWith(_modelsNamespace)) model = _modelsNamespace + "." + model;
      var type = _context.Model.FindEntityType(model);
      if (type == null)
      {
        throw new Exception("Model Not Found");
      }

      var result = new List<ModelMetadata>();
      var navigations = GetRelatedNavigations(type);
      navigations.Add(type.ShortName());

      foreach (var navigation in navigations)
      {
        var metdata = GetFields(_context.Model.FindEntityType(_modelsNamespace + "." + navigation)!);
        result.Add(metdata);
      }

      return result;
    }

    public ModelMetadata GetFields(IEntityType model)
    {
      List<Field> fields = new List<Field>();

      foreach (var property in model.GetProperties()!)
      {
        var field = new Field { name = property.Name, type = property.ClrType.Name };

        if (property.IsForeignKey())
        {
          var fk = property.GetContainingForeignKeys().ToList().First();
          if (fk == null) continue;
          var principalModel = fk.PrincipalEntityType.ShortName();
          field.foreignModel = principalModel;
        }

        fields.Add(field);
      };

      return new ModelMetadata { name = model.ShortName(), fields = fields };
    }
  }
}