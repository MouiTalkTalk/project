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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace talktalk
{
    public partial class Ranking : Form
    {

        private string dataDirectory;
        private void SetDataDirectory()
        {
            DirectoryInfo currentDir = new DirectoryInfo(Application.StartupPath);

            DirectoryInfo dataDir = currentDir.Parent.Parent.Parent;

            dataDirectory = Path.Combine(dataDir.FullName, "data");
        }

        public Ranking()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            SetDataDirectory();
        }

        public Ranking(string name, double RaiseRate)
        {
            InitializeComponent();
            SetDataDirectory();
            this.StartPosition = FormStartPosition.CenterScreen;
            lblMyName.Text = name;
            lblMyRatio.Text = RaiseRate.ToString("F2");
            string filename = "Ranking.csv";
            string filePath = Path.Combine(dataDirectory, filename);
            UpdateCsvFile(filePath, name, RaiseRate);
            DisplayTopThreeRows(filePath);

        }

        private void DisplayTopThreeRows(string filePath)
        {
            var lines = File.ReadAllLines(filePath).Skip(1).ToList();

            if (lines.Count < 3)
            {
                MessageBox.Show("The CSV file does not contain enough rows.");
                return;
            }

            var topThreeRows = lines.Take(3).Select(line => line.Split(',')).ToList();

            if (topThreeRows.Count > 0)
            {
                lblFirstName.Text = topThreeRows[0][0];
                lblFirstRatio.Text = double.TryParse(topThreeRows[0][1], out double firstRatio)
                                     ? firstRatio.ToString("F2") : "Invalid";
            }

            if (topThreeRows.Count > 1)
            {
                lblSecondName.Text = topThreeRows[1][0];
                lblSecondRatio.Text = double.TryParse(topThreeRows[1][1], out double secondRatio)
                                      ? secondRatio.ToString("F2") : "Invalid";
            }

            if (topThreeRows.Count > 2)
            {
                lblThirdName.Text = topThreeRows[2][0];
                lblThirdRatio.Text = double.TryParse(topThreeRows[2][1], out double thirdRatio)
                                     ? thirdRatio.ToString("F2") : "Invalid";
            }
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*";
            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = openFileDialog.FileName;

                List<string> names = new List<string>();
                List<string> ratios = new List<string>();

                using (StreamReader reader = new StreamReader(filePath))
                {
                    while (!reader.EndOfStream)
                    {
                        string line = reader.ReadLine();
                        string[] values = line.Split(',');

                        if (values.Length >= 2)
                        {
                            names.Add(values[0]);
                            ratios.Add(values[1]);
                        }
                    }
                }

                if (names.Count >= 1)
                {
                    lblFirstName.Text = names[0];
                    lblFirstRatio.Text = FormatRatio(ratios[0]);
                }

                if (names.Count >= 2)
                {
                    lblSecondName.Text = names[1];
                    lblSecondRatio.Text = FormatRatio(ratios[1]);
                }

                if (names.Count >= 3)
                {
                    lblThirdName.Text = names[2];
                    lblThirdRatio.Text = FormatRatio(ratios[2]);
                }
            }
        }

        private string FormatRatio(string ratio)
        {
            if (!ratio.Contains('-'))
            {
                return '+' + ratio + '%';
            }
            else
            {
                return ratio + '%';
            }
        }

        private void Ranking_FormClosed(object sender, FormClosedEventArgs e)
        {
            string username = lblMyName.Text;
            string filename = username + ".csv";
            string filePath = Path.Combine(dataDirectory, filename);
            if(File.Exists(filePath)) { File.Delete(filePath); }
            Application.Exit();
        }

        private void UpdateCsvFile(string filePath, string name, double raiseRate)
        {
            var lines = new List<string>(File.ReadAllLines(filePath));
            bool itemExists = false;

            for (int i = 1; i < lines.Count; i++)
            {
                string[] fields = lines[i].Split(',');
                if (fields.Length == 2 && fields[0] == name)
                {
                    double oldraiseRate;
                    if (double.TryParse(fields[1], out oldraiseRate))
                    {
                        if (oldraiseRate < raiseRate) { oldraiseRate = raiseRate; }
                        lines[i] = $"{name},{oldraiseRate}";
                        itemExists = true;
                        break;
                    }
                }
            }

            if (!itemExists)
            {
                lines.Add($"{name},{raiseRate}");
            }

            File.WriteAllLines(filePath, lines);

            SortCsvFileByRatio(filePath);
        }

        private void SortCsvFileByRatio(string filePath)
        {
            var lines = new List<string>();

            using (var reader = new StreamReader(filePath, Encoding.UTF8))
            {
                while (!reader.EndOfStream)
                {
                    lines.Add(reader.ReadLine());
                }
            }

            if (lines.Count <= 1) return;

            var header = lines[0];
            var dataLines = lines.Skip(1)
                                 .Select(line => line.Split(','))
                                 .Where(fields => fields.Length == 2)
                                 .Select(fields => new
                                 {
                                     Name = fields[0],
                                     RatioString = fields[1],
                                     OriginalLine = string.Join(",", fields)
                                 })
                                 .OrderByDescending(item => ParseDouble(item.RatioString))
                                 .Select(item => item.OriginalLine)
                                 .ToList();

            var sortedLines = new List<string> { header };
            sortedLines.AddRange(dataLines);

            File.WriteAllLines(filePath, sortedLines, Encoding.UTF8);
        }

        private double ParseDouble(string value)
        {
            if (double.TryParse(value, out double result))
            {
                return result;
            }
            return double.MinValue;
        }
    }
}
