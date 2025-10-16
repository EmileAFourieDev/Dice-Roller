using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace DiceRollerAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DiceRollsController : ControllerBase
    {
        private readonly string _connectionString = "Server=YOUR_PC_NAME\\SQLEXPRESS;Database=DiceDB;Trusted_Connection=True;";

        [HttpPost]
        public IActionResult Post([FromBody] DiceRoll roll)
        {
            using var connection = new SqlConnection(_connectionString);
            connection.Open();

            var command = new SqlCommand(
                "INSERT INTO DiceRolls (Roll1, Roll2, Sum, TimeStamp) VALUES (@r1, @r2, @sum, @time)",
                connection);
            command.Parameters.AddWithValue("@r1", roll.Roll1);
            command.Parameters.AddWithValue("@r2", roll.Roll2);
            command.Parameters.AddWithValue("@sum", roll.Sum);
            command.Parameters.AddWithValue("@time", roll.Timestamp);

            command.ExecuteNonQuery();

            return Ok();
        }
    }

    public class DiceRoll
    {
        public int Roll1 { get; set; }
        public int Roll2 { get; set; }
        public int Sum { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
