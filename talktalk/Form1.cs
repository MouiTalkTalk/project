using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace talktalk
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
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
    }
}
