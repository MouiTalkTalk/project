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
using Game1;
using game2;
using System.Globalization;
using System.Net;
using System.Net.Sockets;


namespace talktalk
{
    public partial class Form1 : Form
    {
        DateTime currentDate = new DateTime(2024, 6, 4);

        private Timer timer;
        private List<string[]> csvData;
        private int currentIndex = 60;
        private int countdown = 15;
        private int totalMoney = 1000000;
        private string dataDirectory;
        private string[] lastTenData;
        private int currentPlotIndex;
        private int dayCount = 1;
        private bool GameCount = true;

        private TcpClient client = new TcpClient();
        private StreamWriter streamWrite = null;


        public Form1()
        {
            InitializeComponent();
            InitializeChart();
            InitializeTimer();
            InitializeCountdownLabel();
            SetDataDirectory();
            InitializeDayCountLabel();
            UserTotalAsset();
        }

        public Form1(string userID)
        {
            InitializeComponent();
            InitializeChart();
            InitializeTimer();
            InitializeCountdownLabel();
            label2.Text = userID;
            label3.Text = userID;
            UpdateTotalMoneyLabel();
            SetDataDirectory();
            InitializeDayCountLabel();
            UserTotalAsset();
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

        private void InitializeDayCountLabel()
        {
            Label label15 = new Label();
            label15.Name = "label15";
            label15.Text = "DAY1";
            label15.AutoSize = true;
            this.Controls.Add(label15);
        }

        private void SetDataDirectory()
        {
            DirectoryInfo currentDir = new DirectoryInfo(Application.StartupPath);

            DirectoryInfo dataDir = currentDir.Parent.Parent.Parent;

            dataDirectory = Path.Combine(dataDir.FullName, "data");
        }

        private void UpdateTotalMoneyLabel()
        {
            label1.Text = $"{totalMoney:N0}";
        }

        private void UserTotalAsset() 
        {
            int money = totalMoney;
            string username = label2.Text;
            string filename = username + ".csv";
            string filePath = Path.Combine(dataDirectory, filename);
            if (File.Exists(filePath))
            {
                var lines = File.ReadAllLines(filePath);
                foreach (string line in lines)
                {
                    string[] fields = line.Split(',');
                    foreach (ListViewItem item in listView1.Items)
                    {
                        if (fields[0] == item.SubItems[1].Text)
                        {
                            int asset = int.Parse(item.SubItems[2].Text.Replace(",", ""));
                            int totalast = asset * int.Parse(fields[1]);
                            money += totalast;
                        }
                    }
                }
            }
            label6.Text = money.ToString();
        }


        private void UpdateListViewPrice(ListViewItem item, string oldData, string newPrice)
        {
            if (listView1.Items.Count > 0)
            {
                //ListViewItem item = listView1.Items[0];
                item.SubItems[2].Text = newPrice;

                if (decimal.TryParse(oldData, out decimal oldPrice) && decimal.TryParse(newPrice, out decimal newPriceDecimal))
                {
                    decimal changeRate = (newPriceDecimal - oldPrice) / oldPrice * 100;
                    item.SubItems[4].Text = $"{changeRate:F2}%";

                    if (newPriceDecimal > oldPrice)
                    {
                        item.SubItems[3].Text = "up";
                    }
                    else if (newPriceDecimal < oldPrice)
                    {
                        item.SubItems[3].Text = "down";
                    }
                    else
                    {
                        item.SubItems[3].Text = "same";
                    }
                }


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

            var stocks = new string[] { "celltrion", "hyundai", "kbbank", "krafton", "lgenergysolution", "naver", "samsung" };
            foreach (var stock in stocks)
            {
                string filePath2 = GetCsvFilePath(stock);
                ClearCsvFile(filePath2);
            }

            String[] samsung = { "01", "SAMSUNG", "73500", "same", "0.00%" };
            ListViewItem newitem1 = new ListViewItem(samsung);
            listView1.Items.Add(newitem1);

            String[] naver = { "02", "NAVER", "170200", "down", "-0.10%" };
            listView1.Items.Add(new ListViewItem(naver));

            String[] hyundai = { "03", "Hyundai", "253000", "down", "-1.56%" };
            listView1.Items.Add(new ListViewItem(hyundai));

            String[] celltrion = { "04", "Celltrion", "176200", "down", "-0.62%" };
            listView1.Items.Add(new ListViewItem(celltrion));

            String[] kb = { "05", "KBBank", "79400", "up", "0.89%" };
            listView1.Items.Add(new ListViewItem(kb));

            String[] krafton = { "06", "Krafton", "250000", "down", "-0.40%" };
            listView1.Items.Add(new ListViewItem(krafton));

            String[] lgenergysolution = { "07", "LGEnergySolution", "331000", "up", "0.12%" };
            listView1.Items.Add(new ListViewItem(lgenergysolution));

            string filePath = Path.Combine(dataDirectory, "samsung.csv");
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

            //for (int i = 1; i < csvLines.Length - 10; i++)
            for (int i = 1; i < currentIndex + 2; i++)
            {
                string[] data = csvLines[i].Split(',');
                string date = data[0];
                double close = double.Parse(data[1]);

                series.Points.AddXY(date, close);
            }

            //lastTenData = csvLines.Skip(csvLines.Length - 10).ToArray();
            lastTenData = csvLines.Skip(currentIndex + 2).ToArray();
            currentPlotIndex = 0;

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
            Account account = new Account(label2.Text);
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
            clientForm.Show();
        }

        private string currentItemName;

        private void guna2Button10_Click(object sender, EventArgs e)
        {
            int quantityToBuy = int.Parse(guna2TextBox2.Text);
            string username = label2.Text;
            string filename = username + ".csv";
            string filePath = Path.Combine(dataDirectory, filename);
            if (!File.Exists(filePath))
            {
                using (StreamWriter sw = File.CreateText(filePath))
                {
                    sw.WriteLine("ItemName,Quantity, TotalCost");
                }
            }

            if(listView1.SelectedItems.Count == 1)
            {
                ListViewItem item = listView1.SelectedItems[0];
                decimal price = decimal.Parse(item.SubItems[2].Text.Replace(",", ""));
                totalMoney -= (int)(price * quantityToBuy);

                if (totalMoney < 0)
                {
                    totalMoney += (int)(price * quantityToBuy);
                    MessageBox.Show("YOU CANNOT BUY.");
                    return;
                }
                UpdateCsvFile(filePath, currentItemName, quantityToBuy, price);

                UpdateTotalMoneyLabel();
            }
            UserTotalAsset();
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
            string username = label2.Text;
            string filename = username + ".csv";
            string filePath = Path.Combine(dataDirectory, filename);
            if (File.Exists(filePath))
            {
                
                if (listView1.SelectedItems.Count == 1)
                {
                    ListViewItem item = listView1.SelectedItems[0];
                    decimal price = decimal.Parse(item.SubItems[2].Text.Replace(",", ""));
                    SellItem(filePath, currentItemName, quantityToSell, price);
                    /*totalMoney += (int)(price * quantityToSell);
                    UpdateTotalMoneyLabel();*/
                }
            }
            else
            {
                MessageBox.Show("No inventory file found.");
            }

            UserTotalAsset();
        }
        private int GetCurrentQuantity(string itemName)
        {
            string username = label2.Text;
            string filename = username + ".csv";
            string filePath = Path.Combine(dataDirectory, filename);
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
                    totalMoney += (int)(price * quantityToSell);
                    UpdateTotalMoneyLabel();
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
            if (GameCount)
            {
                timer.Stop();
                News news = new News();
                DialogResult dResult = news.ShowDialog();

                if (dResult == DialogResult.OK && news.IsSuccess)
                {
                    MessageBox.Show("동전 예측에 성공했습니다.");

                    if (listView1.SelectedItems.Count == 1)
                    {
                        ListView.SelectedListViewItemCollection items = listView1.SelectedItems;
                        ListViewItem lvItem = items[0];
                        string name = lvItem.SubItems[1].Text;
                        string price = lvItem.SubItems[2].Text;
                        string filePath2 = GetCsvFilePath(name);
                        if (File.Exists(filePath2))
                        {
                            string[] lines = File.ReadAllLines(filePath2);
                            csvData = lines.Skip(1).Select(line => line.Split(',')).ToList();
                            if (csvData != null && csvData.Count > 0)
                            {
                                string NowData = csvData[currentIndex][1];
                                int NowDataN = int.Parse(NowData);
                                int FutDataN = int.Parse(csvData[currentIndex + 1][1]);
                                if (NowDataN < FutDataN)
                                {
                                    MessageBox.Show(name + " 주식은 다음 턴에 좋은 일이 있어 보입니다..");
                                }
                                else
                                {
                                    MessageBox.Show(name + " 주식은 다음 턴에 나쁜 일이 있어 보입니다..");
                                }
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("동전 예측에 실패했습니다.");
                }

                GameCount = false;
                timer.Start();
            }
            else
            {
                MessageBox.Show("이번 라운드에 게임 횟수를 초과했습니다!");
            }
 
        }

        private string GetCsvFilePath(string itemName)
        {
            string fileName = $"{itemName.ToLower()}.csv";
            return Path.Combine(dataDirectory, fileName);
        }



        private void listView1_MouseClick(object sender, MouseEventArgs e)
        {
            if (listView1.SelectedItems.Count == 1)
            {
                ListViewItem item = listView1.SelectedItems[0];
                currentItemName = item.SubItems[1].Text;
                guna2TextBox2.Text = GetCurrentQuantity(currentItemName).ToString();

                string filePath = GetCsvFilePath(currentItemName);
                if (File.Exists(filePath))
                {
                    string[] lines = File.ReadAllLines(filePath);
                    PlotData(lines);
                }
                else
                {
                    MessageBox.Show($"CSV file for {currentItemName} not found.");
                }
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
                countdown = 15;

                var random = new Random();
                string[] stocks = new string[7] { "celltrion", "hyundai", "kbbank", "krafton", "lgenergysolution", "naver", "samsung" };
                for(int i = 0; i<7; i++)
                {
                    string filename = stocks[i] + ".csv";
                    string filePath = Path.Combine(dataDirectory, filename);
                    UpdateStockFile(filePath, random);
                }
                currentDate = currentDate.AddDays(1);

                /*
                if (csvData != null && csvData.Count > 0)
                {
                    string oldData = csvData[currentIndex][1];
                    currentIndex = (currentIndex + 1) % csvData.Count;
                    UpdateListViewPrice(oldData, csvData[currentIndex][1]);
                }
                */

                foreach (ListViewItem item in listView1.Items)
                {
                    string filePath2 = GetCsvFilePath(item.SubItems[1].Text);

                    if (File.Exists(filePath2))
                    {
                        string[] lines = File.ReadAllLines(filePath2);
                        csvData = lines.Skip(1).Select(line => line.Split(',')).ToList();
                        if (csvData != null && csvData.Count > 0)
                        {
                            string oldData = csvData[currentIndex][1];
                            // currentIndex = (currentIndex + 1) % csvData.Count;
                            UpdateListViewPrice(item, oldData, csvData[(currentIndex + 1) % csvData.Count][1]);
                        }
                    }
                    
                }
                currentIndex++;

                UserTotalAsset();

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

                Account accountForm = Application.OpenForms.OfType<Account>().FirstOrDefault();
                if (accountForm != null)
                {
                    accountForm.Close();
                    accountForm.Dispose();
                    accountForm = new Account(label2.Text);
                    accountForm.Show();
                }

                if (!client.Connected)
                {
                    this.client.Connect(IPAddress.Parse("127.0.0.1"), 16000 + label2.Text.Length);
                }
                if (this.streamWrite == null)
                {
                    this.streamWrite = new StreamWriter(client.GetStream());
                }

                streamWrite.WriteLine(label2.Text + "," + label6.Text + "," + label15.Text);
                streamWrite.Flush();

/*                PacketClient.Client clientForm = new PacketClient.Client(label2.Text);

                if (clientForm != null)
                {
                    // label2.Text는 Username, label6.Text는 총자산. label15.Text는 DAY.
                    int.TryParse(label6.Text, out int total);
                    clientForm.TickByForm(label2.Text, total, label15.Text);
                    //clientForm.SendFile(filePath);
                }
*/

                if (currentPlotIndex < lastTenData.Length)
                {
                    string[] data = lastTenData[currentPlotIndex].Split(',');
                    string date = data[0];
                    double close = double.Parse(data[1]);

                    chart1.Series[0].Points.AddXY(date, close);
                    currentPlotIndex++;
                }

                dayCount++;
                if(dayCount == 9)
                {
                    timer.Stop();
                    this.Visible = false;
                    double raiseRate = double.Parse(label6.Text) / 1000000 - (double)1;
                    Ranking ranking = new Ranking(label2.Text, raiseRate);
                    ranking.Show();

                }
                label15.Text = "DAY" + dayCount;

                GameCount = true;
            }
        }

       

        private void guna2Button6_Click(object sender, EventArgs e)
        {
            if (GameCount)
            {
                timer.Stop();
                Game1.Form1 game1 = new Game1.Form1();
                DialogResult dResult = game1.ShowDialog();

                if (dResult == DialogResult.OK && game1.IsSuccess)
                {
                    MessageBox.Show("겜블링 성공 보상으로 10만원이 지급됩니다.");
                    totalMoney += 100000;
                    UpdateTotalMoneyLabel();
                    UserTotalAsset();
                }
                else
                {
                    MessageBox.Show("겜블링에 실패하셨습니다.");
                }
                GameCount = false;
                timer.Start();
            }
            else
            {
                MessageBox.Show("이번 라운드에 게임 횟수를 초과했습니다!");
            }

        }

        private void guna2Button7_Click(object sender, EventArgs e)
        {
            if(GameCount) {
                timer.Stop();
                game2.Form2 game2 = new game2.Form2();
                DialogResult dResult = game2.ShowDialog();

                if (dResult == DialogResult.OK && game2.IsSuccess)
                {
                    MessageBox.Show("경마 성공 보상으로 자산이 2배로 불어납니다.");
                    totalMoney *= 2;
                    UpdateTotalMoneyLabel();
                    UserTotalAsset();
                }
                else
                {
                    MessageBox.Show("경마 실패 대가로 자산이 절반으로 줄어듭니다.");
                    totalMoney /= 2;
                    UpdateTotalMoneyLabel();
                    UserTotalAsset();
                }
                GameCount = false;

                timer.Start();
        }
            else
            {
                MessageBox.Show("이번 라운드에 게임 횟수를 초과했습니다!");
            }
    
        }

        private void UpdateStockFile(string filePath, Random random)
        {
            var lines = new List<string>(File.ReadAllLines(filePath));

            // 기본 변동
            decimal changePercent = (decimal)(random.NextDouble() * 0.04 - 0.02);

            // 이전 주가 데이터 활용 (이동 평균)
            decimal previousPriceImpact = 0;
            int previousPrices = 0;
            int currentPrice = 0;

            for (int i = 1; i < 5; i++)
            {
                string[] fields = lines[i].Split(',');
                previousPrices += int.Parse(fields[1]);
                currentPrice = int.Parse(fields[1]);               
            }
            decimal averagePreviousPrice = previousPrices / 4;
            previousPriceImpact = (currentPrice - averagePreviousPrice) * 0.0000005m;

            // 종합 변동률 계산
            decimal totalChangePercent = changePercent + previousPriceImpact;
            totalChangePercent = Math.Round(totalChangePercent, 2);

            // 새로운 주가 계산
            decimal nextPrice = currentPrice * (1 + totalChangePercent);
            nextPrice = Math.Round(nextPrice / 100) * 100;

            // 현재 날짜를 "d/M/yy" 형식으로 변환하기
            string formattedDate = currentDate.ToString("M/d/yy", CultureInfo.InvariantCulture);
            string line = $"{formattedDate},{Convert.ToInt32(nextPrice)}";
            lines.Add(line);
            
            File.WriteAllLines(filePath, lines);
        }

        private void ClearCsvFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                var lines = File.ReadAllLines(filePath).ToList();
                if (lines.Count > 63)
                {
                    lines = lines.Take(63).ToList();
                    File.WriteAllLines(filePath, lines);
                }
            }
        }
    }
}
