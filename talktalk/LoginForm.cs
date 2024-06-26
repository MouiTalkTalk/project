﻿using System;
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
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            if (txtUsername.Text == "admin")
            {
                PacketServer.Server serverForm = new PacketServer.Server();
                serverForm.Show();
                this.Hide();
            }
            else if (!txtUsername.Text.Equals(string.Empty))
            {
                PacketClient.Client clientForm = new PacketClient.Client(txtUsername.Text);
                clientForm.Show();
                new Form1(txtUsername.Text).Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("User name is incorrect, try again");
                txtUsername.Clear();
                txtUsername.Focus();
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void txtUsername_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                 guna2Button1_Click(this, e);
            }
        }
    }
}
