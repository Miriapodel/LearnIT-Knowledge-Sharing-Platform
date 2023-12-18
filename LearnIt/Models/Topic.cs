using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LearnIt.Models
{
    public class Topic
    {
        [Key]
        public int Id { get; set; }
        [Required (ErrorMessage = "Discutia trebuie sa aiba un titlu")]
        [MinLength(10 , ErrorMessage = "Titlul trebuie sa aiba minim 10 caractere")]
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DateTime { get; set; }
        public virtual ICollection<Comment>? Comments { get; set; }

        public string? AuthorId { get; set; }
        public virtual MyUser? Author { get; set; }

        public int? CategoryId { get; set; }
        public virtual Category? Category { get; set; }

        [NotMapped]
        public virtual IEnumerable<SelectListItem>? Categ { get; set; }

    }
}
