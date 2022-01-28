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
    public partial class DataUser : Form
    {
        private SqlCommand cmd;
        private DataSet ds;
        private SqlDataAdapter da;

        koneksi konn = new koneksi();
        public DataUser()
        {
            InitializeComponent();
        }

        void tampildata()
        {
            SqlConnection conn = konn.GetConn();
            try
            {
                conn.Open();
                cmd = new SqlCommand("select * from tb_user", conn);
                ds = new DataSet();
                da = new SqlDataAdapter(cmd);
                da.Fill(ds, "tb_user");
                dataGrid.DataSource = ds;
                dataGrid.DataMember = "tb_user";
                dataGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }
            catch (Exception x)
            {
                MessageBox.Show(x.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        void caridata()
        {
            SqlConnection conn = konn.GetConn();
            try
            {
                    conn.Open();
                    SqlDataAdapter sqlInsert = new SqlDataAdapter("Exec spUser @User", conn);
                    sqlInsert.SelectCommand.Parameters.AddWithValue("@User", tbSearch.Text.Trim());
                    sqlInsert.SelectCommand.ExecuteNonQuery();
                    DataTable dt = new DataTable();
                    sqlInsert.Fill(dt);
                    dataGrid.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void bersih()
        {
            tbId.Text = "";
            tbNama.Text = "";
            tbEmail.Text = "";
            tbPass.Text = "";
            cbLu.Text = null;
        }

        void idOto()
        {
            long itung;
            string urut;
            SqlDataReader dr;
            SqlConnection conn = konn.GetConn();
            conn.Open();
            cmd = new SqlCommand("select id_user from tb_user where id_user in(select max(id_user) from tb_user) order by id_user desc",conn);
            dr = cmd.ExecuteReader();
            dr.Read();
            if (dr.HasRows)
            {
                itung = Convert.ToInt64(dr[0].ToString().Substring(dr["id_user"].ToString().Length - 3, 3)) +1;
                string idurut = "000" + itung;
                urut = "ID" + idurut.Substring(idurut.Length - 3, 3);
            }
            else
            {
                urut = "ID001";
            }
            dr.Close();
            tbId.Text = urut;
            conn.Close();

        }
        void combo()
        {
            cbLu.Items.Add("Administrator");
            cbLu.Items.Add("User");
        }
        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            this.Close();
        }

        private void DataUser_Load(object sender, EventArgs e)
        {
            tampildata();
            combo();
            bersih();
            idOto();
        }

        private void gunaButton1_Click(object sender, EventArgs e)
        {
            SqlConnection conn = konn.GetConn();
            if (tbId.Text.Trim() == "" || tbNama.Text.Trim() == "" || tbEmail.Text.Trim() == "" || tbPass.Text.Trim() == "" || cbLu.Text.Trim() == "")
            {
                MessageBox.Show("Data Tidak Lengkap", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                try
                {
                        SqlCommand sqlInsert = new SqlCommand("Exec spINSERTUSER @id_user, @nama, @email, @password, @level", conn);
                        conn.Open();
                        sqlInsert.Parameters.AddWithValue("@id_user", tbId.Text.Trim());
                        sqlInsert.Parameters.AddWithValue("@nama", tbNama.Text.Trim());
                        sqlInsert.Parameters.AddWithValue("@email", tbEmail.Text.Trim());
                        sqlInsert.Parameters.AddWithValue("@password", tbPass.Text.Trim());
                        sqlInsert.Parameters.AddWithValue("@level", cbLu.Text.Trim());
                        sqlInsert.ExecuteNonQuery();
                        MessageBox.Show("Data Kasimpen", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        {
                            SqlDataAdapter getData = new SqlDataAdapter("SELECT * FROM tb_user Order By id_user Asc", conn);
                            getData.SelectCommand.ExecuteNonQuery();
                            DataTable dt = new DataTable();
                            getData.Fill(dt);
                            dataGrid.DataSource = dt;
                        }
                    tampildata();
                    bersih();
                    idOto();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void dataGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                DataGridViewRow row = this.dataGrid.Rows[e.RowIndex];
                tbId.Text = row.Cells["id_user"].Value.ToString();
                tbNama.Text = row.Cells["nama"].Value.ToString();
                tbEmail.Text = row.Cells["email"].Value.ToString();
                tbPass.Text = row.Cells["password"].Value.ToString();
                cbLu.Text = row.Cells["level_user"].Value.ToString();
            }
            catch (Exception x)
            {
                MessageBox.Show(x.ToString());
            }
        }

        private void gunaButton2_Click(object sender, EventArgs e)
        {
            SqlConnection conn = konn.GetConn();
            if (tbId.Text.Trim() == "" || tbNama.Text.Trim() == "" || tbEmail.Text.Trim() == "" || tbPass.Text.Trim() == "" || cbLu.Text.Trim() == "")
            {
                MessageBox.Show("Data Tidak Lengkap", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                try
                {
                    cmd = new SqlCommand("Update tb_user set nama = '" + tbNama.Text + "', email = '" + tbEmail.Text + "', password = '" + tbPass.Text + "', level_user = '" + cbLu.Text + "' where  id_user = '" + tbId.Text + "'", conn);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Ubah Data Berhasil", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    tampildata();
                    bersih();
                    idOto();
                }
                catch (Exception x)
                {
                    MessageBox.Show(x.Message);
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        private void btnHapus_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Apakah Anda Yakin Data User "+tbNama.Text+ " ?", "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                SqlConnection conn = konn.GetConn();
                cmd = new SqlCommand("delete tb_user  where  id_user = '" + tbId.Text + "'", conn);
                conn.Open();
                cmd.ExecuteNonQuery();
                MessageBox.Show("Hapus Data Berhasil", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                tampildata();
                bersih();
                idOto();
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            bersih();
            idOto();
        }

        private void tbSearch_TextChanged(object sender, EventArgs e)
        {
            caridata();
        }
    }
}
