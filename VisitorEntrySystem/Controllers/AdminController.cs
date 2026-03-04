using Microsoft.AspNetCore.Mvc;
using VisitorEntrySystem.Data;
using VisitorEntrySystem.Models;

namespace VisitorEntrySystem.Controllers
{
    public class AdminController : Controller
    {
        private readonly AppDbContext _db;
        public AdminController(AppDbContext db) { _db = db; }

        // Get current admin's ID from session
        private int AdminId() => int.Parse(HttpContext.Session.GetString("UserId"));
        private bool IsAdmin() => HttpContext.Session.GetString("Role") == "Admin";

        // Show only this admin's visitors
        public IActionResult Dashboard(string search)
        {
            if (!IsAdmin()) return RedirectToAction("Login", "Account");
            var list = _db.Visitors.Where(v => v.AdminId == AdminId()).ToList();
            if (!string.IsNullOrEmpty(search))
                list = list.Where(v => v.FullName.Contains(search)).ToList();
            ViewBag.Search = search;
            return View(list);
        }

        // Show only this admin's users
        public IActionResult ManageUsers()
        {
            if (!IsAdmin()) return RedirectToAction("Login", "Account");
            return View(_db.Users.Where(u => u.AdminId == AdminId()).ToList());
        }

        public IActionResult AddUser()
        {
            if (!IsAdmin()) return RedirectToAction("Login", "Account");
            return View();
        }

        // Link new user to this admin
        [HttpPost]
        public IActionResult AddUser(User user)
        {
            if (!IsAdmin()) return RedirectToAction("Login", "Account");
            user.AdminId = AdminId();
            _db.Users.Add(user);
            _db.SaveChanges();
            return RedirectToAction("ManageUsers");
        }

        public IActionResult DeleteUser(int id)
        {
            if (!IsAdmin()) return RedirectToAction("Login", "Account");
            var user = _db.Users.Find(id);
            if (user != null) { _db.Users.Remove(user); _db.SaveChanges(); }
            return RedirectToAction("ManageUsers");
        }
    }
}