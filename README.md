# DapperApp - Database Setup and Operations

## Use SQL Server and PostgreSQL in Same Project with Dapper to Call Stored Procedures

This project demonstrates how to use Dapper to interact with both SQL Server and PostgreSQL in the same application, handling stored procedures in SQL Server and functions in PostgreSQL. It also includes the steps for adding migrations and applying them to both databases.

### Packages Required

1. **Dapper**: An object-relational mapper (ORM) that allows you to interact with your database in a more efficient and less boilerplate way.
   ```bash
   dotnet add package Dapper
   ```

2. **Npgsql**: PostgreSQL data provider for .NET to connect with PostgreSQL databases.

   ```bash
   dotnet add package Npgsql
   ```

3. **Microsoft.EntityFrameworkCore.SqlServer**: A provider to use SQL Server with Entity Framework Core.

   ```bash
   dotnet add package Microsoft.EntityFrameworkCore.SqlServer
   ```

4. **Microsoft.EntityFrameworkCore.Tools**: Tools for working with migrations and database updates.

   ```bash
   dotnet add package Microsoft.EntityFrameworkCore.Tools
   ```

---

### Project Setup

### 1. **Add Initial Migration for SQL Server**
```bash
dotnet ef migrations add InitialSqlServer --context SqlServerDbContext --output-dir Migrations/SqlServer --project .\DapperApp\DapperApp.csproj
````

### 2. **Add Initial Migration for PostgreSQL**

```bash
dotnet ef migrations add InitialPostgres --context PostgresDbContext --output-dir Migrations/Postgres --project .\DapperApp\DapperApp.csproj
```

### 3. **Apply Migrations to PostgreSQL Database**

```bash
dotnet ef database update --context PostgresDbContext --project .\DapperApp\DapperApp.csproj
```

### 4. **Apply Migrations to SQL Server Database**

```bash
dotnet ef database update --context SqlServerDbContext --project .\DapperApp\DapperApp.csproj
```

---

### 5. **Enable `uuid-ossp` Extension in PostgreSQL**

```sql
CREATE EXTENSION IF NOT EXISTS "uuid-ossp";
```

### 6. **Insert Sample Users into PostgreSQL**

```sql
INSERT INTO "AspNetUsers"
           ("Id", "UserName", "NormalizedUserName", "Email", "NormalizedEmail", "EmailConfirmed", "PasswordHash", "SecurityStamp", "ConcurrencyStamp", "PhoneNumber", "PhoneNumberConfirmed", "TwoFactorEnabled", "LockoutEnd", "LockoutEnabled", "AccessFailedCount")
     VALUES
           (uuid_generate_v4(), 'user1', 'USER1', 'user1@example.com', 'USER1@EXAMPLE.COM', true, 'hashedpassword1', 'securitystamp1', 'concurrencystamp1', '123-456-7890', true, false, NULL, true, 0),
           (uuid_generate_v4(), 'user2', 'USER2', 'user2@example.com', 'USER2@EXAMPLE.COM', true, 'hashedpassword2', 'securitystamp2', 'concurrencystamp2', '234-567-8901', true, false, NULL, true, 0),
           (uuid_generate_v4(), 'user3', 'USER3', 'user3@example.com', 'USER3@EXAMPLE.COM', true, 'hashedpassword3', 'securitystamp3', 'concurrencystamp3', '345-678-9012', true, false, NULL, true, 0),
           (uuid_generate_v4(), 'user4', 'USER4', 'user4@example.com', 'USER4@EXAMPLE.COM', true, 'hashedpassword4', 'securitystamp4', 'concurrencystamp4', '456-789-0123', true, false, NULL, true, 0),
           (uuid_generate_v4(), 'user5', 'USER5', 'user5@example.com', 'USER5@EXAMPLE.COM', true, 'hashedpassword5', 'securitystamp5', 'concurrencystamp5', '567-890-1234', true, false, NULL, true, 0);
```

### 7. **Create Function `GetAllUsers` in PostgreSQL**

```sql
CREATE OR REPLACE FUNCTION GetAllUsers()
RETURNS TABLE (Username TEXT, Phone TEXT, Email TEXT) AS
$$
BEGIN
    RETURN QUERY
    SELECT 
        "UserName"::TEXT, 
        "PhoneNumber"::TEXT AS "Phone", 
        "Email"::TEXT
    FROM "AspNetUsers";
END;
$$ LANGUAGE plpgsql;
```

### 8. **Call `GetAllUsers` Function in PostgreSQL**

```sql
SELECT * FROM GetAllUsers();
```

---

### 9. **Create Stored Procedure `GetAllUsers` in SQL Server**

```sql
CREATE PROCEDURE GetAllUsers
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        Username, 
        PhoneNumber as Phone, 
        Email
    FROM AspNetUsers;
END
```

### 10. **Insert Sample Users into SQL Server**

```sql
INSERT INTO [dbo].[AspNetUsers]
           ([Id], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount])
     VALUES
           (NEWID(), 'user1', 'USER1', 'user1@example.com', 'USER1@EXAMPLE.COM', 1, 'hashedpassword1', 'securitystamp1', 'concurrencystamp1', '123-456-7890', 1, 0, NULL, 1, 0),
           (NEWID(), 'user2', 'USER2', 'user2@example.com', 'USER2@EXAMPLE.COM', 1, 'hashedpassword2', 'securitystamp2', 'concurrencystamp2', '234-567-8901', 1, 0, NULL, 1, 0),
           (NEWID(), 'user3', 'USER3', 'user3@example.com', 'USER3@EXAMPLE.COM', 1, 'hashedpassword3', 'securitystamp3', 'concurrencystamp3', '345-678-9012', 1, 0, NULL, 1, 0),
           (NEWID(), 'user4', 'USER4', 'user4@example.com', 'USER4@EXAMPLE.COM', 1, 'hashedpassword4', 'securitystamp4', 'concurrencystamp4', '456-789-0123', 1, 0, NULL, 1, 0),
           (NEWID(), 'user5', 'USER5', 'user5@example.com', 'USER5@EXAMPLE.COM', 1, 'hashedpassword5', 'securitystamp5', 'concurrencystamp5', '567-890-1234', 1, 0, NULL, 1, 0);
```

### 11. **Execute `GetAllUsers` Stored Procedure in SQL Server**

```sql
EXEC GetAllUsers;
```


### Conclusion

* **SQL Server**: You can use **stored procedures** directly to retrieve data.
* **PostgreSQL**: Since **stored procedures** are not directly supported, **functions** are used to return data.

By following these steps, you can successfully interact with both databases using Dapper and manage your data through stored procedures in SQL Server and functions in PostgreSQL.
