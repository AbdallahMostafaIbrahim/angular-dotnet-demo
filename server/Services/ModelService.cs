using TodoApi.Models;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Collections;
using Newtonsoft.Json;


namespace TodoApi.Services
{
    public class Field
    {
        public string name { get; set; } = string.Empty;
        public string type { get; set; } = string.Empty;
        public string? foreignModel { get; set; }
    }

    public class Navigation
    {
        public string name { get; set; } = string.Empty;
        public string reference { get; set; } = string.Empty;
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
        ModelMetadata GetModelMetadata(IEntityType model);
        List<ModelMetadata> GetAllRelatedModelsMetadata(string model);
        string ParseType(string type);

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

        public List<string> GetModels()
        {
            return _context.Model.GetEntityTypes()
            .Select(t => t.ShortName())
            .Distinct()
            .ToList();
        }

        public string ParseType(string type)
        {
            if (_types.ContainsKey(type))
            {
                return _types[type]?.ToString()!;
            }
            return type;
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
        public List<ModelMetadata> GetAllRelatedModelsMetadata(string model)
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
                var metdata = GetModelMetadata(_context.Model.FindEntityType(_modelsNamespace + "." + navigation)!);
                result.Add(metdata);
            }

            return result;
        }

        public ModelMetadata GetModelMetadata(IEntityType model)
        {
            List<Field> fields = new List<Field>();

            foreach (var property in model.GetProperties()!)
            {
                var field = new Field { name = property.Name, type = ParseType(property.GetTypeMapping().ClrType.Name) };

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
                var nav = new Navigation { name = navigation.Name, reference = navigation.TargetEntityType.ShortName() };
                navigations.Add(nav);
            }

            return new ModelMetadata { name = model.ShortName(), fields = fields, navigations = navigations };
        }
    }
}