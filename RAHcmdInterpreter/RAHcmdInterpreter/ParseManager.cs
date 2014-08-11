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
        XMLManager xmlManager = new XMLManager();

        public async Task getData(String column)
        {
            var query = ParseObject.GetQuery("MonitorData")
                .OrderByDescending("createdAt")
                .Limit(50);

            var monitorData = await query.FindAsync();
            var data = monitorData
                .ToDictionary(x => ((DateTime)x.CreatedAt).AddHours(-4), x => x.Get<float>(column));

            xml = xmlManager.SerializeXML(data, column);
        }

        public String getXml()
        {
            return xml;
        }
    }
}
