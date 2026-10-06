using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms;
using ContactsBusinessLayer;

namespace ContactsWinForms_PresentationLayer
{
    public partial class frmAddEditContact : Form
    {
        public enum enMode { AddNew = 0, Update = 1 };
        private enMode _Mode;

        private clsContact Contact1; 
        public int ID { get; set; }
        private string _ImagePath = "";   // فوق في الكلاس، مع المتغيرات



        public frmAddEditContact() : this(-1)   // ينادي التاني وبيبعتله -1
        {
        }

        public frmAddEditContact(int ID)        // الشغل الحقيقي هنا مرة واحدة بس
        {
            InitializeComponent();
            _StyleForm();
            this.ID = ID;
            _Mode = (ID == -1) ? enMode.AddNew : enMode.Update;

        }

        //التصميم///////////////////////////////////////////
        private void _StyleForm()
        {
            this.BackColor = AppColors.Background;

            // العنوان
            lblTitle.ForeColor = AppColors.Header;

            // كل الـ Labels العادية (حتى اللي جوا Panels)
            _ApplyLabelColor(this);

            _StyleButton(btnSave, AppColors.Header);        // كان Success (الأخضر)، بقى نيلي
            _StyleButton(btnSetImage, AppColors.Header);    // زي ما هو
            _StyleButton(btnClose, AppColors.Neutral);      // زي ما هو
            _StyleButton(btnRemoveImage, AppColors.Danger); // زي ما هو
        }
        private void _StyleButton(Button btn, Color color)
        {
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.BackColor = color;
            btn.ForeColor = Color.White;
            btn.Cursor = Cursors.Hand;
            // مفيش تغيير في الـ Font: الخط بيفضل زي ما صممته في الـ Designer
        }
        private void _ApplyLabelColor(Control parent)
        {
            foreach (Control c in parent.Controls)
            {
                if (c is Label && c != lblTitle)
                    c.ForeColor = AppColors.TextDark;

                if (c.HasChildren)
                    _ApplyLabelColor(c);
            }
        }
        //=====================================================\\
        private void _LoadCountries()
        {
            cmbCountry.DataSource = clsCountry.GetAllCountries();
            cmbCountry.DisplayMember = "CountryName";   // اللي بيظهر
            cmbCountry.ValueMember = "CountryID";       // القيمة الحقيقية
        }

        private void _LoadEditContacts()
        {
            try
            {
                if (_Mode == enMode.AddNew)
                {
                    // إضافة: مفيش بيانات تتحمّل
                    lblTitle.Text = "Add New Contact";
                    lblContactIDValue.Text = "N/A";
                   // Contact1 = new clsContact();
                    return; 
                }

                lblTitle.Text = $"Edit Contact ID = {this.ID}";
                lblContactIDValue.Text = Convert.ToString(this.ID);

                Contact1 = clsContact.Find(ID);
                if (Contact1 != null)
                {
                    txtFirstName.Text = Contact1.FirstName;
                    txtLastName.Text = Contact1.LastName;
                    txtEmail.Text = Contact1.Email;
                    txtPhone.Text = Contact1.Phone;
                    dtpDateOfBirth.Value = Contact1.DateOfBirth;
                    if (Contact1.CountryID > 0)
                    {
                        cmbCountry.SelectedValue = Contact1.CountryID;                    // اختيار بالـ ID مباشرة
                    }
                    else
                    {
                        cmbCountry.SelectedIndex = -1;
                    }
                    txtAddress.Text = Contact1.Address;

                    if (!string.IsNullOrEmpty(Contact1.ImagePath) && File.Exists(Contact1.ImagePath))
                    {
                        _ImagePath = Contact1.ImagePath;
                        picContact.ImageLocation = _ImagePath;   // بدل Image.FromFile
                    }
                    else
                    {
                        _ImagePath = "";
                        picContact.Image = Properties.Resources.Boy;
                    }
                }
                else
                {
                    MessageBox.Show($"Contact [{ID}] Not Found to Update!");

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while deleting the contact: " + ex.Message,
                   "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
            

        private void save()
        {
            DialogResult result = MessageBox.Show("Are you sure you want to Save this contact?",
             "Save Contact", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);

            if (result != DialogResult.Yes)
                return;
            if (Contact1 == null)
            {
                Contact1 = new clsContact();  
            }

            Contact1.FirstName = txtFirstName.Text;
            Contact1.LastName = txtLastName.Text;
            Contact1.Email = txtEmail.Text;
            Contact1.Phone = txtPhone.Text;
            Contact1.DateOfBirth = dtpDateOfBirth.Value;
            Contact1.CountryID = Convert.ToInt32(cmbCountry.SelectedValue);  
            Contact1.ImagePath = _ImagePath;
            if (Contact1.Save())
            {
                MessageBox.Show("Saved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                _Mode = enMode.Update;
                ID = Contact1.ID;
                lblTitle.Text = $"Edit Contact ID = {ID}";
                lblContactIDValue.Text = ID.ToString();
            }
            else
                MessageBox.Show("Failed to save.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void frmAddEditContact_Load(object sender, EventArgs e)
        {
            _LoadCountries();
            _LoadEditContacts();

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            save();

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSetImage_Click(object sender, EventArgs e)
        { 
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";
                dlg.Title = "Select Contact Image";

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    _ImagePath = dlg.FileName;
                    picContact.ImageLocation = _ImagePath;
                }
            }
        }

        private void btnRemoveImage_Click(object sender, EventArgs e)
        {
            _ImagePath = "";
            picContact.Image = Properties.Resources.Boy;  
        }
    
    }
    
}
