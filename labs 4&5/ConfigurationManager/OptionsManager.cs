using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using ConfigurationManager.Parsers;

namespace ConfigurationManager
{
    public class OptionsManager
    {
        public TOptionsModel GetConfiguredOptionsModel<TOptionsModel>() where TOptionsModel : class, new()
        {
            foreach (var path in Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory,
                $"{typeof(TOptionsModel).Name}.*"))
                try
                {
                    var configurator = GetConfigurator<TOptionsModel>();
                    var obj = configurator.GetConfiguredOptionsModel(path);
                    Validate(obj);
                    return obj;
                }
                catch (KeyNotFoundException)
                {
                }

            throw new KeyNotFoundException();
        }

        private Configurator<TOptionsModel> GetConfigurator<TOptionsModel>() where TOptionsModel : class, new()
        {
            var configurator = new Configurator<TOptionsModel>();
            AddParsers(configurator, GetParsers<TOptionsModel>());
            return configurator;
        }

        private List<IParser<TOptionsModel>> GetParsers<TOptionsModel>() where TOptionsModel : class, new()
        {
            var parsers = new List<IParser<TOptionsModel>>
            {
                new JsonParser<TOptionsModel>(),
                new XmlParser<TOptionsModel>()
            };
            return parsers;
        }

        private void AddParsers<TOptionsModel>(Configurator<TOptionsModel> configurator,
            IEnumerable<IParser<TOptionsModel>> parsers) where TOptionsModel : class, new()
        {
            foreach (var item in parsers)
            {
                configurator.AddParser(item);
            }
        }

        private void Validate(object obj)
        {
            var context = new ValidationContext(obj);
            Validator.ValidateObject(obj, context);
        }
    }
}