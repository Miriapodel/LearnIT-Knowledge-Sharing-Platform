using LearnIt.Data;
using LearnIt.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LearnIt.Controllers
{
    public class LikesController : Controller
    {
    
        private readonly ApplicationDbContext db;
        private readonly UserManager<MyUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public LikesController(
        ApplicationDbContext context,
        UserManager<MyUser> userManager,
        RoleManager<IdentityRole> roleManager
        )
        {
            db = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        [Authorize (Roles = "User,Admin")]
            public IActionResult ChangeLike(int TopicId, int CommentId)
        {
            if(db.Likes.Include("Comment").Include("MyUser").Where(l=>l.CommentId == CommentId && l.MyUserId == _userManager.GetUserId(User)).Count() == 0)
                {
                db.Comments.Find(CommentId).TotalLikes += 1;
                Like like = new Like();
                like.CommentId = CommentId;
                like.MyUserId = _userManager.GetUserId(User);
                db.Likes.Add(like);
                db.SaveChanges();
                }
            else
            {
                db.Comments.Find(CommentId).TotalLikes -= 1;
                Like like = db.Likes.Include("Comment").Include("MyUser").Where(l => l.CommentId == CommentId && l.MyUserId == _userManager.GetUserId(User)).First();
                db.Remove(like);
                db.SaveChanges();
            }
            return Redirect("/Topics/Show/" + TopicId);
        }
    }
}
