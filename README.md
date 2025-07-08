# 🏥 Patient Manager – ASP.NET Core MVC

A complete patient management system built with **ASP.NET Core MVC** and **Onion Architecture**. This application demonstrates multi-role access control, modular design, and real-world CRUD operations, making it ideal for learning enterprise-level web development in .NET.

## 🚀 Features

- 🔐 **Authentication & Registration**  
  Secure login and registration system for administrators with password confirmation and user uniqueness validation.

- 👨‍⚕️ **Role-Based Access**  
  - **Admins** can manage users, doctors, and lab tests.  
  - **Assistants** can manage patients, appointments, and lab results.

- 📋 **Patient Records**  
  Create, edit, and manage detailed patient profiles, including smoking status, allergies, and photo uploads.

- 🧪 **Lab Tests & Results**  
  Assign lab tests during appointments and manage pending/completed results with state transitions.

- 📅 **Appointment System**  
  Schedule, track, and complete patient appointments linked to both patients and doctors.

- 🧠 **Smart Workflow**  
  Appointments flow through logical states: pending → lab test assigned → results → completed.

## 🎯 Purpose

This project aims to:
- Demonstrate how to implement clean architecture with clear separation of concerns.
- Apply **Entity Framework Core** with Code-First approach.
- Practice role-based access, validations, and file upload handling in ASP.NET.
- Deliver a real-world use case of a clinic/consultation management system.

## 🧱 Tech Stack

| Layer            | Tech Stack                            |
|------------------|---------------------------------------|
| Frontend         | ASP.NET Core MVC + Bootstrap          |
| Backend          | ASP.NET Core C#                       |
| Persistence      | Entity Framework Core + SQL Server    |
| Architecture     | Onion Architecture                    |
| Authentication   | Custom login system                   |
| File Handling    | File uploads for patient/doctor photos|

## 📂 Getting Started

1. Clone the repository  
   `git clone https://github.com/WilmeGM/PatientManager.git`

2. Configure the database in `appsettings.example.Developmentjson` or create your own based on.

3. Apply migrations  
   `dotnet ef database update`

4. Run the app  
   `dotnet run`

## 📄 License

This project is licensed under the MIT License – free to use and extend.

---
