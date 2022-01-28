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
    public partial class FormLogin : Form
    {
        public static string user;


        koneksi konn = new koneksi();
        public FormLogin()
        {
            InitializeComponent();
        }

        private void FormLogin_Load(object sender, EventArgs e)
        {

        }

        private void gunaButton1_Click(object sender, EventArgs e)
        {
            SqlConnection conn = konn.GetConn();
            user = tbUser.Text;
            try
            {
                if (tbUser.Text == "" || tbPass.Text == "")
                {
                    MessageBox.Show("Login Gagal");
                }
                else
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("select nama, password, level_user from tb_user where nama = '" + tbUser.Text + "' and password = '" + tbPass.Text + "'", conn);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            if (dr["level_user"].ToString() == "Administrator")
                            {
                                MessageBox.Show("Login Success! Welcome Administrator!");
                                Dashboard admin = new Dashboard();
                                admin.Show();
                                this.Hide();
                                conn.Close();
                            }
                            else if (dr["level_user"].ToString() == "User")
                            {
                                MessageBox.Show("Login Success! Welcome User!");
                                Dashboard2 user = new Dashboard2();
                                user.Show();
                                this.Hide();
                                conn.Close();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void tbPass_TextChanged(object sender, EventArgs e)
        {
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            Application.Exit();
        }
    }
}
