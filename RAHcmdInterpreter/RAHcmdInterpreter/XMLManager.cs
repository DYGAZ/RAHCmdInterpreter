using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Linq;

namespace RAHcmdInterpreter
{
    class XMLManager
    {
        String name = "";
        Dictionary<DateTime,float> data = new Dictionary<DateTime,float>();

        public String SerializeXML(Dictionary<DateTime,float> data, String column)
        {
            var attributes = data
                .Select(x => new XElement(column,
                    new XAttribute("Date", x.Key),
                    new XAttribute("Value", x.Value)
                    ))
                .ToArray();

            var xmlData = new XElement("ParseData", attributes);

            return xmlData.ToString();
        }

        public void DeserializeXML(String xml)
        {
            var doc = XDocument.Parse(xml);
            var n = ((XElement)((XElement)doc.DescendantNodes().First()).FirstNode).Name.ToString();
            name = n;

            var d = doc.Descendants(name)
                .ToDictionary(x => DateTime.Parse(x.Attribute("Date").Value), x => float.Parse(x.Attribute("Value").Value));
            data = d;
        }

        public String getName()
        {
            return name;
        }

        public Dictionary<DateTime,float> getData()
        {
            return data;
        }
    }
}
