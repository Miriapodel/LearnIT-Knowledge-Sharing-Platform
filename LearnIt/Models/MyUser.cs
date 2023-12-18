using Microsoft.AspNetCore.Identity;

namespace LearnIt.Models
{
    public class MyUser : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        public virtual ICollection<Comment>? Comments { get; set; }
        public virtual ICollection<Topic>? Topics { get; set; }
        public virtual ICollection<Like>? Likes { get; set; }



    }
}
