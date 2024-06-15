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
using ClassLibrary;
using PacketClient;
using PacketServer;

namespace talktalk
{
    public partial class Form1 : Form
    {
        private Timer timer;
        private List<string[]> csvData;
        private int currentIndex = 0;
        private int countdown = 30;
        private decimal totalMoney = 1000000m;

        public Form1()
        {
            InitializeComponent();
            InitializeChart();
            InitializeTimer();
            InitializeCountdownLabel();
        }

        public Form1(string userID)
        {
            InitializeComponent();
            InitializeChart();
            InitializeTimer();
            InitializeCountdownLabel();
            label2.Text = userID;
            UpdateTotalMoneyLabel();
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
            timer.Interval = 1000; // 1 sec
            timer.Tick += new EventHandler(OnTimerTick);
            timer.Start();
        }

        private void InitializeCountdownLabel()
        {
            Label countdownLabel = new Label();
            countdownLabel.Name = "countdownLabel";
            countdownLabel.Text = "Next update in: 30 seconds";
            countdownLabel.AutoSize = true;
            this.Controls.Add(countdownLabel);
        }

        private void UpdateTotalMoneyLabel()
        {
            label1.Text = $"{totalMoney:C}";
        }

        private void UpdateListViewPrice(string newPrice)
        {
            if (listView1.Items.Count > 0)
            {
                ListViewItem item = listView1.Items[0];
                item.SubItems[2].Text = newPrice;
                string ud = item.SubItems[3].Text;
                if (ud == "up")
                {
                    item.SubItems[2].ForeColor = Color.Red;
                    item.SubItems[4].ForeColor = Color.Red;
                }
                else if (ud == "down")
                {
                    item.SubItems[2].ForeColor = Color.Blue;
                    item.SubItems[4].ForeColor = Color.Blue;
                }
                else
                {
                    item.SubItems[2].ForeColor = Color.Black;
                    item.SubItems[4].ForeColor = Color.Black;
                }
            }
        }

        private void guna2Button4_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            guna2ShadowForm1.SetShadowForm(this);

            String[] samsung = { "01", "SAMSUNG", "78,300", "up", "0.03%" };
            ListViewItem newitem = new ListViewItem(samsung);
            listView1.Items.Add(newitem);

            string filePath = @"C:\Users\DONGHO\Desktop\#TT\TEAM\samsung.csv";
            LoadListViewFromCsv(filePath);
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
                else if (ud == "down")
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

        private void LoadListViewFromCsv(string filePath)
        {
            if (File.Exists(filePath))
            {
                string[] lines = File.ReadAllLines(filePath);
                csvData = lines.Skip(1).Select(line => line.Split(',')).ToList();
                PlotData(lines);
            }
            else
            {
                MessageBox.Show("CSV file not found.");
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

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            PacketClient.Client clientForm = new PacketClient.Client(label2.Text);
            PacketServer.Server serverForm = new PacketServer.Server();
            clientForm.Show();
            serverForm.Show();
        }

        private string currentItemName;

        private void guna2Button10_Click(object sender, EventArgs e)
        {
            int quantityToBuy = int.Parse(guna2TextBox2.Text);
            string filePath = "admin.csv";
            if (!File.Exists(filePath))
            {
                using (StreamWriter sw = File.CreateText(filePath))
                {
                    sw.WriteLine("ItemName,Quantity");
                }
            }

            if(listView1.SelectedItems.Count == 1)
            {
                ListViewItem item = listView1.SelectedItems[0];
                decimal price = decimal.Parse(item.SubItems[2].Text.Replace(",", ""));
                UpdateCsvFile(filePath, currentItemName, quantityToBuy, price);
                totalMoney -= price * quantityToBuy;
                UpdateTotalMoneyLabel();
            }
        }
        private void UpdateCsvFile(string filePath, string itemName, int quantity, decimal price)
        {
            var lines = new List<string>(File.ReadAllLines(filePath));
            bool itemExists = false;

            for (int i = 1; i < lines.Count; i++)
            {
                string[] fields = lines[i].Split(',');
                if (fields[0] == itemName)
                {
                    int currentQuantity = int.Parse(fields[1]);
                    decimal currentTotalCost = decimal.Parse(fields[2]);
                    int newQuantity = currentQuantity + quantity;
                    decimal newTotalCost = currentTotalCost + (price * quantity);
                    decimal averagePrice = newTotalCost / newQuantity;

                    lines[i] = $"{itemName},{newQuantity},{newTotalCost}";
                    itemExists = true;
                    break;
                }
            }

            if (!itemExists)
            {
                decimal totalCost = price * quantity;
                lines.Add($"{itemName},{quantity},{totalCost}");
            }

            File.WriteAllLines(filePath, lines);
        }

        private void guna2Button11_Click(object sender, EventArgs e)
        {
            int quantityToSell = int.Parse(guna2TextBox2.Text);
            string filePath = "admin.csv";
            if (File.Exists(filePath))
            {
                
                if (listView1.SelectedItems.Count == 1)
                {
                    ListViewItem item = listView1.SelectedItems[0];
                    decimal price = decimal.Parse(item.SubItems[2].Text.Replace(",", ""));
                    SellItem(filePath, currentItemName, quantityToSell, price);
                    totalMoney += price * quantityToSell;
                    UpdateTotalMoneyLabel();
                }
            }
            else
            {
                MessageBox.Show("No inventory file found.");
            }
        }
        private int GetCurrentQuantity(string itemName)
        {
            string filePath = "admin.csv";
            if (File.Exists(filePath))
            {
                var lines = File.ReadAllLines(filePath);
                foreach (string line in lines)
                {
                    string[] fields = line.Split(',');
                    if (fields[0] == itemName)
                    {
                        return int.Parse(fields[1]);
                    }
                }
            }
            return 0;
        }
        private void SellItem(string filePath, string itemName, int quantityToSell, decimal price)
        {
            List<string> lines = new List<string>(File.ReadAllLines(filePath));
            bool itemUpdated = false;

            for (int i = 1; i < lines.Count; i++)
            {
                string[] fields = lines[i].Split(',');
                if (fields[0] == itemName)
                {
                    int currentQuantity = int.Parse(fields[1]);
                    int newQuantity = currentQuantity - quantityToSell;

                    if (newQuantity > 0)
                    {
                        decimal currentTotalCost = decimal.Parse(fields[2]);
                        decimal averagePrice = currentTotalCost / currentQuantity;
                        decimal newTotalCost = averagePrice * newQuantity;

                        lines[i] = $"{itemName},{newQuantity},{newTotalCost}";
                    }
                    else
                    {
                        lines.RemoveAt(i);
                    }
                    itemUpdated = true;
                    break;
                }
            }

            if (!itemUpdated)
            {
                MessageBox.Show("Item not found in inventory.");
            }
            else
            {
                File.WriteAllLines(filePath, lines);
            }
        }

        private void guna2Button5_Click(object sender, EventArgs e)
        {
            News news = new News();
            DialogResult dResult = news.ShowDialog();

            if (dResult == DialogResult.OK && news.IsSuccess)
            {
                MessageBox.Show("News operation was successful.");
            }
            else
            {
                MessageBox.Show("News operation failed.");
            }
        }

        private void listView1_MouseClick(object sender, MouseEventArgs e)
        {
            if (listView1.SelectedItems.Count == 1)
            {
                ListViewItem item = listView1.SelectedItems[0];
                currentItemName = item.SubItems[1].Text;
                guna2TextBox2.Text = GetCurrentQuantity(currentItemName).ToString();
            }
        }

        private void OnTimerTick(object sender, EventArgs e)
        {
            countdown--;

            Label countdownLabel = this.Controls.Find("countdownLabel", true).FirstOrDefault() as Label;
            if (countdownLabel != null)
            {
                countdownLabel.Text = $"Next update in: {countdown} seconds";
            }

            if (countdown == 0)
            {
                countdown = 30;
                if (csvData != null && csvData.Count > 0)
                {
                    currentIndex = (currentIndex + 1) % csvData.Count;
                    UpdateListViewPrice(csvData[currentIndex][1]);
                }
                Account accountForm = Application.OpenForms.OfType<Account>().FirstOrDefault();
                if (accountForm != null)
                {
                    accountForm.Close();
                    accountForm.Dispose();
                    accountForm = new Account();
                    accountForm.Show();
                }
                string filePath = "앞에채우기\\admin.csv";
                PacketClient.Client clientForm = (PacketClient.Client)Application.OpenForms["Client"];
                if (clientForm != null)
                {
                    //clientForm.SendFile(filePath);
                }
            }
        }
    }
}
