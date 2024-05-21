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
    }
}
