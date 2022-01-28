using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
namespace pos
{
    public partial class Dashboard2 : Form
    {
        public Dashboard2()
        {
            InitializeComponent();
            customsub();
        }
        private void customsub()
        {
            panelSubMenu.Visible = false;
        }
        private void hidesub()
        {
            if (panelSubMenu.Visible == true)
                panelSubMenu.Visible = false;
        }

        private void showsub(Panel sub)
        {
            if (sub.Visible == false)
            {
                hidesub();
                sub.Visible = true;
            }
            else
                sub.Visible = false;
        }

        private Form activeForm = null;
        private void tampilan(Form tampil)
        {
            if (activeForm != null)
                activeForm.Close();
            activeForm = tampil;
            tampil.TopLevel = false;
            tampil.FormBorderStyle = FormBorderStyle.None;
            tampil.Dock = DockStyle.Fill;
            panelTampil.Controls.Add(tampil);
            panelTampil.Tag = tampil;
            tampil.BringToFront();
            tampil.Show();
        }

        private void Dashboard2_Load(object sender, EventArgs e)
        {
            lblUser.Text = FormLogin.user;
        }

        private void btnDataMaster_Click(object sender, EventArgs e)
        {
            showsub(panelSubMenu);
        }

        private void btnAb_Click(object sender, EventArgs e)
        {
            tampilan(new DataAlatBerat());
            hidesub();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            tampilan(new Transaksi());
            hidesub();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            FormLogin fl = new FormLogin();
            fl.Show();
            this.Close();
        }

        private void pictureBox2_MouseClick(object sender, MouseEventArgs e)
        {
            Application.Exit();
        }
    }
}
