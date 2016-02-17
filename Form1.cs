using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CustomersDatabase
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        DatabaseConnection objConnect;
        string conString;

        DataSet ds;
        DataRow dRow;

        int MaxRows;
        int incr = 0;

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                objConnect = new DatabaseConnection();
                conString = Properties.Settings.Default.CustomersConnectionString;

                objConnect.connection_string = conString;

                objConnect.Sql = Properties.Settings.Default.SQL;

                ds = objConnect.GetConnection;
                MaxRows = ds.Tables[0].Rows.Count;

                NavigateRecords();
            }
            catch (Exception err)
            {

                MessageBox.Show(err.Message);
            }
        }

        private void NavigateRecords()
        {
            dRow = ds.Tables[0].Rows[incr];
            txtName.Text = dRow.ItemArray.GetValue(1).ToString();
            txtSurname.Text = dRow.ItemArray.GetValue(2).ToString();
            txtTelephone.Text = dRow.ItemArray.GetValue(3).ToString();
            txtAddress.Text = dRow.ItemArray.GetValue(4).ToString();
            labelUpdate();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (incr != MaxRows - 1)
            {
                incr++;
                NavigateRecords();
            }
            else 
            {
                MessageBox.Show("No more rows in database.");
            }
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            if (incr > 0)
            {
                incr--;
                NavigateRecords();
            }
            else
            {
                MessageBox.Show("First row in database, cannot go to previous");
            }
        }

        private void btnLast_Click(object sender, EventArgs e)
        {
            if(incr != MaxRows -1)
            {
                incr = MaxRows - 1;
                NavigateRecords();
            }
        }

        private void btnFirst_Click(object sender, EventArgs e)
        {
            if(incr != 0)
            {
                incr = 0;
                NavigateRecords();
            }
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            txtAddress.Clear();
            txtName.Clear();
            txtSurname.Clear();
            txtTelephone.Clear();

            btnNew.Enabled = false;
            btnSave.Enabled = true;
            btnCancel.Enabled = true;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            NavigateRecords();
            btnNew.Enabled = true;
            btnSave.Enabled = false;
            btnCancel.Enabled = false;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            DataRow row = ds.Tables[0].NewRow();
            row[1] = txtName.Text; 
            row[2] = txtSurname.Text;
            row[3] = txtTelephone.Text;
            row[4] = txtAddress.Text;

            ds.Tables[0].Rows.Add(row);

            try
            {
                objConnect.UpdateDatabase(ds);
                MaxRows += 1;
                incr = MaxRows - 1;
                MessageBox.Show("Database successfully updated");
            }
            catch (Exception err)
            {

                MessageBox.Show(err.Message);
            }

            btnNew.Enabled = true;
            btnSave.Enabled = false;
            btnCancel.Enabled = false;

        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            DataRow row = ds.Tables[0].Rows[incr];
            row[1] = txtName.Text;
            row[2] = txtSurname.Text;
            row[3] = txtTelephone.Text;
            row[4] = txtAddress.Text;

            try
            {
                objConnect.UpdateDatabase(ds);

                MessageBox.Show("Record successfully updated");
            }
            catch (Exception err)
            {

                MessageBox.Show(err.Message);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                ds.Tables[0].Rows[incr].Delete();
                objConnect.UpdateDatabase(ds);
                MaxRows = ds.Tables[0].Rows.Count;
                incr--;

                MessageBox.Show("Record Deleted");

                NavigateRecords();
            }
            catch (Exception err)
            {

                MessageBox.Show(err.Message);
            }

            
        }

        private void labelUpdate()
        {
            lblState.Text = "Record " + (incr + 1) + " of " + MaxRows;
        }
   
    }
}
