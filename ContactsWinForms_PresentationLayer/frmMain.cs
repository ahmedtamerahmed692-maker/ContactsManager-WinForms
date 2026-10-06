using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ContactsBusinessLayer;

namespace ContactsWinForms_PresentationLayer
{
    public partial class frmMain : Form
    {
        private DataTable _dtContacts;

        public frmMain()
        {
            InitializeComponent();
            _StyleForm();
            _StyleGrid();
        }
        //تظبيط الشكل ================================================\\
        private void _StyleForm()
        {
            this.BackColor = AppColors.Background;
            pnlTop.BackColor = AppColors.Background;

            // العنوان
            lblTitle.ForeColor = AppColors.Header;

            // زرار Add
            btnAdd.FlatStyle = FlatStyle.Flat;
            btnAdd.FlatAppearance.BorderSize = 0;
            btnAdd.BackColor = AppColors.Header;
            btnAdd.ForeColor = Color.White;
            btnAdd.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            btnAdd.Cursor = Cursors.Hand;
        }
        private void _StyleGrid()
        {
            dgvContacts.EnableHeadersVisualStyles = false;

            // الهيدر
            dgvContacts.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvContacts.ColumnHeadersHeight = 50;
            dgvContacts.ColumnHeadersDefaultCellStyle.BackColor = AppColors.Header;
            dgvContacts.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvContacts.ColumnHeadersDefaultCellStyle.SelectionBackColor = AppColors.Header;
            dgvContacts.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            dgvContacts.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dgvContacts.ColumnHeadersDefaultCellStyle.Padding = new Padding(8, 0, 0, 0);
            dgvContacts.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.False;

            // الصفوف
            dgvContacts.RowTemplate.Height = 40;
            dgvContacts.DefaultCellStyle.Font = new Font("Segoe UI", 10);
            dgvContacts.DefaultCellStyle.ForeColor = AppColors.TextDark;
            dgvContacts.BackgroundColor = Color.White;
            dgvContacts.BorderStyle = BorderStyle.None;
            dgvContacts.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvContacts.GridColor = Color.FromArgb(230, 230, 230);
            dgvContacts.AlternatingRowsDefaultCellStyle.BackColor = AppColors.RowAlt;

            // التحديد
            dgvContacts.DefaultCellStyle.SelectionBackColor = AppColors.RowSelected;
            dgvContacts.DefaultCellStyle.SelectionForeColor = Color.Black;
        }
        //================================================================\\
        
        private void _LoadContacts()
        {
            try
            {
                _dtContacts = clsContact.GetAllContacts();
                dgvContacts.DataSource = _dtContacts;

                dgvContacts.Columns["ContactID"].HeaderText = "ID";
                dgvContacts.Columns["FirstName"].HeaderText = "First Name";
                dgvContacts.Columns["LastName"].HeaderText = "Last Name";
                dgvContacts.Columns["Email"].HeaderText = "Email Address";
                dgvContacts.Columns["Phone"].HeaderText = "Phone Number";
                dgvContacts.Columns["Address"].HeaderText = "Address";
                dgvContacts.Columns["DateOfBirth"].HeaderText = "Date of Birth";

                dgvContacts.Columns["CountryID"].Visible = false;
                dgvContacts.Columns["ImagePath"].Visible = false;

                foreach (DataGridViewRow row in dgvContacts.Rows)
                    row.Height = 40;

                dgvContacts.ClearSelection();   // يشيل التحديد الأزرق من أول خلية
            }
            catch (Exception)
            {
                MessageBox.Show("An error occurred while loading contacts.",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            _LoadContacts();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvContacts.CurrentRow == null || dgvContacts.CurrentRow.Cells["ContactID"].Value == DBNull.Value)
                {
                    MessageBox.Show("Please select a contact to delete.",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                int ID = Convert.ToInt32(dgvContacts.CurrentRow.Cells["ContactID"].Value);

                DialogResult result = MessageBox.Show("Are you sure you want to delete this contact?",
                       "Delete Contact", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);

                if (result != DialogResult.Yes)
                    return;

                if (!clsContact.DeleteContact(ID))
                {
                    MessageBox.Show("Contact not found or could not be deleted.",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show("Contact deleted successfully.",
                        "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    _LoadContacts(); 
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while deleting the contact: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvContacts.CurrentRow == null || dgvContacts.CurrentRow.Cells["ContactID"].Value == DBNull.Value)
                {
                    MessageBox.Show("Please select a contact to Edit.",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                int ID = Convert.ToInt32(dgvContacts.CurrentRow.Cells["ContactID"].Value);

                DialogResult result = MessageBox.Show("Are you sure you want to Edit this contact?",
                       "Edit Contact", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);

                if (result != DialogResult.Yes)
                    return;
                using (frmAddEditContact frm = new frmAddEditContact(ID))
                {
                    frm.ShowDialog();
                }
                _LoadContacts();
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while Editing the contact: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                using (frmAddEditContact frm = new frmAddEditContact())
                {
                    frm.ShowDialog();
                }
                _LoadContacts();
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while adding a contact: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {

            if (_dtContacts == null)
                return;

            string text = txtSearch.Text.Trim().Replace("'", "''");

            if (text == "")
            {
                _dtContacts.DefaultView.RowFilter = "";
                return;
            }

            _dtContacts.DefaultView.RowFilter =
                $"FirstName LIKE '%{text}%' OR " +
                $"LastName LIKE '%{text}%' OR " +
                $"Email LIKE '%{text}%' OR " +
                $"Phone LIKE '%{text}%'";
        }

    }
}
