using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using ContactsDataAccessLayer;
using System.Data;

namespace ContactsBusinessLayer
{
    public class clsContact
    {
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;
        public int ID { set; get; }
        public string FirstName { set; get; }
        public string LastName { set; get; }
        public string Email { set; get; }
        public string Phone { set; get; }
        public string Address { set; get; }
        public DateTime DateOfBirth { set; get; }

        public string ImagePath { set; get; }

        public int CountryID { set; get; }

        public clsContact()

        {
            this.ID = -1;
            this.FirstName = "";
            this.LastName = "";
            this.Email = "";
            this.Phone = "";
            this.Address = "";
            this.DateOfBirth = DateTime.Now;
            this.CountryID = -1;
            this.ImagePath = "";
            Mode = enMode.AddNew; // تعيين الحالة كـ إضافة
        }

        private clsContact(int ID, string FirstName, string LastName,
            string Email, string Phone, string Address, DateTime DateOfBirth, int CountryID, string ImagePath)

        {
            this.ID = ID;
            this.FirstName = FirstName;
            this.LastName = LastName;
            this.Email = Email;
            this.Phone = Phone;
            this.Address = Address;
            this.DateOfBirth = DateOfBirth;
            this.CountryID = CountryID;
            this.ImagePath = ImagePath;
            Mode = enMode.Update;
        }

        //اول حاجه فكرت فيها انا عاوز ارجع لليوز داتا ففكرنا نرجعها عن طريق الداتا تيبل بس انا عاوز ارجعها كلاس ففكرت اني اعمل فانكشن ترجعلي الكلاس ده من الداتا تيبل
        //وطبعا اليوز ال هو طبقه البريزنتشن بيتعامل فقط مع البيزنس لزك اليوز محتاج ستقبل اوبجيكت فيه دتا فعلمنا فيه البيزنس لير اوبجيكت يرجع الداتا 
        //طبعا انا معنديش اي فكره هكتب كود ايه هنا فنا اقولت هروح انشيء الداله بس حاليا فيه ال 3 طبقات وبعدين افكر فيه الكود مش عارف ازا كان صح انا اول مره اعمل حاجه زي ديه 

        public static clsContact Find(int ID)
        {
            if (ID <= 0)
                return null;

            // إنشاء الـ Struct من طبقة البيانات
            stContact contactDTO = new stContact();

            if (clsContactDataAccess.GetContactByID(ID, ref contactDTO))
            {
                return new clsContact(
                    contactDTO.ID,
                    contactDTO.FirstName,
                    contactDTO.LastName,
                    contactDTO.Email,
                    contactDTO.Phone,
                    contactDTO.Address,
                    contactDTO.DateOfBirth,
                    contactDTO.CountryID,
                    contactDTO.ImagePath
                );
            }
            else
            {
                return null;
            }
        }
        private bool _AddNewContact(stContact contactDTO)
        {
            this.ID = clsContactDataAccess.AddNewContact(contactDTO);
            if (this.ID != -1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private bool _UpdateContact(stContact contactDTO1)
        {
            return clsContactDataAccess.UpdateContact(contactDTO1);
        }

        public bool Save()
        {
            stContact contactDTO = new stContact
            {
                ID = this.ID,
                FirstName = this.FirstName,
                LastName = this.LastName,
                Email = this.Email,
                Phone = this.Phone,
                Address = this.Address,
                DateOfBirth = this.DateOfBirth,
                CountryID = this.CountryID,
                ImagePath = this.ImagePath
            };

            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewContact(contactDTO))
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    return false;

                case enMode.Update:
                    return _UpdateContact(contactDTO);

                default:
                    return false;
            }
        }

        public static bool DeleteContact(int ID)
        {
            if (ID <= 0)
                return false;

            return clsContactDataAccess.DeleteContact(ID);
        }
        public static bool DeleteContacts(List<int> IDs)
        {
            if (IDs == null || IDs.Count == 0)
                return false;

            if (IDs.Any(id => id <= 0))
                return false;

            return clsContactDataAccess.DeleteContacts(IDs);
        }

        public static DataTable GetAllContacts()
        {
            return clsContactDataAccess.GetAllContacts();
        }

        public static bool IsContactExist(int ID)
        {
            if (ID <= 0)
                return false;

            return clsContactDataAccess.IsContactExist(ID);
        }
    }
}
