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
using System.Windows.Controls.DataVisualization.Charting;

namespace GraphView
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class LineChart : UserControl
    {
        Dictionary<DateTime, float> data;
        String name;

        public LineChart(Dictionary<DateTime,float> data, String name)
        {
            InitializeComponent();
            this.data = data;
            this.name = name;
            setYRange();
            LoadLineChartData();
        }

        private void LoadLineChartData()
        {
            GraphArea.Title = name;
            mcChart.Title = name + " over time";
            ((LineSeries)mcChart.Series[0]).ItemsSource = data;
            /*new KeyValuePair<DateTime, int>[]{
            new KeyValuePair<DateTime, int>(DateTime.Now, 100),
            new KeyValuePair<DateTime, int>(DateTime.Now.AddMonths(1), 130),
            new KeyValuePair<DateTime, int>(DateTime.Now.AddMonths(2), 150),
            new KeyValuePair<DateTime, int>(DateTime.Now.AddMonths(3), 125),
            new KeyValuePair<DateTime, int>(DateTime.Now.AddMonths(4),155) };*/
        }

        void setYRange()
        {
            var values = data.Select(x => x.Value).ToList();
            float max,min;
            max = min = values.First();

            values.ForEach(x =>
                {
                    if (x > max) max = x;
                    else if (x < min) min = x;
                });
            var difference = max - min;
            max += difference/2;
            min -= difference/2;


            YAxis.Maximum = max;
            YAxis.Minimum = min;
        }
    }
}
