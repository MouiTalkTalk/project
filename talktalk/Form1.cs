using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace talktalk
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            InitializeChart();
        }

        private void InitializeChart()
        {
            ChartArea chartArea = new ChartArea();
            chart1.ChartAreas.Add(chartArea);

            chart1.Legends.Clear();
            chartArea.Position = new ElementPosition(0, 0, 100, 100);
        }

        private void guna2Button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            guna2ShadowForm1.SetShadowForm(this);

            String[] samsung = { "01", "삼성전자", "78,300", "up", "0.03%" };
            ListViewItem newitem = new ListViewItem(samsung);
            listView1.Items.Add(newitem);
        }

        private void lstAdress(object sender, MouseEventArgs e)
        {
            if (listView1.SelectedItems.Count == 1)
            {
                ListView.SelectedListViewItemCollection items = listView1.SelectedItems;
                ListViewItem lvItem = items[0];
                string name = lvItem.SubItems[1].Text;
                string price = lvItem.SubItems[2].Text;
                string ud = lvItem.SubItems[3].Text;
                string updown = lvItem.SubItems[4].Text;
                stockName.Text = name;
                stockPrice.Text = price;
                stockUpdown.Text = updown;
                if (ud == "up")
                {
                    stockPrice.ForeColor = Color.Red;
                    stockUpdown.ForeColor = Color.Red;
                }
                else if(ud == "down")
                {
                    stockPrice.ForeColor = Color.Blue;
                    stockUpdown.ForeColor = Color.Blue;
                }
                else
                {
                    stockPrice.ForeColor = Color.Black;
                    stockUpdown.ForeColor = Color.Black;
                }
            }
        }

        private void PlotData(string[] csvLines)
        {
            chart1.Series.Clear();
            chart1.ChartAreas.Clear();

            ChartArea chartArea = new ChartArea();
            chart1.ChartAreas.Add(chartArea);

            Series series = new Series(stockName.Text);
            series.ChartType = SeriesChartType.Line;

            series.BorderWidth = 3;

            List<double> closePrices = new List<double>();
            for (int i = 1; i < csvLines.Length; i++)
            {
                string[] data = csvLines[i].Split(',');
                double close = double.Parse(data[1]);
                closePrices.Add(close);
            }

            double minClose = closePrices.Min();
            double maxClose = closePrices.Max();

            maxClose = Math.Ceiling(maxClose / 10000) * 10000;
            minClose = Math.Floor(minClose / 10000) * 10000;

            chart1.ChartAreas[0].AxisY.Minimum = minClose;
            chart1.ChartAreas[0].AxisY.Maximum = maxClose;

            double averageInterval = (maxClose - minClose) / 5;
            chart1.ChartAreas[0].AxisY.Interval = averageInterval;

            for (int i = 1; i < csvLines.Length; i++)
            {
                string[] data = csvLines[i].Split(',');
                string date = data[0];
                double close = double.Parse(data[1]);

                series.Points.AddXY(date, close);
            }

            chart1.Series.Add(series);
        }

        private void guna2Button14_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "CSV files (*.csv)|*.csv";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string[] lines = File.ReadAllLines(openFileDialog.FileName);
                PlotData(lines);
            }
        }
    }
}
