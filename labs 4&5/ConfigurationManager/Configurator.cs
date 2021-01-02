using System.Collections.Generic;
using System.IO;

namespace ConfigurationManager
{
    class Configurator<TOptionsModel> where TOptionsModel : class, new()
    {
        private readonly Dictionary<string, IParser<TOptionsModel>> parsersDictionary =
            new Dictionary<string, IParser<TOptionsModel>>();

        public void AddParser(IParser<TOptionsModel> parser)
        {
            parsersDictionary.Add(parser.Extension, parser);
        }

        public void RemoveParser(string extension)
        {
            parsersDictionary.Remove(extension);
        }

        public TOptionsModel GetConfiguredOptionsModel(string path)
        {
            var configureReaderWriter = GetParser(Path.GetExtension(path));
            return configureReaderWriter.Parse(path);
        }

        public void Serialize(TOptionsModel configureObject, string path)
        {
            var configureReaderWriter = GetParser(Path.GetExtension(path));
            configureReaderWriter.Serialize(configureObject, path);
        }

        private IParser<TOptionsModel> GetParser(string key)
        {
            return parsersDictionary.TryGetValue(key, out var value) ? value : throw new KeyNotFoundException();
        }
    }
}
