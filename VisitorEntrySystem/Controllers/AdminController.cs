using Microsoft.AspNetCore.Mvc;
using VisitorEntrySystem.Data;
using VisitorEntrySystem.Models;

namespace VisitorEntrySystem.Controllers
{
    public class AdminController : Controller
    {
        private readonly AppDbContext _db;

        public AdminController(AppDbContext db)
        {
            _db = db;
        }

        private bool IsAdmin()
        {
            return HttpContext.Session.GetString("Role") == "Admin";
        }

        public IActionResult Dashboard(string search)
        {
            if (!IsAdmin()) return RedirectToAction("Login", "Account");
            var visitors = _db.Visitors.AsQueryable();
            if (!string.IsNullOrEmpty(search))
                visitors = visitors.Where(v => v.FullName.Contains(search));
            ViewBag.Search = search;
            return View(visitors.ToList());
        }

        public IActionResult ManageUsers()
        {
            if (!IsAdmin()) return RedirectToAction("Login", "Account");
            var users = _db.Users.ToList();
            return View(users);
        }

        public IActionResult AddUser()
        {
            if (!IsAdmin()) return RedirectToAction("Login", "Account");
            return View();
        }

        [HttpPost]
        public IActionResult AddUser(User user)
        {
            if (!IsAdmin()) return RedirectToAction("Login", "Account");
            _db.Users.Add(user);
            _db.SaveChanges();
            return RedirectToAction("ManageUsers");
        }

        public IActionResult DeleteUser(int id)
        {
            if (!IsAdmin()) return RedirectToAction("Login", "Account");
            var user = _db.Users.Find(id);
            if (user != null)
            {
                _db.Users.Remove(user);
                _db.SaveChanges();
            }
            return RedirectToAction("ManageUsers");
        }
    }
}