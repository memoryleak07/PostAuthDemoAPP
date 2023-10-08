using System.ComponentModel.DataAnnotations;

namespace WebApiDemoApp.Models
{
    public class PostDTO
    {
        [Key] // Specify the primary key
        public long PostId { get; set; }
        public string? Title { get; set; }
        public string? Body { get; set; }
        public string? AuthorId { get; set; }
        //public User? Author { get; set; }
        public DateTime Updated { get; set; }

    }
}
