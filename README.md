# RaceDayAPI - PROG6212 Part 2

## Project Overview
RaceDayAPI is a secure, role-based RESTful API built with ASP.NET Core 8. It serves as the backend for a South African sports event management system. The system supports two distinct roles:
- **Organisers**: Can create, update, and delete events, define categories and routes, and capture results.
- **Participants**: Can view events, enroll in specific categories, and view their own results.

## Technologies Used
- **C# / ASP.NET Core 8** (Controllers-based Web API)
- **Entity Framework Core 8** (Code-First approach)
- **SQL Server LocalDB**
- **JWT Bearer Authentication** (JSON Web Tokens)
- **Swagger / OpenAPI** (API Documentation)
- **xUnit & Microsoft.AspNetCore.Mvc.Testing** (Integration Testing)
- **GitHub Actions** (CI/CD Pipeline)

## Prerequisites
- Visual Studio 2022
- .NET 8.0 SDK
- SQL Server LocalDB (installed with Visual Studio)

## Setup and Installation
1. Clone the repository to your local machine.
2. Open the solution (`RaceDay_API.sln`) in Visual Studio 2022.
3. Ensure the `Default project` in the Package Manager Console is set to `RaceDay_API`.
4. Run the following command to create the database:
   ```powershell
   Update-Database