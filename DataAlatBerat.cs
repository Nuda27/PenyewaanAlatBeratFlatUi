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
    public partial class DataAlatBerat : Form
    {
        private SqlCommand cmd;
        private DataSet ds;
        private SqlDataAdapter da;

        koneksi konn = new koneksi();
        public DataAlatBerat()
        {
            InitializeComponent();
        }

        void tampildata()
        {
            SqlConnection conn = konn.GetConn();
            try
            {
                conn.Open();
                cmd = new SqlCommand("select * from tb_alat", conn);
                ds = new DataSet();
                da = new SqlDataAdapter(cmd);
                da.Fill(ds, "tb_alat");
                dataGrid.DataSource = ds;
                dataGrid.DataMember = "tb_alat";
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
                cmd = new SqlCommand("select * from tb_alat where id_alat like '%" + tbSearch.Text + "%' or jenis like '%" + tbSearch.Text + "%' ", conn);
                ds = new DataSet();
                da = new SqlDataAdapter(cmd);
                da.Fill(ds, "tb_alat");
                dataGrid.DataSource = ds;
                dataGrid.DataMember = "tb_alat";
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

        void bersih()
        {
            tbId.Text = "";
            tbJa.Text = "";
            tbJml.Text = "";
            tbTahun.Text = "";
            tbHarga.Text = null;
        }

        void idOto()
        {
            long itung;
            string urut;
            SqlDataReader dr;
            SqlConnection conn = konn.GetConn();
            conn.Open();
            cmd = new SqlCommand("select id_alat from tb_alat where id_alat in(select max(id_alat) from tb_alat) order by id_alat desc", conn);
            dr = cmd.ExecuteReader();
            dr.Read();
            if (dr.HasRows)
            {
                itung = Convert.ToInt64(dr[0].ToString().Substring(dr["id_alat"].ToString().Length - 3, 3)) + 1;
                string idurut = "000" + itung;
                urut = "AB" + idurut.Substring(idurut.Length - 3, 3);
            }
            else
            {
                urut = "AB001";
            }
            dr.Close();
            tbId.Text = urut;
            conn.Close();

        }
        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            this.Close();
        }

        private void DataAlatBerat_Load(object sender, EventArgs e)
        {
            tampildata();
            bersih();
            idOto();
        }

        private void btnSimpan_Click(object sender, EventArgs e)
        {
            SqlConnection conn = konn.GetConn();
            if (tbId.Text.Trim() == "" || tbJa.Text.Trim() == "" || tbJml.Text.Trim() == "" || tbTahun.Text.Trim() == "" || tbHarga.Text.Trim() == "")
            {
                MessageBox.Show("Data Tidak Lengkap", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                try
                {
                    cmd = new SqlCommand("insert into tb_alat values('" + tbId.Text + "', '" + tbJa.Text + "', '" + tbJml.Text + "', '" + tbTahun.Text + "', '" + tbHarga.Text + "')", conn);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Insert Data Berhasil", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void dataGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            try
            {
                DataGridViewRow row = this.dataGrid.Rows[e.RowIndex];
                tbId.Text = row.Cells["id_alat"].Value.ToString();
                tbJa.Text = row.Cells["jenis"].Value.ToString();
                tbJml.Text = row.Cells["jumlah"].Value.ToString();
                tbTahun.Text = row.Cells["Tahun"].Value.ToString();
                tbHarga.Text = row.Cells["Harga"].Value.ToString();
            }
            catch (Exception x)
            {
                MessageBox.Show(x.ToString());
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            SqlConnection conn = konn.GetConn();
            if (tbId.Text.Trim() == "" || tbJa.Text.Trim() == "" || tbJml.Text.Trim() == "" || tbTahun.Text.Trim() == "" || tbHarga.Text.Trim() == "")
            {
                MessageBox.Show("Data Tidak Lengkap", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                try
                {
                    cmd = new SqlCommand("Update tb_alat set jenis = '" + tbJa.Text + "', jumlah = '" + tbJml.Text + "', Tahun = '" + tbTahun.Text + "', Harga = '" + tbHarga.Text + "' where  id_alat = '" + tbId.Text + "'", conn);
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
            if (MessageBox.Show("Apakah Anda Yakin Data Alat Berat " + tbJa.Text + " ?", "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                SqlConnection conn = konn.GetConn();
                cmd = new SqlCommand("delete tb_alat  where  id_alat = '" + tbId.Text + "'", conn);
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
