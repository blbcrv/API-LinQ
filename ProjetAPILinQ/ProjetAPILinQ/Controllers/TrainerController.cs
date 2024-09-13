using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using NuGet.DependencyResolver;
using ProjetAPILinQ.Models;

namespace ProjetAPILinQ.Controllers
{
    [Route("api/[controller]")]
    public class TrainerController : Controller
    {
        private readonly IConfiguration _configuration;

        public TrainerController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Trainer>> Get()
        {
            var trainers = new List<Trainer>();
            string connectionString = _configuration.GetConnectionString("DefaultConnection");

            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                var command = new MySqlCommand("SELECT * FROM Trainer", connection);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var trainer = new Trainer
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Name = reader["Name"].ToString(),
                            Age = Convert.ToInt32(reader["Age"]),
                            Region = reader["Region"].ToString()
                        };
                        trainers.Add(trainer);
                    }
                }
            }

            return trainers;
        }

        [HttpGet("{id}")]
        public Trainer Get(int id)
        {
            var trainer = new Trainer();
            string connectionString = _configuration.GetConnectionString("DefaultConnection");

            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                var command = new MySqlCommand("SELECT * FROM Trainer WHERE Id=" + id, connection);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        trainer = new Trainer
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Name = reader["Name"].ToString(),
                            Age = Convert.ToInt32(reader["Age"]),
                            Region = reader["Region"].ToString()
                        };
                    }
                }
            }

            return trainer;
        }

        [HttpPost]
        public IActionResult Post([FromBody] Trainer trainer)
        {
            if (trainer == null)
            {
                return BadRequest("Trainer is null.");
            }

            try
            {
                string connectionString = _configuration.GetConnectionString("DefaultConnection");
                using (var connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    var command = new MySqlCommand("INSERT INTO Trainer (Name, Age, Region) VALUES (@Name, @Age, @Region)", connection);
                    command.Parameters.AddWithValue("@Name", trainer.Name);
                    command.Parameters.AddWithValue("@Age", trainer.Age);
                    command.Parameters.AddWithValue("@Region", trainer.Region);

                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }

            return Ok(trainer);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Trainer trainer)
        {
            if (trainer == null)
            {
                return BadRequest("Trainer is null.");
            }

            try
            {
                string connectionString = _configuration.GetConnectionString("DefaultConnection");
                using (var connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    var command = new MySqlCommand("UPDATE Trainer SET Name=@Name, Age=@Age, Region=@Region WHERE Id=" + id, connection);
                    command.Parameters.AddWithValue("@Name", trainer.Name);
                    command.Parameters.AddWithValue("@Age", trainer.Age);
                    command.Parameters.AddWithValue("@Region", trainer.Region);

                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }

            return Ok(trainer);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            string connectionString = _configuration.GetConnectionString("DefaultConnection");

            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                var command = new MySqlCommand("DELETE FROM Trainer WHERE Id=" + id, connection);
                command.ExecuteNonQuery();
            }

            return Ok("Trainer " + id + " has been deleted successfully.");
        }
    }
}
