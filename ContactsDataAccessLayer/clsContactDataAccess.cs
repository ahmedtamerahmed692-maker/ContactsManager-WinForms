using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
namespace ContactsDataAccessLayer

{
    //هنا هعمل  الاتسلاكشر 
 // تعريف الـ Struct
//انا كنت عامل الاستركشر فيه طبقه البيزنيس ولكن رجعت عملتاه هنا لان الدات لير ملهاش  وصول ليه البيزنيس فمقدرتش تشوف الاستركشر هناك 
    public struct stContact
    {
        public int ID;
        public string FirstName;
        public string LastName;
        public string Email;
        public string Phone;
        public string Address;
        public DateTime DateOfBirth;
        public int CountryID;
        public string ImagePath;
    }

    public class clsContactDataAccess
    {
        //دلوقتي انا عملت داله find فيه ال البريزنتيشن وخلتاه تستقبل متغير رقم 
        //وروحت عملت طبقه التانيه ورحت عمل كلاس كونتيست وظبته وعملت ليه متغيرات وكونرتيمت وعملت الاله نوعها اسم الكلاس وترجه اوبجيكت 
        // لسا هنا اعمل دا داله find في الداتا اكسس وديه هتستقبل رقم وتروح تعمل كويري علي الداتا بيز وتجيبلي الدتا وترجعلي اوبجيكت من الكلاس ده
        //كده عكلت داله البحث فيه ال 3 طبقات لسا الكود اكتبه جواهم بقا 
        //طبعا انا قولت فيه اول طبقتين عملت داله البحث تاخد براميتر ال هو ال id 
        //طب دلوقتي اانا هبعت ال الطبقعه ال 3 ديه بدرو ال id 
        //ولكن هل الايدي بس انا عاوز داتا عشان اردعها لفيند البيسنيس والبينيس يرجعها لليوزر  يعني ممكن اعمل استركشر وابعته كيرفيرنس 

        // استلام الـ struct المباشر بدون كتابة clsContact.
        public static bool GetContactByID(int ID, ref stContact contact)
        {
            bool isFound = false;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    string query = "SELECT * FROM Contacts WHERE ContactID = @ContactID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ContactID", ID);

                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                isFound = true;

                                contact.ID = (int)reader["ContactID"];
                                contact.FirstName = (string)reader["FirstName"];
                                contact.LastName = (string)reader["LastName"];
                                contact.DateOfBirth = (DateTime)reader["DateOfBirth"];
                                contact.CountryID = (int)reader["CountryID"];

                                // معالجة النصوص التي قد تكون NULL في الجدول
                                contact.Email = reader["Email"] != DBNull.Value ? (string)reader["Email"] : "";
                                contact.Phone = reader["Phone"] != DBNull.Value ? (string)reader["Phone"] : "";
                                contact.Address = reader["Address"] != DBNull.Value ? (string)reader["Address"] : "";
                                contact.ImagePath = reader["ImagePath"] != DBNull.Value ? (string)reader["ImagePath"] : "";
                            }
                            else
                            {
                                isFound = false;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw;

            }

            return isFound;
        }


        public static int AddNewContact(stContact contactDTO)
        {
            int newID = -1;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    string query = @"INSERT INTO Contacts (FirstName, LastName, Email, Phone, Address, DateOfBirth, CountryID, ImagePath)
                             VALUES (@FirstName, @LastName, @Email, @Phone, @Address, @DateOfBirth, @CountryID, @ImagePath);
                             SELECT SCOPE_IDENTITY();";
                    using(SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@FirstName", contactDTO.FirstName);
                        command.Parameters.AddWithValue("@LastName", contactDTO.LastName);
                        command.Parameters.AddWithValue("@Email", contactDTO.Email);
                        command.Parameters.AddWithValue("@Phone", contactDTO.Phone);
                        command.Parameters.AddWithValue("@Address", contactDTO.Address);
                        command.Parameters.AddWithValue("@DateOfBirth", contactDTO.DateOfBirth);
                        command.Parameters.AddWithValue("@CountryID", contactDTO.CountryID);
                        command.Parameters.AddWithValue("@ImagePath", string.IsNullOrEmpty(contactDTO.ImagePath) ? (object)DBNull.Value : contactDTO.ImagePath);

                        connection.Open();
                        object result = command.ExecuteScalar();
                        if (result != null && int.TryParse(result.ToString(), out int id))
                        {
                            newID = id;
                        }
                    }
                }
            }
            catch (Exception)
            {
                newID = -1;   
            }
            return newID;     
        }

        public static bool UpdateContact(stContact contactDTO)
        {
            int rowsAffected = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    string query = @"UPDATE Contacts 
                                     SET FirstName = @FirstName, 
                                         LastName = @LastName, 
                                         Email = @Email, 
                                         Phone = @Phone, 
                                         Address = @Address, 
                                         DateOfBirth = @DateOfBirth, 
                                         CountryID = @CountryID, 
                                         ImagePath = @ImagePath
                                     WHERE ContactID = @ContactID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@FirstName", contactDTO.FirstName);
                        command.Parameters.AddWithValue("@LastName", contactDTO.LastName);
                        command.Parameters.AddWithValue("@Email", string.IsNullOrEmpty(contactDTO.Email) ? (object)DBNull.Value : contactDTO.Email);
                        command.Parameters.AddWithValue("@Phone", string.IsNullOrEmpty(contactDTO.Phone) ? (object)DBNull.Value : contactDTO.Phone);
                        command.Parameters.AddWithValue("@Address", string.IsNullOrEmpty(contactDTO.Address) ? (object)DBNull.Value : contactDTO.Address);
                        command.Parameters.AddWithValue("@DateOfBirth", contactDTO.DateOfBirth);
                        command.Parameters.AddWithValue("@CountryID", contactDTO.CountryID);
                        command.Parameters.AddWithValue("@ImagePath", string.IsNullOrEmpty(contactDTO.ImagePath) ? (object)DBNull.Value : contactDTO.ImagePath);
                        command.Parameters.AddWithValue("@ContactID", contactDTO.ID);

                        connection.Open();
                        rowsAffected = command.ExecuteNonQuery();
                    }
                }
            }
            catch
            {
                return false;
            }

            return (rowsAffected > 0); 
        }

        public static bool DeleteContact(int ID)
        {
            int rowsAffected = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    string query = "DELETE FROM Contacts WHERE ContactID = @ContactID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ContactID", ID);

                        connection.Open();
                        rowsAffected = command.ExecuteNonQuery();
                    }
                }
            }
            catch
            {
                return false;
            }

            return (rowsAffected > 0);
        }
        public static bool DeleteContacts(List<int> IDs)
        {
            if (IDs == null || IDs.Count == 0)
                return false;

            int rowsAffected = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    List<string> parameters = new List<string>();

                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Connection = connection;

                        for (int i = 0; i < IDs.Count; i++)
                        {
                            string parameterName = "@ID" + i;
                            parameters.Add(parameterName);

                            command.Parameters.AddWithValue(parameterName, IDs[i]);
                        }

                        string query = $@"
                    DELETE FROM Contacts
                    WHERE ContactID IN ({string.Join(", ", parameters)})";

                        command.CommandText = query;

                        connection.Open();
                        rowsAffected = command.ExecuteNonQuery();
                    }
                }
            }
            catch
            {
                return false;
            }

            return rowsAffected > 0;
        }
        public static DataTable GetAllContacts()
        {
            DataTable datatable = new DataTable();

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    string query = "SELECT * FROM Contacts";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        connection.Open();   // السطر الناقص

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                datatable.Load(reader);
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;   // بدل return null
            }

            return datatable;
        }

        public static bool IsContactExist(int ID)
        {
            bool isFound = false;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    string query = "SELECT 1 FROM Contacts WHERE ContactID = @ContactID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ContactID", ID);
                        connection.Open();
                        isFound = command.ExecuteScalar() != null;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return isFound;
        }
    }
}


    

