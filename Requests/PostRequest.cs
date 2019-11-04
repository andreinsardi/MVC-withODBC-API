using System;
namespace MVC_withODBC_API.Requests
{
    public class PostRequest
    {
        public string Title { get; set; }

        public string Text { get; set; }
  
        public int AuthorID { get; set; }
    }
}
