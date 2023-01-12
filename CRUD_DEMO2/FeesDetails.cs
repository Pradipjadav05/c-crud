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

namespace CRUD_DEMO2
{
    public partial class FeesDetails : Form
    {
        SqlConnection conn = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=D:\MCA\C#\Project\Desktop\CRUD_DEMO2\CRUD_DEMO2\Database1.mdf;Integrated Security=True");
        public FeesDetails()
        {
            InitializeComponent();
            fillEnrollment();
            fillGrid();
        }

        private void btninsert_Click(object sender, EventArgs e)
        {
            conn.Open();
            string sql = "Insert into Fees(Enrollment,Amount,Date) values(@Enrollment,@Amount,@Date)";
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Enrollment", cmbEnroll.SelectedItem);
            cmd.Parameters.AddWithValue("@Amount", txtAmount.Text);
            cmd.Parameters.AddWithValue("@Date", payDate.Value.ToString());
            cmd.ExecuteNonQuery();
            MessageBox.Show("Record inserted...");
            fillGrid();
            clear();
            conn.Close();
        }
        public void fillEnrollment()
        {
            conn.Open();
            string sql = "select Enrollment from Student";
            SqlCommand cmd = new SqlCommand(sql,conn);
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                cmbEnroll.Items.Add(dr[0]);
                cmbEnrollment.Items.Add(dr[0]);
            }
            conn.Close();
        }
        public void fillGrid()
        {
            string sql = "select f.FeesId, f.Enrollment, s.Name, f.Amount, f.Date from Student s,Fees f where s.Enrollment = f.Enrollment";
            SqlDataAdapter da = new SqlDataAdapter(sql, conn);
            DataSet ds = new DataSet();
            da.Fill(ds);
            DGV.DataSource = ds.Tables[0];
        }
        public void clear()
        {
            txtReceip.Text = txtAmount.Text = string.Empty;
            cmbEnroll.SelectedItem = null;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            conn.Open();
            string sql = "select * from Fees where FeesId = @feesid";
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("feesid", txtReceip.Text);
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                cmbEnroll.Text = dr["Enrollment"].ToString();
                txtAmount.Text = dr["Amount"].ToString();
                payDate.Text = dr["Date"].ToString();
            }
            conn.Close();
        }

        private void DGV_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtReceip.Text = DGV.SelectedRows[0].Cells[0].Value.ToString();
            cmbEnroll.Text = DGV.SelectedRows[0].Cells[1].Value.ToString();
            txtAmount.Text = DGV.SelectedRows[0].Cells[3].Value.ToString();
            payDate.Text = DGV.SelectedRows[0].Cells[4].Value.ToString();
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            clear();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            conn.Open();
            string sql = "update Fees set Enrollment = @Enrollment, Amount = @Amount, Date = @Date where FeesId = @FeesId";
            SqlCommand cmd = new SqlCommand(sql,conn);
            cmd.Parameters.AddWithValue("@Enrollment", cmbEnroll.SelectedItem);
            cmd.Parameters.AddWithValue("@Amount", txtAmount.Text);
            cmd.Parameters.AddWithValue("@Date", payDate.Value.ToString());
            cmd.Parameters.AddWithValue("@FeesId", txtReceip.Text);
            cmd.ExecuteNonQuery();
            MessageBox.Show("Record Updated...");
            conn.Close();
            fillGrid();
            clear();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            conn.Open();
            string sql = "Delete from Fees where FeesId = @FeesId";
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@FeesId", txtReceip.Text);
            cmd.ExecuteNonQuery();
            MessageBox.Show("Record Deleted...");
            conn.Close();
            fillGrid();
            clear();
        }

        private void cmbSort_SelectedIndexChanged(object sender, EventArgs e)
        {
            string sql = "select f.FeesId, f.Enrollment, s.Name, f.Amount, f.Date from Student s,Fees f where s.Enrollment = f.Enrollment order by '"+cmbSort.SelectedItem+"'";
            SqlDataAdapter da = new SqlDataAdapter(sql, conn);
            DataSet ds = new DataSet();
            da.Fill(ds);
            DGV.DataSource = ds.Tables[0];
        }

        private void cmbEnrollment_SelectedIndexChanged(object sender, EventArgs e)
        {
            string sql = "select f.FeesId, f.Enrollment, s.Name, f.Amount, f.Date from Student s,Fees f where s.Enrollment = f.Enrollment and f.Enrollment = '"+cmbEnrollment.SelectedItem+"'";
            SqlDataAdapter da = new SqlDataAdapter(sql, conn);
            DataSet ds = new DataSet();
            da.Fill(ds);
            DGV.DataSource = ds.Tables[0];
        }

        private void feesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form1 f = new Form1();
            f.Show();
            this.Hide();
        }
    }
}
