using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace RAHcmdInterpreter
{
    public delegate void CloseTabEventHandler(Object sender, CloseTabEventArgs e);
    public delegate void GraphParseDataEventHandler(Object sender, ParseEventArgs e);
    public delegate void RawParseDataEventHandler(Object sender, ParseEventArgs e);

    public class CloseTabEventArgs : EventArgs
    {
        List<int> data;
        public CloseTabEventArgs(List<int> d)
        {
            data = d;
        }

        public List<int> getData()
        {
            return data;
        }
    }

    public class ParseEventArgs : EventArgs
    {
        String data;
        public ParseEventArgs(string d)
        {
            data = d;
        }

        public String getData()
        {
            return data;
        }
    }

    class RAHCore
    {
        public event CloseTabEventHandler CloseTab;
        public event GraphParseDataEventHandler GraphParse;
        public event RawParseDataEventHandler RawParse;

        Parser p;
        Interpreter i;

        String input, output;

        readonly BackgroundWorker worker;

        public RAHCore()
        {
            p = new Parser();
            i = new Interpreter();
            worker = new BackgroundWorker();

            input = output = "";

            worker.DoWork += worker_DoWork;
            worker.RunWorkerCompleted += worker_RunWorkerCompleted;
        }

        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            p.Parse(input);
            Node root = p.getRoot();
            output = i.Interpret(root);
        }

        void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            var a = i.getAction();
            var data = i.getData();

            switch (a)
            {
                case InterpreterAction.GraphParse:
                    graphParseData(data);
                    break;
                case InterpreterAction.RawParse:
                    rawParseData(data);
                    break;
                case InterpreterAction.CloseTab:
                    closeTabs(data);
                    break;
                case InterpreterAction.BadParseInput:
                    rawParseData("Invalid Parse commands received\n" +
                                "  >> parse -i\t for a list of available commands.");
                    break;
                case InterpreterAction.BadTabsInput:
                    rawParseData("Invalid Tabs commands received\n" +
                                "  >> tabs -i\t for a list of available commands.");
                    break;
                case InterpreterAction.BadColumnInput:
                    rawParseData("Specified column does not exist. Available columns:\n" +
                                "\t>\"humidity\"\n" +
                                "\t>\"fahrenheit\"\n" +
                                "\t>\"LDR\"\n" +
                                "\t>\"waterLevel\"\n");
                    break;
                case InterpreterAction.ParseInfo:
                    rawParseData("The PARSE command is used to retrieve MonitorData from the Parse database.\n" +
                                "USAGE:  >> Parse [OPTION] [*ARGUMENTS]\n" +
                                "OPTIONS:\n" +
                                "  >> .. Graph [ column ]\t\t: Graphs values of the specified column\n" +
                                "\t\t\t\t  from the past 24 hours.\n" +
                                "  >> .. Raw [ column ]\t\t: Prints values of the specified column\n" +
                                "\t\t\t\t  to the output window.\n"
                                );
                    break;
                case InterpreterAction.TabsInfo:
                    rawParseData("The TABS command is used to manage opened tabs.\n" +
                                "USAGE:  >> Tabs [OPTION] [*ARGUMENTS]\n" +
                                "OPTIONS:\n" +
                                "  >> .. CloseAll\t\t\t: Closes all opened tabs\n" +
                                "  >> .. CloseAllBut [ x || x,y,z ]\t: All tabs will be closed except those who's\n" +
                                "\t\t\t\t  index is specified in the arguments.\n"
                                );
                    break;
            }
        }

        public void ParseInput(string input)
        {
            this.input = input;
            worker.RunWorkerAsync();
        }

        void graphParseData(String data)
        {
            if (GraphParse != null)
            {
                var e = new ParseEventArgs(data);
                GraphParse(this, e);
            }
        }

        void rawParseData(String data)
        {
            if (RawParse != null)
            {
                var e = new ParseEventArgs(data);
                RawParse(this, e);
            }
        }

        void closeTabs(String data)
        {
            if (CloseTab != null)
            {
                var indices = getIndices(data);
                var e = new CloseTabEventArgs(indices);
                CloseTab(this, e);
            }
        }

        public String getOutput()
        {
            return output;
        }

        List<int> getIndices(String data)
        {
            var input = data.ToCharArray();
            List<int> values = new List<int>();
            if (input.Length == 0) return values;

            int pos = 0;
            while (pos <= input.Length)
            {
                values.Add((int)Char.GetNumericValue(input[pos]));
                pos += 2;
            }
            return values;

        }
    }
}
