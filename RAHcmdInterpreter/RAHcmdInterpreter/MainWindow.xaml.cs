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
using System.Text.RegularExpressions;

namespace RAHcmdInterpreter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 

    public partial class MainWindow : Window
    {
        RAHCore core;

        public MainWindow()
        {
            InitializeComponent();
            core = new RAHCore();
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
                String result = core.ParseInput(input);
                writeToOutput(result);
            }
        }

        void InputText_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                setupNewLine();
            }
        }
    }
}
