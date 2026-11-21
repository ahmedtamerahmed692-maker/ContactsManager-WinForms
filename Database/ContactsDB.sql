-- =============================================================
-- ContactsDB - schema + seed data for ContactsManager-WinForms
-- Matches the ADO.NET data-access layer in ContactsDataAccessLayer/
-- (tables/columns taken from the actual queries in clsContactDataAccess.cs
--  and clsCountryDataAccess.cs; connection string: Database=ContactsDB)
-- =============================================================

CREATE DATABASE ContactsDB;
GO

USE ContactsDB;
GO

CREATE TABLE Countries (
    CountryID   INT IDENTITY(1,1) PRIMARY KEY,
    CountryName NVARCHAR(100) NOT NULL,
    Code        NVARCHAR(10)  NULL,
    PhoneCode   NVARCHAR(10)  NULL
);
GO

CREATE TABLE Contacts (
    ContactID   INT IDENTITY(1,1) PRIMARY KEY,
    FirstName   NVARCHAR(50)  NOT NULL,
    LastName    NVARCHAR(50)  NOT NULL,
    Email       NVARCHAR(100) NULL,
    Phone       NVARCHAR(20)  NULL,
    Address     NVARCHAR(200) NULL,
    DateOfBirth DATE          NULL,
    CountryID   INT           NULL,
    ImagePath   NVARCHAR(500) NULL,
    CONSTRAINT FK_Contacts_Countries
        FOREIGN KEY (CountryID) REFERENCES Countries(CountryID)
);
GO

-- Seed countries
INSERT INTO Countries (CountryName, Code, PhoneCode) VALUES
(N'Egypt',        N'EG', N'+20'),
(N'Saudi Arabia', N'SA', N'+966'),
(N'UAE',          N'AE', N'+971'),
(N'USA',          N'US', N'+1'),
(N'UK',           N'GB', N'+44');
GO

-- Seed sample contacts
INSERT INTO Contacts (FirstName, LastName, Email, Phone, Address, DateOfBirth, CountryID, ImagePath) VALUES
(N'Ahmed', N'Tamer',  N'ahmed@example.com', N'01000000000', N'Mansoura, Egypt', '2006-09-08', 1, NULL),
(N'Sara',  N'Ali',    N'sara@example.com',  N'0500000000',  N'Riyadh, KSA',     '1999-03-15', 2, NULL),
(N'Omar',  N'Hassan', N'omar@example.com',  N'0400000000',  N'Dubai, UAE',      '2001-07-22', 3, NULL);
GO
