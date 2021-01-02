using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace ConfigurationManager.Parsers
{
    public class XmlParser<TOptionsModel> : AbstractParser<TOptionsModel> where TOptionsModel : class, new()
    {
        public override string Extension { get; } = ".xml";
        private readonly XmlDocument xmlDocument;
        public XmlParser()
        {
            xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(document);

            var schema = new XmlSchemaSet();
            schema.Add("",
                Path.Combine(Path.GetDirectoryName(path),
                    Path.GetFileNameWithoutExtension(path) + ".xsd"));

            xmlDocument.Schemas = schema;
        }

        protected override string GetRawPropertyByName(string propName)
        {
            var node = GetNodeOrNull(propName, xmlDocument.DocumentElement) ?? throw new KeyNotFoundException();
            xmlDocument.Validate(ValidationEventHandler, node);

            return node.InnerText;
        }

        private XmlNode GetNodeOrNull(string name, XmlNode xmlNode)
        {
            foreach (XmlNode node in xmlNode)
                if (node.Name == name)
                {
                    return node;
                }
                else
                {
                    var xmlNode1 = GetNodeOrNull(name, node);

                    if (xmlNode1 != null)
                        return xmlNode1;
                }

            return null;
        }

        protected override void Serialize(TOptionsModel configureObject)
        {
            using (var sw = new StreamWriter(path, false, Encoding.Default))
            {
                var xmlSerializer = new XmlSerializer(typeof(TOptionsModel));
                xmlSerializer.Serialize(sw, configureObject);
            }
        }

        private void ValidationEventHandler(object sender, ValidationEventArgs e)
        {
            if (!Enum.TryParse("Error", out XmlSeverityType type))
                return;
            if (type == XmlSeverityType.Error)
                throw new XmlSchemaValidationException(e.Message);
        }
    }
}