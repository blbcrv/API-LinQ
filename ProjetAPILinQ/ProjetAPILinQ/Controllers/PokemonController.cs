using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using ProjetAPILinQ.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ProjetAPILinQ.Controllers
{
    [Route("api/[controller]")]
    public class PokemonController : Controller
    {
        private readonly IConfiguration _configuration;

        public PokemonController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // GET: api/Pokemon
        [HttpGet]
        public ActionResult<IEnumerable<Pokemon>> Get()
        {
            var pokemons = new List<Pokemon>();
            string connectionString = _configuration.GetConnectionString("DefaultConnection");

            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                var command = new MySqlCommand("SELECT * FROM Pokemon", connection);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var pokemon = new Pokemon
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Name = reader["Name"].ToString(),
                            Type = reader["Type"].ToString(),
                            Attack = reader["Attack"] != DBNull.Value
                             ? reader["Attack"].ToString().Split(',')
                             : new string[0],
                            Image = reader["Img"].ToString()
                        };
                        pokemons.Add(pokemon);
                    }
                }
            }

            return pokemons;
        }

        // GET api/Pokemon/5
        [HttpGet("{id}")]
        public Pokemon Get(int id)
        {
            var pokemon = new Pokemon();
            string connectionString = _configuration.GetConnectionString("DefaultConnection");

            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                var command = new MySqlCommand("SELECT * FROM Pokemon where id="+ id, connection);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var targetPokemon = new Pokemon
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Name = reader["Name"].ToString(),
                            Type = reader["Type"].ToString(),
                            Attack = reader["Attack"] != DBNull.Value
                             ? reader["Attack"].ToString().Split(',')
                             : new string[0],
                            Image = reader["Img"].ToString()
                        };
                        pokemon = targetPokemon;
                    }
                }
            }

            return pokemon;
        }

        // POST api/Pokemon
        [HttpPost]
        public IActionResult Post([FromBody] Pokemon pokemon)
        {
            if (pokemon == null)
            {
                return BadRequest("Pokemon is null.");
            }

            try
            {
                string connectionString = _configuration.GetConnectionString("DefaultConnection");
                using (var connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    var command = new MySqlCommand("INSERT INTO Pokemon (Name, Type, Attack, Img) VALUES (@Name, @Type, @Attack, @Image)", connection);
                    command.Parameters.AddWithValue("@Name", pokemon.Name);
                    command.Parameters.AddWithValue("@Type", pokemon.Type);
                    command.Parameters.AddWithValue("@Attack", String.Join(",", pokemon.Attack));
                    command.Parameters.AddWithValue("@Image", pokemon.Image);

                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }

            return Ok(pokemon);
        }

        // PUT api/Pokemon/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]Pokemon pokemon)
        {
            if (pokemon == null)
            {
                return BadRequest("Pokemon is null.");
            }

            try
            {
                string connectionString = _configuration.GetConnectionString("DefaultConnection");
                using (var connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    var command = new MySqlCommand("UPDATE Pokemon set Name=@Name, Type=@Type, Attack=@Attack, Img=@Image where id=" + id, connection);
                    command.Parameters.AddWithValue("@Name", pokemon.Name);
                    command.Parameters.AddWithValue("@Type", pokemon.Type);
                    command.Parameters.AddWithValue("@Attack", String.Join(",", pokemon.Attack));
                    command.Parameters.AddWithValue("@Image", pokemon.Image);

                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }

            return Ok(pokemon);
        }

        // DELETE api/Pokemon/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var pokemon = new Pokemon();
            string connectionString = _configuration.GetConnectionString("DefaultConnection");

            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                var command = new MySqlCommand("Delete FROM Pokemon where id=" + id, connection);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var targetPokemon = new Pokemon
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Name = reader["Name"].ToString(),
                            Type = reader["Type"].ToString(),
                            Attack = reader["Attack"] != DBNull.Value
                             ? reader["Attack"].ToString().Split(',')
                             : new string[0],
                            Image = reader["Img"].ToString()
                        };
                        pokemon = targetPokemon;
                    }
                }
            }

            return Ok("Pokémon " + id + " à bien été supprimé");
        }
    }
}

