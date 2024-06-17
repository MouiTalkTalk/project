﻿using System;
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
            lblMyRatio.Text = FormatRatio(RaiseRate.ToString());
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
    }
}
