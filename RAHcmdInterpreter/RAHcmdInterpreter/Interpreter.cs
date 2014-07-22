using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.Xml.Linq;
using Parse;

namespace RAHcmdInterpreter
{
    public enum InterpreterAction
    {
        GraphParse,
        RawParse,
        CloseTab
    }

    class Interpreter
    {
        String output;
        ParseManager pm;
        InterpreterAction IntAction;
        String data;

        public Interpreter()
        {
            String appId = "k9UH8HfBh2kObyiosc1Pu99Sf3L3zEk7mgGLvo3S";
            String dotNetKey = "4dHeFdRaG4lsjI24xAJtO98rR96KvjSOcTyRnEhO";

            ParseClient.Initialize(appId, dotNetKey);
        }

        public String Interpret(Node root)
        {
            output = "";
            pm = new ParseManager();
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
            if (n != null)
            {
                switch (n.type)
                {
                    case Token.Graph:
                        graphParseData((DataNode)n.child);
                        break;
                    case Token.Raw:
                        rawParseData((DataNode)n.child);
                        break;
                }
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
            if (type > 0)
            {
                DataNode dn = (DataNode)n;
                String data = dn.getValue();
                this.data = data;

            }
            else this.data = "";
            IntAction = InterpreterAction.CloseTab;
            output += "Closing Tabs\n";
        }

        void graphParseData(DataNode n)
    {
            pm.getData(n.getValue()).Wait();
            String xml = pm.getXml();
            data = xml;
            IntAction = InterpreterAction.GraphParse;
            output += "Graphing Parse data\n";
        }

        void rawParseData(DataNode n)
        {
            pm.getData(n.getValue()).Wait();
            String xml = pm.getXml();
            data = xml;
            IntAction = InterpreterAction.RawParse;
            output += "Writing Raw Parse data to output\n";
        }

        public InterpreterAction getAction()
        {
            return IntAction;
        }

        public String getData()
        {
            return data;
        }
    }
}
