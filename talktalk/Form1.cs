﻿using System;
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
        private Timer timer;
        private string[] lastTenData;
        private int currentIndex;

        public Form1()
        {
            InitializeComponent();
            InitializeChart();
            InitializeTimer();
        }

        private void InitializeChart()
        {
            ChartArea chartArea = new ChartArea();
            chart1.ChartAreas.Add(chartArea);

            chart1.Legends.Clear();
            chartArea.Position = new ElementPosition(0, 0, 100, 100);
        }

        private void InitializeTimer()
        {
            timer = new Timer();
            timer.Interval = 1000; // 1 seconds
            timer.Tick += new EventHandler(OnTimerTick);
        }

        private void guna2Button4_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            guna2ShadowForm1.SetShadowForm(this);

            String[] samsung = { "01", "삼성전자", "78,300", "up", "0.03%" };
            ListViewItem newitem = new ListViewItem(samsung);
            listView1.Items.Add(newitem);
            String[] naver = { "02", "NAVER", "172,700", "down", "-1.4%" };
            listView1.Items.Add(new ListViewItem(naver));
            String[] hyundai = { "03", "현대차", "265,000", "down", "-0.1%" };
            listView1.Items.Add(new ListViewItem(hyundai));
            String[] celltrion = { "04", "셀트리온", "179,000", "down", "-1.4%" };
            listView1.Items.Add(new ListViewItem(celltrion));
            String[] kb = { "05", "KB금융그룹", "79,300", "down", "-1.4%" };
            listView1.Items.Add(new ListViewItem(kb));
            String[] krafton = { "06", "크래프톤", "247,500", "down", "-1.4%" };
            listView1.Items.Add(new ListViewItem(krafton));
            String[] lgenergysolution = { "07", "LG에너지솔루션", "333,200", "down", "-1.4%" };
            listView1.Items.Add(new ListViewItem(lgenergysolution));
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

            for (int i = 1; i < csvLines.Length - 10; i++)
            {
                string[] data = csvLines[i].Split(',');
                string date = data[0];
                double close = double.Parse(data[1]);

                series.Points.AddXY(date, close);
            }

            lastTenData = csvLines.Skip(csvLines.Length - 10).ToArray();
            currentIndex = 0;

            chart1.Series.Add(series);
            timer.Start();
        }

        private void OnTimerTick(object sender, EventArgs e)
        {
            if (currentIndex < lastTenData.Length)
            {
                string[] data = lastTenData[currentIndex].Split(',');
                string date = data[0];
                double close = double.Parse(data[1]);

                chart1.Series[0].Points.AddXY(date, close);
                currentIndex++;
            }
            else
            {
                timer.Stop();
            }
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

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            Account account = new Account();
            DialogResult dResult = account.ShowDialog();
        }

        private void guna2Button9_Click(object sender, EventArgs e)
        {
            Ranking ranking = new Ranking();
            DialogResult dResult = ranking.ShowDialog();
        }
    }
}
