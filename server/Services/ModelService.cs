using TodoApi.Models;
using Isopoh.Cryptography.Argon2;


namespace TodoApi.Services
{
    public class Field
    {
        public string name { get; set; } = string.Empty;
        public string type { get; set; } = string.Empty;
        public bool? foreign { get; set; }
        public List<Field>? innerFields { get; set; }
    }
    public interface IModelService
    {
        List<string> GetModels();
        List<Field> GetFields(string model);
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

        public List<Field> GetFields(string model)
        {
            if (!model.StartsWith(_modelsNamespace)) model = _modelsNamespace + "." + model;
            var type = _context.Model.FindEntityType(model);
            if (type == null)
            {
                throw new Exception("Model Not Found");
            }
            var fields = new List<Field>();

            foreach (var property in type?.GetProperties()!)
            {
                var field = new Field { name = property.Name, type = property.ClrType.Name };

                if (property.IsForeignKey())
                {
                    var fk = property.GetContainingForeignKeys().ToList().First();
                    if (fk == null) continue;
                    var principalModel = fk.PrincipalEntityType.Name;
                    field.foreign = true;
                    field.innerFields = GetFields(principalModel);
                }

                fields.Add(field);
            };
            return fields;
        }

    }
}