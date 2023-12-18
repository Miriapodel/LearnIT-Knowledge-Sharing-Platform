using Ganss.Xss;
using LearnIt.Data;
using LearnIt.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace LearnIt.Controllers
{
    public class TopicsController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly UserManager<MyUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public TopicsController(
        ApplicationDbContext context,
        UserManager<MyUser> userManager,
        RoleManager<IdentityRole> roleManager
        )
        {
            db = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public IActionResult Index()
        {
            int _perPage = 3;

            var topics = db.Topics.Include("Category");

            int totalItems = topics.Count();

            var currentPage = Convert.ToInt32(HttpContext.Request.Query["page"]);

            var offset = 0;

            if(!currentPage.Equals(0))
            {
                offset = (currentPage - 1) * _perPage;
            }

            var paginatedTopics = topics.Skip(offset).Take(_perPage);

            ViewBag.lastPage = Math.Ceiling((float)totalItems / (float)_perPage);

            ViewBag.Topics = paginatedTopics;

            SetAccessRights();

            return View();
        }

        [Authorize(Roles ="User,Admin")]
        public IActionResult New()
        {
            Topic topic = new Topic();

            topic.Categ = GetAllCategories();

            return View(topic);
        }

        [Authorize(Roles = "User,Admin")]
        [HttpPost]
        public IActionResult New(Topic topic) 
        {
            var sanitizer = new HtmlSanitizer();
            topic.DateTime = DateTime.Now;
            topic.AuthorId = _userManager.GetUserId(User);

            if(ModelState.IsValid)
            {
                topic.Description = sanitizer.Sanitize(topic.Description);
                db.Topics.Add(topic);
                db.SaveChanges();
                TempData["Message"] = "Ati creat topicul cu succes";
                return RedirectToAction("Index");
            }
            else
            {
                topic.Categ = GetAllCategories();
                TempData["Message"] = "Nu ati respectat criteriile pentru crearea topicului";
                return View(topic);
            }
        }
        public IActionResult Show(int id, string? filterBy)
        {
            SetAccessRights();
            Topic topic = db.Topics.Include("Comments").Where(topic => topic.Id == id).First();

            switch (filterBy)
            {
                case "TotalLikes":
                    topic.Comments = topic.Comments.OrderByDescending(c => c.TotalLikes).ToList();
                    break;
                case "DateTime":
                    topic.Comments = topic.Comments.OrderByDescending(c => c.DateTime).ToList();
                    break;
                default:
                    topic.Comments = topic.Comments.OrderByDescending(c => c.TotalLikes).ToList();
                    break;
            }

            return View(topic);
                          
        }

        [Authorize(Roles = "User,Admin")]
        [HttpPost]
        public IActionResult Show([FromForm] Comment comment )
        {
            comment.DateTime = DateTime.Now;
            comment.AuthorId = _userManager.GetUserId(User);
            comment.TotalLikes = 0;
            if (ModelState.IsValid)
            {
                db.Comments.Add(comment);

                db.SaveChanges();
                TempData["Message"] = "Ati adaugat comentariul cu succes";
                return Redirect("/Topics/Show/"+comment.TopicId);
            }
            else
            {
                SetAccessRights();
                Topic topic = db.Topics.Include("Comments").Where(c => c.Id == comment.TopicId).First();
                TempData["Message"] = "Adaugarea comentariului nu a reusit";
                return View(topic);
            }

        }
        [Authorize(Roles = "User,Admin")]
        public IActionResult Delete(int id)
        {
            Topic topic = db.Topics.Include("Comments").Where(c => c.Id == id).First();
            if (topic.AuthorId == _userManager.GetUserId(User))
            {
                foreach (var comment in topic.Comments)
                {
                    db.Comments.Remove(comment);
                }

                db.Topics.Remove(topic);
                db.SaveChanges();
                TempData["Message"] = "Ati sters topicul cu succes";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["Message"] = "Topicul nu a putut fi sters";
                return RedirectToAction("Index");
            }
        }

        [Authorize(Roles = "User,Admin")]
        public IActionResult Edit(int id)
        {
            
            Topic topic = db.Topics.Find(id);
            if (topic.AuthorId == _userManager.GetUserId(User) || User.IsInRole("Admin"))
            {
                topic.Categ = GetAllCategories();
                return View(topic);
            }
            else
            {
                return Redirect("/Topics/Show/" + id);
            }
        }

        [Authorize(Roles = "User,Admin")]
        [HttpPost]
        public IActionResult Edit(int id, Topic topicEditat)
        {
            var sanitizer = new HtmlSanitizer();
            Topic topic = db.Topics.Find(id);
            if (topic.AuthorId == _userManager.GetUserId(User))
            {
                if (ModelState.IsValid)
                {
                    topicEditat.Description = sanitizer.Sanitize(topicEditat.Description);
                    topic.DateTime = DateTime.Now;
                    topic.Title = topicEditat.Title;
                    topic.Description = topicEditat.Description;
                    topic.CategoryId = topicEditat.CategoryId;

                    db.SaveChanges();
                    TempData["Message"] = "Comentariul a fost editat cu succes";
                    return Redirect("/Topics/Show/"+id);
                }
                else
                {
                    TempData["Message"] = "Comentariul nu a putut fi editat";
                    return View(topicEditat);
                }

            }
            else
            {
                return Redirect("/Topics/Show/" + id);
            }
        }

        [NonAction]
        public IEnumerable<SelectListItem> GetAllCategories()
        {
            var selectList = new List<SelectListItem>();

            var categories = from category in db.Categories
                             select category;

            foreach (var category in categories)
            {
                selectList.Add(new SelectListItem
                {
                    Value = category.Id.ToString(),
                    Text = category.Name.ToString()
                });
            }

            return selectList;
        }

        private void SetAccessRights()
        {
            ViewBag.AfisareButoane = false;

            if ( User.IsInRole("User") || User.IsInRole("Admin"))
            {
                ViewBag.ELogat = true;
            }

            ViewBag.EsteAdmin = User.IsInRole("Admin");

            ViewBag.UserCurent = _userManager.GetUserId(User);

        }
    }
}
