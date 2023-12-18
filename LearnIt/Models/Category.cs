

using System.ComponentModel.DataAnnotations;

namespace LearnIt.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required (ErrorMessage = "Categoria trebuie sa aiba un nume")]
        [MinLength (3 , ErrorMessage = "Numele trebuie sa aiba minim 3 caractere")]
        public string Name { get; set; }

        public virtual ICollection<Topic>? Topics { get; set; }
    }
}
