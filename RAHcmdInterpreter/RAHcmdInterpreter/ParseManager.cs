using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Parse;
using System.Xml.Linq;

namespace RAHcmdInterpreter
{
    class ParseManager
    {
        String xml = "";

        public async Task getData(String column)
        {
            var query = ParseObject.GetQuery("MonitorData")
                .OrderByDescending("createdAt");

            //query.WhereEqualTo("MonitorId", "qy8Ow9Jluh"); //This id should not be hardcoded

            var monitorData = await query.FindAsync();
            var data = monitorData
                .Select(x => x.Get<float>(column))
                .ToList();

            serializeToXML(data, column);
        }

        void serializeToXML(List<float> data, String column)
        {
            var attributes = data
                .Select(x => new XElement(column, x))
                .ToArray();

            var xmlData = new XElement("ParseData", attributes);

            xml = xmlData.ToString();
        }

        public String getXml()
        {
            return xml;
        }
    }
}
