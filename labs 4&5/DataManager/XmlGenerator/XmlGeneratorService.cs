using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DataManager.XmlGenerator
{
    public class XmlGeneratorService<T> : IXmlGeneratorService<T>
    {
        public async Task GenerateXml(string path, IEnumerable<T> enumerable)
        {
            await Task.Run(() =>
           {
               var xmlSerializer = new XmlSerializer(typeof(IEnumerable<T>));
               using (var fs = new FileStream(path, FileMode.OpenOrCreate))
               {
                   xmlSerializer.Serialize(fs, enumerable);
               }
           });
        }
    }
}