using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Parse;

namespace RAHcmdInterpreter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 

    public partial class MainWindow : Window
    {
        RAHCore core;
        XMLManager xmlManager = new XMLManager();

        public MainWindow()
        {
            this.InitializeComponent();

            core = new RAHCore();
            core.CloseTab += closeTabs;
            core.GraphParse += graphParse;
            core.RawParse += rawParse;
            
            closeAllTabs();
            setupNewLine();
        }

        public void setupNewLine()
        {
            InputText.AppendText("RAH >> ");
        }

        String getInputLine()
        {
            TextRange tRange = new TextRange(InputText.Document.ContentEnd.GetLineStartPosition(0), InputText.Document.ContentEnd);
            String command = tRange.Text;
            return command;
        }

        void writeToOutput(String message)
        {
            OutputText.AppendText(message);
        }

        void InputText_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                String input = getInputLine().Substring(7);
                input = input.Substring(0, input.Length - 2);
                core.ParseInput(input);
            }
        }

        void InputText_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                setupNewLine();
            }
        }

        void closeTabs(Object sender, CloseTabEventArgs e)
        {
            writeToOutput(core.getOutput());
            List<int> d = e.getData();
            if (d.Count > 0)
            {
                String values = "";
                d.ForEach(x => values += x + ", ");
                writeToOutput("Keeping tabs: " + values + " open and closing all others");
                var tabitems = Tabs.Items.Cast<TabItem>()
                    .ToList();

                int index = 1;
                tabitems.ForEach(x => 
                    {
                        if (!d.Contains(index)) Tabs.Items.Remove(x);
                        index++;
                    });
            }
            else closeAllTabs();
        }

        void graphParse(Object sender, ParseEventArgs e)
        {
            writeToOutput(core.getOutput());
            xmlManager.DeserializeXML(e.getData());
            var name = xmlManager.getName();
            var data = xmlManager.getData();
            newTab(name, data);
        }

        void rawParse(Object sender, ParseEventArgs e)
        {
            writeToOutput(core.getOutput());
            writeToOutput(e.getData() + "\n");
        }

        void closeAllTabs()
        {
            var tabitems = Tabs.Items.Cast<TabItem>()
                .ToList();

            tabitems.ForEach(x => Tabs.Items.Remove(x));
        }

        private void newTab(String header, Dictionary<DateTime,float> data)
        {
            var tab = new TabItem();
            tab.Header = header;
            tab.Content = new GraphView.LineChart(data, header);
            Tabs.Items.Add(tab);
            Tabs.SelectedIndex = Tabs.Items.Count - 1;
        }
    }
}
