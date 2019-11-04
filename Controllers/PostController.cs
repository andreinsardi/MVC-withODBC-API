using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MVC_withODBC_API.Requests;
using MVC_withODBC_API.Results;
using MySql.Data.MySqlClient;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MVC_withODBC_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : Controller
    {
        private readonly Appsettings _appSettings;

        public PostController(Appsettings appSettings)
        {
            _appSettings = appSettings;
        }

        [HttpGet]
        public IActionResult Get()
        {
            MySqlConnection conn = new MySqlConnection(_appSettings.ConnectionString);
            List<PostResult> result = new List<PostResult>();

            try
            {
                conn.Open();

                using (MySqlCommand cmd = new MySqlCommand("SELECT PostID, Title,Text,Created,AuthorID FROM Post", conn))
                {
                    MySqlDataReader dataReader = cmd.ExecuteReader();
                    while (dataReader.Read())
                    {
                        result.Add(new PostResult
                        {
                            PostID = dataReader.GetInt32(0),
                            Title = dataReader.GetString(1),
                            Text = dataReader.GetString(2),
                            Created = dataReader.GetDateTime(3),
                            AuthorID = dataReader.GetInt32(4) 
                        });
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

        [HttpGet("{PostID}")]
        public IActionResult Get(int PostID)
        {
            MySqlConnection conn = new MySqlConnection(_appSettings.ConnectionString);
            PostResult result = new PostResult();

            try
            {
                conn.Open();

                using (MySqlCommand cmd = new MySqlCommand("SELECT  PostID, Title,Text,Created,AuthorID FROM Post WHERE PostID=" + PostID, conn))
                {
                    MySqlDataReader dataReader = cmd.ExecuteReader();
                    if (dataReader.Read())
                    {
                        result = new PostResult
                        {
                            PostID = dataReader.GetInt32(0),
                            Title = dataReader.GetString(1),
                            Text = dataReader.GetString(2),
                            Created = dataReader.GetDateTime(3),
                            AuthorID = dataReader.GetInt32(4)
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
        public IActionResult Post([FromBody]PostRequest request)
        {
            MySqlConnection conn = new MySqlConnection(_appSettings.ConnectionString);
            int postID = 0;

            try
            {
                conn.Open();

                using (MySqlCommand cmd = new MySqlCommand("INSERT INTO Post (Title,Text,Created,AuthorID) VALUES(@title,@text,@created,@authorID)", conn))
                {

                    cmd.Parameters.AddWithValue("@title", request.Title);
                    cmd.Parameters.AddWithValue("@text", request.Text);
                    cmd.Parameters.AddWithValue("@created",  DateTime.Now);
                    cmd.Parameters.AddWithValue("@authorID", request.AuthorID);

                    cmd.ExecuteNonQuery();

                    using (MySqlCommand cmd2 = new MySqlCommand("SELECT last_insert_id()", conn))
                    {
                        postID = (int)(ulong)cmd2.ExecuteScalar();
                    }

                }

                return new OkObjectResult(new PostResult { PostID = postID, Title = request.Title, Text =request.Text, AuthorID = request.AuthorID, Created = DateTime.Now });

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

        [HttpPut("{PostID}")]
        public IActionResult Put(int PostID, [FromBody]PostRequest request)
        {
            MySqlConnection conn = new MySqlConnection(_appSettings.ConnectionString);

            try
            {
                conn.Open();

                using (MySqlCommand cmd = new MySqlCommand("UPDATE Post SET Title = @title, Text=@text  WHERE PostID = @postID", conn))
                {

                    cmd.Parameters.AddWithValue("@postID", PostID);
                    cmd.Parameters.AddWithValue("@title", request.Title);
                    cmd.Parameters.AddWithValue("@text", request.Text);

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

        [HttpDelete("{PostID}")]
        public IActionResult Delete(int PostID)
        {
            MySqlConnection conn = new MySqlConnection(_appSettings.ConnectionString);

            try
            {
                conn.Open();

                using (MySqlCommand cmd = new MySqlCommand("DELETE FROM Post  WHERE PostID = @postID", conn))
                {

                    cmd.Parameters.AddWithValue("@postID", PostID); 

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
