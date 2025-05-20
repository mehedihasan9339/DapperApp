using DapperApp.Context;
using DapperApp.Services.Dapper.SQLDapper.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var databaseUsed = builder.Configuration["DatabaseUsed"];

if (databaseUsed == "SQL")
{
    // For SQL Server
    builder.Services.AddDbContext<SqlServerDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer")));
}
else if (databaseUsed == "POSTGRES")
{
    // For PostgreSQL
    builder.Services.AddDbContext<PostgresDbContext>(options =>
        options.UseNpgsql(builder.Configuration.GetConnectionString("Postgres")));

}



// Register IHttpContextAccessor (required)
builder.Services.AddHttpContextAccessor();





#region Dapper
builder.Services.AddScoped<IDapper, DapperApp.Services.Dapper.SQLDapper.Dapper>();
#endregion


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
