using System.ComponentModel.DataAnnotations;

namespace LearnIt.Models
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Comentariul trebuie sa aiba un continut")]
        
        public string Content { get; set; }
        public DateTime DateTime { get; set; }
        public virtual ICollection<Like>? Likes { get; set; }
        public int? TotalLikes { get; set; }



        public string? AuthorId { get; set; }
        public virtual MyUser? Author { get; set; }
        public int? TopicId { get; set; }
        public virtual Topic? Topic { get; set; }
    }
}
