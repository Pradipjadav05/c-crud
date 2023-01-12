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
    public partial class Form1 : Form
    {
        SqlConnection conn = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=D:\MCA\C#\Project\Desktop\CRUD_DEMO2\CRUD_DEMO2\Database1.mdf;Integrated Security=True");
        public Form1()
        {
            InitializeComponent();
            fillGrid();
            fillEnrollment();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            conn.Open();
            string sql = "delete from Student where Enrollment=@enrollment";
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@enrollment", txtEnrl.Text);
            cmd.ExecuteNonQuery();
            MessageBox.Show("Record deleted...");
            conn.Close();
            fillGrid();
            fillEnrollment();
            reset();
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            reset();
        }

        private void btninsert_Click(object sender, EventArgs e)
        {
            conn.Open();
            string sql = "Insert into Student values(@Enrollment,@Name,@Age,@City)";
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Enrollment", txtEnrl.Text);
            cmd.Parameters.AddWithValue("@Name", txtName.Text);
            cmd.Parameters.AddWithValue("@Age", txtAge.Text);
            cmd.Parameters.AddWithValue("@City", cmbCity.SelectedItem);
            cmd.ExecuteNonQuery();
            MessageBox.Show("Record Inserted...");
            conn.Close();
            fillGrid();
            fillEnrollment();
            reset();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            conn.Open();
            string sql = "select * from student where Enrollment = @Enrollment";
            SqlCommand cmd = new SqlCommand(sql,conn);
            cmd.Parameters.AddWithValue("@Enrollment", txtEnrl.Text);
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                txtName.Text = dr["Name"].ToString();
                txtAge.Text = dr["Age"].ToString();
                cmbCity.Text = dr["City"].ToString();
            }
            conn.Close();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            conn.Open();
            string sql = "update Student set Name = @name, Age = @age, City = @city where Enrollment = @enrollment";
            SqlCommand cmd = new SqlCommand(sql,conn);
            cmd.Parameters.AddWithValue("@name", txtName.Text);
            cmd.Parameters.AddWithValue("@age", txtAge.Text);
            cmd.Parameters.AddWithValue("@city", cmbCity.SelectedItem);
            cmd.Parameters.AddWithValue("@enrollment", txtEnrl.Text);
            cmd.ExecuteNonQuery();
            MessageBox.Show("Record Updated...");
            conn.Close();
            fillGrid();
            reset();
        }

        private void DGV_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtEnrl.Text = DGV.SelectedRows[0].Cells[0].Value.ToString();
            txtName.Text = DGV.SelectedRows[0].Cells[1].Value.ToString();
            txtAge.Text = DGV.SelectedRows[0].Cells[2].Value.ToString();
            cmbCity.Text = DGV.SelectedRows[0].Cells[3].Value.ToString();
        }

        public void reset()
        {
            txtName.Text = txtEnrl.Text = txtAge.Text = string.Empty;
            cmbCity.SelectedItem = null;
        }

        public void fillGrid()
        {
            conn.Open();
            string sql = "select * from Student";
            SqlDataAdapter da = new SqlDataAdapter(sql,conn);
            DataSet ds = new DataSet();
            da.Fill(ds);
            DGV.DataSource = ds.Tables[0];
            conn.Close();
        }

        private void cmbSort_SelectedIndexChanged(object sender, EventArgs e)
        {
            string sql = "select * from Student order by '" + cmbSort.SelectedItem + "'";
            SqlDataAdapter da = new SqlDataAdapter(sql, conn);
            DataSet ds = new DataSet();
            da.Fill(ds);
            DGV.DataSource = ds.Tables[0];
        }

        public void fillEnrollment()
        {
            conn.Open();
            string sql = "select Enrollment from Student";
            SqlCommand cmd = new SqlCommand(sql, conn);
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                cmbEnrollment.Items.Add(dr[0]);
            }
            conn.Close();
        }

        private void cmbEnrollment_SelectedIndexChanged(object sender, EventArgs e)
        {
            string sql = "select * from Student where Enrollment = '" + cmbEnrollment.SelectedItem + "'";
            conn.Open();
            SqlDataAdapter da = new SqlDataAdapter(sql, conn);
            DataSet ds = new DataSet();
            da.Fill(ds);
            DGV.DataSource = ds.Tables[0];
            conn.Close();
        }

        private void feesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FeesDetails f = new FeesDetails();
            f.Show();
            this.Hide();
        }
    }
}
