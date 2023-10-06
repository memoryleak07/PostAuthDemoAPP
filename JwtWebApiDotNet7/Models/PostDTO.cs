namespace WebApiDemoApp.Models
{
    public class PostDTO
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public string? Author { get; set; }
        public DateTime Updated { get; set; }

    }
}
