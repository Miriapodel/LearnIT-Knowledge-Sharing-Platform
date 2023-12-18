using LearnIt.Data;
using LearnIt.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LearnIt.Controllers
{
    public class CommentsController : Controller
    {


        private readonly ApplicationDbContext db;
        private readonly UserManager<MyUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public CommentsController(
        ApplicationDbContext context,
        UserManager<MyUser> userManager,
        RoleManager<IdentityRole> roleManager
        )
        {
            db = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        [Authorize(Roles = "User,Admin")]
        public IActionResult Delete(int id)
        {
            Comment comment = db.Comments.Find(id);
            if (comment.AuthorId == _userManager.GetUserId(User))
            {
                db.Comments.Remove(comment);
                db.SaveChanges();

                
            }
            return Redirect("/Topics/Show/" + comment.TopicId);
        }
        [Authorize(Roles = "User,Admin")]
        public IActionResult Edit(int id)
        {
            Comment comment = db.Comments.Find(id);
            if (comment.AuthorId == _userManager.GetUserId(User))
            {
                return View(comment);
            }
            else
            {
                return Redirect("/Topics/Show/"+comment.TopicId);
            }
        }
        [Authorize(Roles = "User,Admin")]
        [HttpPost]
        public IActionResult Edit(int id, Comment commentEditat)
        {
            Comment comment = db.Comments.Find(id);
            if (comment.AuthorId == _userManager.GetUserId(User))
            {
                if (ModelState.IsValid)
                {
                    comment.DateTime = DateTime.Now;
                    comment.Content = commentEditat.Content;
                    db.SaveChanges();

                    return Redirect("/Topics/Show/" + comment.TopicId);
                }
                else
                {
                    return View(commentEditat);
                }
            }
            else
            {
                return Redirect("/Topics/Show/" + comment.TopicId);
            }
        }
    }
}
