# 📇 Contacts Manager (C# · WinForms · SQL Server · 3-Tier)

A desktop contacts manager built with **C# Windows Forms, ADO.NET and SQL Server**, structured in a clean 3-tier architecture.

**Timeline:** Oct 2026 · **Stack:** C# · .NET Windows Forms · ADO.NET · SQL Server

---

## 🏗️ Architecture

```
┌──────────────────────────┐
│  Presentation Layer (UI) │  frmMain, frmAddEditContact
├──────────────────────────┤
│  Business Layer (BLL)    │  clsContact, clsCountry
├──────────────────────────┤
│  Data Access Layer (DAL) │  clsContactDataAccess, clsCountryDataAccess (ADO.NET)
└──────────────────────────┘
```

| Layer | Responsibility |
|---|---|
| **Presentation** | Forms, user input, displaying data |
| **Business** | Contact/Country objects: Find, Save, Delete, GetAll |
| **Data Access** | Parameterized SQL Server queries over ADO.NET |

## ✨ Features

- **Full CRUD** for contacts, each with photo, country, date of birth and address
- **Live search**: results filter instantly as you type, across first name, last name, email and phone
- **One form for Add & Edit**: a single form that switches mode automatically
- **Country selection**: ComboBox bound to the database (`DisplayMember` / `ValueMember`)
- **Safe confirmations**: Yes/No dialogs default to the safe option before edit or delete
- **Input-safe filtering**: single quotes in the search box are escaped so names like `O'Brien` work

## 🛠️ Technologies

C# · .NET Windows Forms · ADO.NET · SQL Server
