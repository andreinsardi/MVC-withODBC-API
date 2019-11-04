using System;
namespace MVC_withODBC_API.Results
{
    public class PostResult
    {
        public int PostID { get; set; }

        public string Title { get; set; }

        public string Text { get; set; }

        public DateTime Created { get; set; }

        public int AuthorID { get; set; }
 
    }
}
