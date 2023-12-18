using LearnIt.Data;
using LearnIt.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LearnIt.Controllers
{

    public class CategoriesController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly UserManager<MyUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public CategoriesController(
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
            var cateogies = from category in db.Categories
                            select category;

            ViewBag.Categories = cateogies;

            SetAccessRights();

            return View();
        }

        [Authorize(Roles ="Admin")]
        public IActionResult New()
        {
            Category category = new Category();

            return View(category);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult New(Category category)
        {

            if (ModelState.IsValid)
            {
                db.Categories.Add(category);
                db.SaveChanges();
                TempData["message"] = "Categoria a fost adaugata cu succes";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["message"] = "Categoria nu a putut fi adaugata";
                return View(category);
            }
        }

        public IActionResult Show(int id) 
        {
            Category category = db.Categories.Find(id);

            SetAccessRights();

            return View(category);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int id)
        {
            Category category = db.Categories.Find(id);

            return View(category);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Edit(int id, Category categorieEditata)
        {
            Category category = db.Categories.Find(id);

            if(ModelState.IsValid)
            {
                category.Name = categorieEditata.Name;

                db.SaveChanges();
                TempData["message"] = "Categoria a fost editata cu succes";
                return Redirect("/Categories/Show/"+id);
            }
            else
            {
                TempData["message"] = "Categoria nu a putut fi editata";
                return View(categorieEditata);
            }
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            Category category = db.Categories.Include("Topics").Include("Topics.Comments").Where(c => c.Id == id).First();

            foreach(var topic in category.Topics)
            {
                foreach(var comment in topic.Comments)
                    db.Comments.Remove(comment);

                db.Topics.Remove(topic);
            }

            db.Categories.Remove(category);
            TempData["message"] = "Categoria a fost stearsa cu succes";
            db.SaveChanges();

            return RedirectToAction("Index");

        }

        private void SetAccessRights()
        {
            ViewBag.AfisareButoane = false;

            if ( User.IsInRole("User"))
            {
                ViewBag.AfisareButoane = true;
            }

            ViewBag.EsteAdmin = User.IsInRole("Admin");

            ViewBag.UserCurent = _userManager.GetUserId(User);
        }

    }
}
