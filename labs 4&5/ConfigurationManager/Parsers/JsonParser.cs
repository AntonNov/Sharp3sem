using System.Text.Json;
using System.IO;

namespace ConfigurationManager.Parsers
{ 
    class JsonParser<TOptionsModel> : AbstractParser<TOptionsModel> where TOptionsModel : class, new()
    {
        public override string Extension { get; } = ".json";

        protected override string GetRawPropertyByName(string propName)
        {
            var jsonElement = JsonDocument.Parse(document).RootElement;
            return jsonElement.GetProperty(propName).GetRawText().Trim('"');
        }

        protected override void Serialize(TOptionsModel configureObject)
        {
            using (var sw = new StreamWriter(path, false, System.Text.Encoding.Default))
            {
                sw.Write(JsonSerializer.Serialize(configureObject));
            }
        }
    }
}