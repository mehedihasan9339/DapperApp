using DapperApp.Services.Dapper.SQLDapper.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DapperApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly IDapper _dapper;
        private readonly IConfiguration _config;

        public HomeController(IDapper dapper, IConfiguration config)
        {
            _dapper = dapper;
            _config = config;
        }

        [HttpGet("/api/GetAllUsers")]
        public async Task<IEnumerable<Users>> GetAllUsers()
        {
            try
            {
                string databaseUsed = _config["DatabaseUsed"];
                FormattableString sql;

                if (databaseUsed == "SQL")
                {
                    sql = $"EXEC GetAllUsers";
                }
                else if (databaseUsed == "POSTGRES")
                {
                    sql = $"SELECT * FROM GetAllUsers()";
                }
                else
                {
                    throw new InvalidOperationException("Unsupported database type.");
                }

                var data = await _dapper.FromSqlAsync<Users>(sql);
                return data;
            }
            catch (Exception)
            {
                throw; // Optionally log the error
            }
        }

        public class Users
        {
            public string Username { get; set; }
            public string Phone { get; set; }
            public string Email { get; set; }
        }
    }
}
