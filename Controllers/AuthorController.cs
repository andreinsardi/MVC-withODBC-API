using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MVC_withODBC_API.Requests;
using MVC_withODBC_API.Results;
using MySql.Data.MySqlClient;

namespace MVC_withODBC_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorController : ControllerBase
    {
        private readonly Appsettings _appSettings;

        public AuthorController(Appsettings appSettings )
        {
            _appSettings = appSettings;  
        }

        [HttpGet]
        public IActionResult Get()
        {
            MySqlConnection conn = new MySqlConnection(_appSettings.ConnectionString);
            List<AuthorResult> result = new List<AuthorResult>();

            try
            {
                conn.Open();

                using (MySqlCommand cmd = new MySqlCommand("SELECT AuthorID, Name FROM Author", conn))
                {
                    MySqlDataReader dataReader = cmd.ExecuteReader();
                    while (dataReader.Read())
                    {
                        result.Add(new AuthorResult
                        {
                            AuthorID = dataReader.GetInt32(0),
                            Name = dataReader.GetString(1)
                        }); 
                    }
                }

                return new OkObjectResult(result);

            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
            finally {
                conn.Dispose();
                conn.Close();
            }
 
        }

        [HttpGet("{AuthorID}")]
        public IActionResult Get(int AuthorID)
        {
            MySqlConnection conn = new MySqlConnection(_appSettings.ConnectionString);
            AuthorResult result = new  AuthorResult();

            try
            {
                conn.Open();

                using (MySqlCommand cmd = new MySqlCommand("SELECT AuthorID, Name FROM Author WHERE AuthorID=" + AuthorID, conn))
                {
                    MySqlDataReader dataReader = cmd.ExecuteReader();
                    if (dataReader.Read())
                    {
                        result = new AuthorResult
                        {
                            AuthorID = dataReader.GetInt32(0),
                            Name = dataReader.GetString(1)
                        };
                    }
                }

                return new OkObjectResult(result);

            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
            finally
            {
                conn.Dispose();
                conn.Close();
            }

        }

        [HttpPost]
        public IActionResult Post([FromBody]AuthorRequest request)
        {
            MySqlConnection conn = new MySqlConnection(_appSettings.ConnectionString);
            int authorID = 0;

            try
            {
                conn.Open();

                using (MySqlCommand cmd = new MySqlCommand("INSERT INTO Author (Name) VALUES(@name)", conn))
                {

                    cmd.Parameters.AddWithValue("@name", request.Name);

                    cmd.ExecuteNonQuery();

                    using (MySqlCommand cmd2 = new MySqlCommand("SELECT last_insert_id()", conn))
                    {
                        authorID = (int)(ulong)cmd2.ExecuteScalar();
                    }

                }

                return new OkObjectResult(new AuthorResult {  AuthorID = authorID , Name = request.Name});

            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
            finally
            {
                conn.Dispose();
                conn.Close();
            }

        }

        [HttpPut("{AuthorID}")]
        public IActionResult Put(int AuthorID, [FromBody]AuthorRequest request)
        {
            MySqlConnection conn = new MySqlConnection(_appSettings.ConnectionString);
         
            try
            {
                conn.Open();

                using (MySqlCommand cmd = new MySqlCommand("UPDATE Author SET Name = @name WHERE AuthorID = @authorID", conn))
                {

                    cmd.Parameters.AddWithValue("@name", request.Name);
                    cmd.Parameters.AddWithValue("@authorID", AuthorID);

                    cmd.ExecuteNonQuery();
 
                }

                return new OkObjectResult(new AuthorResult { AuthorID = AuthorID, Name = request.Name });

            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
            finally
            {
                conn.Dispose();
                conn.Close();
            }

        }

        [HttpDelete("{AuthorID}")]
        public IActionResult Delete(int AuthorID)
        {
            MySqlConnection conn = new MySqlConnection(_appSettings.ConnectionString);

            try
            {
                conn.Open();

                using (MySqlCommand cmd = new MySqlCommand("DELETE FROM Author WHERE AuthorID = @authorID", conn))
                {
  
                    cmd.Parameters.AddWithValue("@authorID", AuthorID);

                    cmd.ExecuteNonQuery();

                }

                return new OkResult();

            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
            finally
            {
                conn.Dispose();
                conn.Close();
            }

        }
    }
}
