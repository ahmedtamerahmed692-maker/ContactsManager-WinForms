using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using ContactsDataAccessLayer;

namespace ContactsBusinessLayer
{
    public class clsCountry
    {
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public int CountryID { set; get; }
        public string CountryName { set; get; }
        public string Code { set; get; }
        public string PhoneCode { set; get; }

        public clsCountry()
        {
            this.CountryID = -1;
            this.CountryName = "";
            this.Code = "";
            this.PhoneCode = "";
            Mode = enMode.AddNew;
        }

        private clsCountry(int CountryID, string CountryName, string Code, string PhoneCode)
        {
            this.CountryID = CountryID;
            this.CountryName = CountryName;
            this.Code = Code;
            this.PhoneCode = PhoneCode;
            Mode = enMode.Update;
        }

        public static clsCountry Find(int ID)
        {
            if (ID <= 0)
                return null;

            stCountry countryDTO = new stCountry();

            if (clsCountryDataAccess.GetCountryByID(ID, ref countryDTO))
            {
                return new clsCountry(countryDTO.CountryID, countryDTO.CountryName, countryDTO.Code, countryDTO.PhoneCode);
            }

            return null;
        }

        private bool _AddNewCountry(stCountry countryDTO)
        {
            this.CountryID = clsCountryDataAccess.AddNewCountry(countryDTO);
            return (this.CountryID != -1);
        }

        private bool _UpdateCountry(stCountry countryDTO)
        {
            return clsCountryDataAccess.UpdateCountry(countryDTO);
        }

        public bool Save()
        {
            stCountry countryDTO = new stCountry
            {
                CountryID = this.CountryID,
                CountryName = this.CountryName,
                Code=this.Code,
                PhoneCode=this.PhoneCode
            };

            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewCountry(countryDTO))
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    return false;

                case enMode.Update:
                    return _UpdateCountry(countryDTO);

                default:
                    return false;
            }
        }

        public static bool DeleteCountry(int ID)
        {
            if (ID <= 0)
                return false;

            return clsCountryDataAccess.DeleteCountry(ID);
        }

        public static bool DeleteCountries(List<int> IDs)
        {
            if (IDs == null || IDs.Count == 0)
                return false;

            if (IDs.Any(id => id <= 0))
                return false;

            return clsCountryDataAccess.DeleteCountries(IDs);
        }

        public static DataTable GetAllCountries()
        {
            return clsCountryDataAccess.GetAllCountries();
        }
        public static bool IsCountryExist(int ID)
        {
            if (ID <= 0)
                return false;

            return clsCountryDataAccess.IsCountryExist(ID);
        }

        public static clsCountry Find(string countryName)
        {
            if (string.IsNullOrWhiteSpace(countryName))
                return null;

            stCountry countryDTO = new stCountry();

            if (clsCountryDataAccess.GetCountryByName(countryName.Trim(), ref countryDTO))
            {
                return new clsCountry(countryDTO.CountryID, countryDTO.CountryName, countryDTO.Code, countryDTO.PhoneCode);
            }

            return null;
        }

        public static bool IsCountryExist(string countryName)
        {
            if (string.IsNullOrWhiteSpace(countryName))
                return false;

            return clsCountryDataAccess.IsCountryExist(countryName.Trim());
        }
    }
}