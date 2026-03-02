using Microsoft.AspNetCore.Mvc;
using VisitorEntrySystem.Data;
using VisitorEntrySystem.Models;

namespace VisitorEntrySystem.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _db;

        public AccountController(AppDbContext db)
        {
            _db = db;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            var user = _db.Users.FirstOrDefault(u =>
                u.Username == username && u.PasswordHash == password);

            if (user == null)
            {
                ViewBag.Error = "Wrong username or password.";
                return View();
            }

            HttpContext.Session.SetString("Username", user.Username);
            HttpContext.Session.SetString("Role", user.Role);
            HttpContext.Session.SetString("FullName", user.FullName ?? user.Username);

            if (user.IsAdmin())
                return RedirectToAction("Dashboard", "Admin");
            else
                return RedirectToAction("Index", "Visitor");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        public IActionResult Register()
        {
            bool adminExists = _db.Users.Any(u => u.Role == "Admin");
            if (adminExists)
            {
                ViewBag.Error = "An admin account already exists. Please login.";
            }
            return View();
        }

        [HttpPost]
        public IActionResult Register(User user)
        {
            bool adminExists = _db.Users.Any(u => u.Role == "Admin");
            if (adminExists)
            {
                ViewBag.Error = "An admin account already exists.";
                return View();
            }

            bool usernameTaken = _db.Users.Any(u => u.Username == user.Username);
            if (usernameTaken)
            {
                ViewBag.Error = "Username already taken.";
                return View();
            }

            user.Role = "Admin";
            _db.Users.Add(user);
            _db.SaveChanges();

            ViewBag.Success = "Admin account created! You can now login.";
            return View();
        }
    }
}