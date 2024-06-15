using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace talktalk
{
    public partial class Account : Form
    {
        public Account()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void Account_Load(object sender, EventArgs e)
        {
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            foreach (DataGridViewColumn column in dataGridView1.Columns)
            {
                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            }

            LoadAdminCsv();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*";
            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = openFileDialog.FileName;

                int asset = 0;
                int profit = 0;
                int totalAsset = 0;

                using (StreamReader reader = new StreamReader(filePath))
                {
                    while (!reader.EndOfStream)
                    {
                        string line = reader.ReadLine();
                        string[] values = line.Split(',');

                        string name = values[0];
                        int quantity = int.Parse(values[1]);
                        int buyPrice = int.Parse(values[2]);
                        int currentPrice = int.Parse(values[3]);
                        int profitLoss = (currentPrice - buyPrice) * quantity;
                        double profitLossRatio = (double)profitLoss / (buyPrice * quantity) * 100;
                        string ratioString = profitLossRatio.ToString("0.00") + "%";

                        dataGridView1.Rows.Add(
                            name,
                            quantity,
                            profitLoss,
                            ratioString,
                            currentPrice * quantity,
                            buyPrice,
                            currentPrice
                        );

                        asset += currentPrice * quantity;
                        profit += profitLoss;
                        totalAsset += buyPrice * quantity;
                    }
                }
                double profitRatio = (double)profit / totalAsset * 100;

                lblAsset.Text = asset.ToString();
                lblProfit.Text = profit.ToString();
                lblProfitRatio.Text = profitRatio.ToString("0.00") + "%";

                for (int i = 1; i <= 6; i++)
                {
                    dataGridView1.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                }
            }
        }

        private void LoadAdminCsv()
        {
            string filePath = "C:\\Users\\DONGHO\\Desktop\\#TT\\TEAM\\talktalk\\bin\\Debug\\admin.csv";
            if (File.Exists(filePath))
            {
                using (StreamReader reader = new StreamReader(filePath))
                {
                    string line;
                    reader.ReadLine();
                    while ((line = reader.ReadLine()) != null)
                    {
                        string[] values = line.Split(',');
                        string name = values[0];
                        int quantity = int.Parse(values[1]);
                        decimal totalCost = decimal.Parse(values[2]);
                        decimal buyPrice = totalCost / quantity;
                        decimal currentPrice = GetCurrentPriceFromListView(name);

                        int profitLoss = (int)((currentPrice - buyPrice) * quantity);
                        double profitLossRatio = (double)profitLoss / (double)(buyPrice * quantity) * 100;
                        string ratioString = profitLossRatio.ToString("0.00") + "%";

                        dataGridView1.Rows.Add(
                            name,
                            quantity,
                            profitLoss,
                            ratioString,
                            currentPrice * quantity,
                            buyPrice,
                            currentPrice
                        );
                    }
                }
            }
            else
            {
                MessageBox.Show("admin.csv file not found.");
            }
        }
        private decimal GetCurrentPriceFromListView(string itemName)
        {
            Form1 form1 = Application.OpenForms.OfType<Form1>().FirstOrDefault();
            if (form1 != null)
            {
                foreach (ListViewItem item in form1.listView1.Items)
                {
                    if (item.SubItems[1].Text == itemName)
                    {
                        return decimal.Parse(item.SubItems[2].Text.Replace(",", ""));
                    }
                }
            }
            return 0;
        }

        public void UpdateCurrentPrices()
        {
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                string itemName = row.Cells["name"].Value.ToString();
                decimal currentPrice = GetCurrentPriceFromListView(itemName);
                int quantity = int.Parse(row.Cells["quantity"].Value.ToString());
                decimal buyPrice = decimal.Parse(row.Cells["buyPrice"].Value.ToString());

                int profitLoss = (int)((currentPrice - buyPrice) * quantity);
                double profitLossRatio = (double)profitLoss / (double)(buyPrice * quantity) * 100;
                string ratioString = profitLossRatio.ToString("0.00") + "%";

                row.Cells["currentPrice"].Value = currentPrice;
                row.Cells["profitLoss"].Value = profitLoss;
                row.Cells["profitLossRatio"].Value = ratioString;
                row.Cells["currentAsset"].Value = currentPrice * quantity;
            }
        }
    }
}
