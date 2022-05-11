using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SQL_Injection.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public HomeController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        // GET: api/<HomeController>
        [HttpGet]
        public IActionResult Get(string firstName)
        {
            DataTable dt = new DataTable();
            SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            conn.Open();
            // Ex -> firstName = 'Guy ;DELETE FROM DimEmployee'

            //Bad
            //SqlCommand command = new SqlCommand($"SELECT TOP(10) * FROM DimEmployee WHERE FirstName='{firstName}'", conn);

            //Good
            SqlCommand command = new SqlCommand("SELECT TOP(10) * FROM DimEmployee WHERE FirstName=@firstName", conn);
            command.Parameters.Add("@firstName",SqlDbType.VarChar);
            command.Parameters["@firstName"].Value = firstName;

            using (SqlDataReader reader = command.ExecuteReader())
            {
                dt.Load(reader);
            }
            return Ok();
        }

    }
}
