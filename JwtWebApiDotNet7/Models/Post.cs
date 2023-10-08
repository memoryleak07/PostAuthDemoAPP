using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace WebApiDemoApp.Models
{
    public class Post
    {
        [Key] // Primary Key
        public long PostId { get; set; }
        public string? Title { get; set; }
        public string? Body { get; set; }
        [ForeignKey("AuthorId")] // Foreign Key
        [JsonIgnore]
        public User? Author { get; set; }
        public string? AuthorId { get; set; }
        public DateTime Updated { get; set; } = DateTime.Now; // Automatically set datetime

    }
}
//https://github.com/patrickgod/EFCoreRelationshipsTutorial/tree/master/EFCoreRelationshipsTutorial