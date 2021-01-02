using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataManager.XmlGenerator
{
    public interface IXmlGeneratorService<in T>
    {
        Task GenerateXml(string path, IEnumerable<T> enumerable);
    }
}