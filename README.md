# RWAEShop

RWAEShop is a simple e-commerce web application built using **ASP.NET Core MVC (.NET 8)**.  
The project demonstrates a standard MVC architecture with separated layers for data access, business logic, and presentation.

---

## Technologies Used

- .NET 8 (ASP.NET Core MVC)
- C#
- Razor Views
- Entity Framework Core
- HTML, CSS, Bootstrap
- Dependency Injection
- Layered architecture (DAL, Web, API)

---

## Project Structure

/
├─ RWAEShopWebApp/ # MVC Web Application
├─ RWAEShopWebApi/ # Web API layer
├─ RWAEshopDAL/ # Data Access Layer
├─ RWAEShop.sln # Solution file
├─ .gitignore
└─ README.md


---

## Features

- Product listing
- Product details view
- Basic shopping cart functionality
- MVC-based architecture
- Separation of concerns using layers
- Razor view templates

---

## Getting Started

### Prerequisites

- .NET 8 SDK
- Visual Studio or Visual Studio Code

---

### Running the Application

1. Clone the repository:
   ```bash
   git clone https://github.com/IvanPetar/RWAEShop.git
Navigate to the project directory:

cd RWAEShop
Build the solution:

dotnet build
Run the MVC web application:

cd RWAEShopWebApp
dotnet run
Open a browser and navigate to:

http://localhost:5000
Notes
Database connection strings and configuration can be adjusted in appsettings.json.

This project is intended for educational and demonstration purposes.

The application can be extended with authentication, authorization, and payment processing.

License
This project does not currently specify a license.
If you plan to reuse or distribute it, consider adding an appropriate license.