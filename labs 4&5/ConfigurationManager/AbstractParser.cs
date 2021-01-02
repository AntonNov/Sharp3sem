using System;
using System.IO;
using System.Reflection;

namespace ConfigurationManager
{
    public abstract class AbstractParser<TOptionsModel> : IParser<TOptionsModel> where TOptionsModel : class, new()
    {
        protected string path;
        protected string document;

        public abstract string Extension { get; }
        protected abstract string GetRawPropertyByName(string propName);
        protected abstract void Serialize(TOptionsModel configureObject);

        public TOptionsModel Parse(string path)
        {
            this.path = Path.Combine(path);
            var configureObject = new TOptionsModel();
            document = ReadDocument(path);
            return ConfigureModel(configureObject);
        }

        public void Serialize(TOptionsModel configureObject, string path)
        {
            this.path = System.IO.Path.Combine(path);
            Serialize(configureObject);
        }

        private string ReadDocument(string path)
        {
            var sr = new StreamReader(path, System.Text.Encoding.Default);
            sr.Dispose();
            return sr.ReadToEnd();
        }

        private T ConfigureModel<T>(T configureObject) where T : class, new()
        {
            var configureObjectType = configureObject.GetType();
            foreach (var property in configureObjectType.GetProperties())
                if (property.PropertyType.IsPrimitive || property.PropertyType == typeof(string))
                {
                    var sourceProp = GetRawPropertyByName(GetPropertyName(property));
                    var prop = ConvertStringToType(sourceProp, property.PropertyType);
                    property.SetValue(configureObject, prop);
                }
                else
                {
                    var subType = property.PropertyType;
                    var subObj = Activator.CreateInstance(subType);
                    property.SetValue(configureObject, subObj);
                    ConfigureModel(subObj);
                }

            return configureObject;
        }

        private string GetPropertyName(PropertyInfo property)
        {
            var propName = property.Name;
            var nameAttribute = property.GetCustomAttribute<NameAttribute>();
            propName = nameAttribute?.Name ?? propName;
            return propName;
        }

        private object ConvertStringToType(string sourceProp, Type type)
        {
            return Convert.ChangeType(sourceProp, type);
        }
    }
}
