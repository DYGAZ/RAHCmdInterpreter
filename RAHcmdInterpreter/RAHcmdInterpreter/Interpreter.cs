using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAHcmdInterpreter
{
    public delegate void CloseTabEventHandler(Object sender, CloseTabEventArgs e);
    public delegate void GraphParseDataEventHandler(Object sender, ParseEventArgs e);
    public delegate void RawParseDataEventHandler(Object sender, ParseEventArgs e);

    public class CloseTabEventArgs : EventArgs
    {
        String data;
        public CloseTabEventArgs(String d)
        {
            data = d;
        }
    }

    public class ParseEventArgs : EventArgs
    {
        String data;
        public ParseEventArgs(string d)
        {
            data = d;
        }
    }

    class Interpreter
    {
        String output;

        public event CloseTabEventHandler CloseTab;
        public event GraphParseDataEventHandler GraphParse;
        public event RawParseDataEventHandler RawParse;

        public String Interpret(Node root)
        {
            output = "";
            operate(root.child);
            return output;
        }

        void operate(Node n)
        {
            switch (n.type)
            {
                case Token.Parse:
                    output += "Parse command received...\n";
                    parseCommand(n.child);
                    break;
                case Token.Tabs:
                    output += "Tabs command received...\n";
                    tabCommand(n.child);
                    break;
            }
        }

        void parseCommand(Node n)
        {
            switch (n.type)
            {
                case Token.Graph:
                    graphParseData(n.child);
                    break;
                case Token.Raw:
                    rawParseData(n.child);
                    break;
            }
        }

        void tabCommand(Node n)
        {
            switch (n.type)
            {
                case Token.CloseAll:
                    closeTabs(n.child, -1);
                    break;
                case Token.CloseAllBut:
                    closeTabs(n.child, 1);
                    break;
            }
        }

        public void closeTabs(Node n, int type)
        {
            if(CloseTab != null)
            {
                CloseTabEventArgs e;
                if (type > 0)
                {
                    DataNode dn = (DataNode)n.child;
                    String data = dn.getValue();
                    e = new CloseTabEventArgs(data);

                }
                else e = new CloseTabEventArgs("");
                output += "Closing Tabs\n";
                CloseTab(this, e);
            }

        }

        void graphParseData(Node n)
        {
            if (GraphParse != null)
            {
                String xml = getDataFromParse();
                ParseEventArgs e = new ParseEventArgs(xml);
                output += "Graphing Parse data\n";
                GraphParse(this, e);
            }
        }

        void rawParseData(Node n)
        {
            if (RawParse != null)
            {
                String data = getDataFromParse();
                ParseEventArgs e = new ParseEventArgs(data);
                output += "Writing Raw Parse data to output\n";
                RawParse(this, e);
            }
        }

        String getDataFromParse()
        {
            //Get the data from Parse.com Here and put it into xml
            //
            return "";
        }
    }
}
