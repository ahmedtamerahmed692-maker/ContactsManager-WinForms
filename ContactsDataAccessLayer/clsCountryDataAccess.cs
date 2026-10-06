using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace ContactsDataAccessLayer
{
    public struct stCountry
    {
        public int CountryID;
        public string CountryName;
        public string Code;
        public string PhoneCode;
    }


    public class clsCountryDataAccess
    {
        public static bool GetCountryByID(int ID, ref stCountry country)
        {
            bool isFound = false;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    string query = "SELECT * FROM Countries WHERE CountryID = @CountryID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@CountryID", ID);
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                isFound = true;
                                country.CountryID = (int)reader["CountryID"];
                                country.CountryName = (string)reader["CountryName"];
                                country.Code = reader["Code"] != DBNull.Value ? (string)reader["Code"] : "";
                                country.PhoneCode = reader["PhoneCode"] != DBNull.Value ? (string)reader["PhoneCode"] : "";
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return isFound;
        }

        public static int AddNewCountry(stCountry countryDTO)
        {
            int newID = -1;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    string query = @"INSERT INTO Countries (CountryName, Code, PhoneCode)
                 VALUES (@CountryName, @Code, @PhoneCode);
                 SELECT SCOPE_IDENTITY();";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@CountryName", countryDTO.CountryName);

                        command.Parameters.AddWithValue("@Code",
                            string.IsNullOrEmpty(countryDTO.Code) ? (object)DBNull.Value : countryDTO.Code);

                        command.Parameters.AddWithValue("@PhoneCode",
                            string.IsNullOrEmpty(countryDTO.PhoneCode) ? (object)DBNull.Value : countryDTO.PhoneCode);

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
                throw;
            }

            return newID;
        }

        public static bool UpdateCountry(stCountry countryDTO)
        {
            int rowsAffected = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    string query = @"UPDATE Countries
                 SET CountryName = @CountryName,
                     Code = @Code,
                     PhoneCode = @PhoneCode
                 WHERE CountryID = @CountryID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@CountryName", countryDTO.CountryName);

                        command.Parameters.AddWithValue("@Code",
                            string.IsNullOrEmpty(countryDTO.Code) ? (object)DBNull.Value : countryDTO.Code);

                        command.Parameters.AddWithValue("@PhoneCode",
                            string.IsNullOrEmpty(countryDTO.PhoneCode) ? (object)DBNull.Value : countryDTO.PhoneCode);

                        command.Parameters.AddWithValue("@CountryID", countryDTO.CountryID);

                        connection.Open();
                        rowsAffected = command.ExecuteNonQuery();
                    }
                }
                
            }
            catch (Exception)
            {
                throw;
            }

            return (rowsAffected > 0);
        }

        public static bool DeleteCountry(int ID)
        {
            int rowsAffected = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    string query = "DELETE FROM Countries WHERE CountryID = @CountryID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@CountryID", ID);
                        connection.Open();
                        rowsAffected = command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return (rowsAffected > 0);
        }

        public static bool DeleteCountries(List<int> IDs)
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

                        command.CommandText = $@"DELETE FROM Countries
                                                 WHERE CountryID IN ({string.Join(", ", parameters)})";

                        connection.Open();
                        rowsAffected = command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return (rowsAffected > 0);
        }

        public static DataTable GetAllCountries()
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    string query = "SELECT * FROM Countries";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        connection.Open();                          

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                dt.Load(reader);
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return dt;
        }

        public static bool GetCountryByName(string countryName, ref stCountry country)
        {
            bool isFound = false;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    string query = "SELECT * FROM Countries WHERE CountryName = @CountryName";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@CountryName", countryName);
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                isFound = true;
                                country.CountryID = (int)reader["CountryID"];
                                country.CountryName = (string)reader["CountryName"];
                                country.Code = reader["Code"] != DBNull.Value ? (string)reader["Code"] : "";
                                country.PhoneCode = reader["PhoneCode"] != DBNull.Value ? (string)reader["PhoneCode"] : "";
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return isFound;
        }
        public static bool IsCountryExist(int ID)
        {
            bool isFound = false;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    string query = "SELECT 1 FROM Countries WHERE CountryID = @CountryID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@CountryID", ID);
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

        public static bool IsCountryExist(string countryName)
        {
            bool isFound = false;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    string query = "SELECT 1 FROM Countries WHERE CountryName = @CountryName";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@CountryName", countryName);
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