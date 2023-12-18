using System.ComponentModel.DataAnnotations;

namespace LearnIt.Models
{
    public class Like
    {
        [Key]
        public int Id { get; set; }
        public int? CommentId { get; set; }
        public string? MyUserId { get; set; }
        public virtual Comment? Comment { get; set; }
        public virtual MyUser? MyUser { get; set; }

    }
}
