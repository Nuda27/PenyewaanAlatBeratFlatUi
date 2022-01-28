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
    public partial class Transaksi : Form
    {
        private SqlCommand cmd;
        private DataSet ds;
        private SqlDataAdapter da;

        koneksi konn = new koneksi();
        public Transaksi()
        {
            InitializeComponent();
        }
        void tampildata()
        {
            SqlConnection conn = konn.GetConn();
            try
            {
                conn.Open();
                cmd = new SqlCommand("select * from tb_trans", conn);
                ds = new DataSet();
                da = new SqlDataAdapter(cmd);
                da.Fill(ds, "tb_trans");
                dataGrid.DataSource = ds;
                dataGrid.DataMember = "tb_trans";
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
                cmd = new SqlCommand("select * from tb_trans where id_transaksi like '%" + tbSearch.Text + "%' or pelanggan like '%" + tbSearch.Text + "%' ", conn);
                ds = new DataSet();
                da = new SqlDataAdapter(cmd);
                da.Fill(ds, "tb_trans");
                dataGrid.DataSource = ds;
                dataGrid.DataMember = "tb_trans";
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
            tbHarga.Text = "";
            tbPel.Text = "";
            tbAlamat.Text = "";
            tbJs.Text = "";
            tbNo.Text = "";
        }
        void idOto()
        {
            long itung;
            string urut;
            SqlDataReader dr;
            SqlConnection conn = konn.GetConn();
            conn.Open();
            cmd = new SqlCommand("select id_transaksi from tb_trans where id_transaksi in(select max(id_transaksi) from tb_trans) order by id_transaksi desc", conn);
            dr = cmd.ExecuteReader();
            dr.Read();
            if (dr.HasRows)
            {
                itung = Convert.ToInt64(dr[0].ToString().Substring(dr["id_transaksi"].ToString().Length - 3, 3)) + 1;
                string idurut = "000" + itung;
                urut = "TR" + idurut.Substring(idurut.Length - 3, 3);
            }
            else
            {
                urut = "TR001";
            }
            dr.Close();
            tbId.Text = urut;
            conn.Close();
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            this.Close();
        }

        private void Transaksi_Load(object sender, EventArgs e)
        {
            tampildata();
            bersih();
            idOto();
        }

        private void btnSimpan_Click(object sender, EventArgs e)
        {
            SqlConnection conn = konn.GetConn();
            int a, b, total;
            a = int.Parse(this.tbJs.Text);
            b = int.Parse(this.tbHarga.Text);
            total = a * b;
            if (tbId.Text.Trim() == "" || tbPel.Text.Trim() == "" || tbNo.Text.Trim() == "" || tbAlamat.Text.Trim() == "" || tbJa.Text.Trim() == "" || tbJs.Text.Trim() == "" || tbHarga.Text.Trim() == "" || dtp.Text.Trim() == "" || dtpBalik.Text.Trim() == "")
            {
                MessageBox.Show("Data Tidak Lengkap", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                try
                {
                    cmd = new SqlCommand("insert into tb_trans values('" + tbId.Text + "', '" + tbPel.Text + "', '" + tbNo.Text + "', '" + dtp.Text + "', '" + dtpBalik.Text + "','" + tbAlamat.Text + "', '" + tbJa.Text + "', '" + tbJs.Text + "', '" + tbHarga.Text + "','"+total+"')", conn);
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
                tbId.Text = row.Cells["id_transaksi"].Value.ToString();
                tbPel.Text = row.Cells["pelanggan"].Value.ToString();
                tbNo.Text = row.Cells["no"].Value.ToString();
                dtp.Text = row.Cells["tgl_sewa"].Value.ToString();
                dtpBalik.Text = row.Cells["tgl_berakhir"].Value.ToString();
                tbAlamat.Text = row.Cells["alamat"].Value.ToString();
                tbJa.Text = row.Cells["jenis"].Value.ToString();
                tbJs.Text = row.Cells["jumlah_sewa"].Value.ToString();
                tbHarga.Text = row.Cells["harga"].Value.ToString();
                row.Cells["total"].Value.ToString();
            }
            catch (Exception x)
            {
                MessageBox.Show(x.ToString());
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            SqlConnection conn = konn.GetConn();
            int a, b, total;
            a = int.Parse(this.tbJs.Text);
            b = int.Parse(this.tbHarga.Text);
            total = a * b;
            if (tbId.Text.Trim() == "" || tbPel.Text.Trim() == "" || tbNo.Text.Trim() == "" || tbAlamat.Text.Trim() == "" || tbJa.Text.Trim() == "" || tbJs.Text.Trim() == "" || tbHarga.Text.Trim() == "" || dtp.Text.Trim() == "" || dtpBalik.Text.Trim() == "")
            {
                MessageBox.Show("Data Tidak Lengkap", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                try
                {
                    cmd = new SqlCommand("Update tb_trans set pelanggan = '" + tbPel.Text + "', no = '" + tbNo.Text + "', alamat = '" + tbAlamat.Text + "', tgl_sewa = '" + dtp.Text + "', tgl_berakhir = '" + dtpBalik.Text + "', jenis = '" + tbJa.Text + "', jumlah_sewa = '" + tbJs.Text + "', harga = '" + tbHarga.Text + "', total = '"+total+"' where  id_transaksi = '" + tbId.Text + "'", conn);
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
            if (MessageBox.Show("Apakah Anda Yakin Data Transaksi " + tbJa.Text + " ?", "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                SqlConnection conn = konn.GetConn();
                cmd = new SqlCommand("delete tb_trans  where  id_transaksi = '" + tbId.Text + "'", conn);
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

        private void dataGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
