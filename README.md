# EURASIA BUS — ASP.NET Core MVC 10 + MySQL

## Requirements
- .NET 10 SDK 10.0.401
- MySQL 8.0+
- Visual Studio 2022/2026 or VS Code

## Project
- Target Framework: `net10.0`
- ASP.NET Core Identity: `10.0.12`
- Entity Framework Core Design: `10.0.12`
- MySQL Connector/NET EF Core provider: `MySql.EntityFrameworkCore 10.0.9`

The project uses the official MySQL EF Core provider and `UseMySQL(...)`.

## Configure MySQL
Edit `appsettings.json` and set the MySQL password if necessary. The default database is `EurasiaBusDb`.

## Create the database
Open a terminal in the project folder:

```bash
dotnet --version
dotnet restore
dotnet tool install --global dotnet-ef --version 10.*
dotnet ef migrations add InitialCreate
dotnet ef database update
dotnet run
```

If `dotnet-ef` 10.* is already installed, do not install it again.

The application also runs `Database.MigrateAsync()` during startup.

## Administrator
Email: `admin@eurasiabus.com`
Password: `Admin123!`

Change the password immediately for a real deployment.
Скрины:

<img width="1255" height="819" alt="image" src="https://github.com/user-attachments/assets/4261e26f-d9f4-425f-b937-caa125e18206" />

<img width="980" height="876" alt="image" src="https://github.com/user-attachments/assets/b7fc9d32-ce7e-47bc-a7df-bb72f4d849e3" />
<img width="1164" height="747" alt="image" src="https://github.com/user-attachments/assets/8a2fd6fc-8bf0-46c5-bd25-38150cbf69e9" />

<img width="1221" height="653" alt="image" src="https://github.com/user-attachments/assets/e851c902-2f9f-4843-ab73-48168462ebba" />
Полностью рабочая и протестированая версия. Платежные APIs не подключены, имитируются. При необходимости можно подключить. 





