using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.Entity;

namespace EmployeeApplication
{
    public partial class Form1 : Form
    {
        Table model = new Table();

        public Form1()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Clear();
        }

        void Clear()
        {
            txtName.Text = txtLast.Text = txtMail.Text = txtPhone.Text = "";
            btnSave.Text = "Save";
            btnDel.Enabled = false;
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Clear();
            PopulateDataGridView();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            model.Name = txtName.Text.Trim();
            model.LastName = txtLast.Text.Trim();
            model.Email = txtMail.Text.Trim();
            model.Phone = Convert.ToInt32(txtPhone.Text.Trim());

            using (DBEntities db = new DBEntities())
            {

                if (model.EmployeeId == 0) //insert
                    db.Tables.Add(model);
                else //Update
                    db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();
            }

            Clear();
            PopulateDataGridView();
            MessageBox.Show("Submitted to DB");
        }

        void PopulateDataGridView()
        {
            dataGridView1.AutoGenerateColumns = false;
            using (DBEntities db = new DBEntities())
            {
                dataGridView1.DataSource = db.Tables.ToList<Table>();
            }

        }

        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
            if(dataGridView1.CurrentRow.Index != -1)
            {
                model.EmployeeId = Convert.ToInt32(dataGridView1.CurrentRow.Cells["EmpId"].Value);
                using (DBEntities db = new DBEntities())
                {
                    model = db.Tables.Where(x => x.EmployeeId == model.EmployeeId).FirstOrDefault();
                    txtName.Text = model.Name;
                    txtLast.Text = model.LastName;
                    txtMail.Text = model.Email;
                    txtPhone.Text = Convert.ToString(model.Phone);
                              
                }
                btnSave.Text = "Update";
                btnDel.Enabled = true;

            }
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you wish to delete this record??", "Delete Operation", MessageBoxButtons.YesNo) == DialogResult.Yes )
            {
                using (DBEntities db = new DBEntities())
                {
                    var entry = db.Entry(model);
                    if (entry.State == EntityState.Detached)
                        db.Tables.Attach(model);
                    db.Tables.Remove(model);
                    db.SaveChanges();
                    PopulateDataGridView();
                    Clear();

                    MessageBox.Show("Deleted Successfully");
                }
            }
        }
    }
}
