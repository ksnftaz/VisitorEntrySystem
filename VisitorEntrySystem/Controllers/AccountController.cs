using Microsoft.AspNetCore.Mvc;
using VisitorEntrySystem.Data;
using VisitorEntrySystem.Models;

namespace VisitorEntrySystem.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _db;
        public AccountController(AppDbContext db) { _db = db; }

        public IActionResult Login() => View();

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            var user = _db.Users.FirstOrDefault(u => u.Username == username && u.PasswordHash == password);
            if (user == null)
            {
                ViewBag.Error = "Wrong username or password.";
                return View();
            }

            // Admin stores own ID, Receptionist stores their admin's ID
            var adminId = user.IsAdmin() ? user.Id : user.AdminId;
            HttpContext.Session.SetString("UserId", adminId.ToString());
            HttpContext.Session.SetString("Username", user.Username);
            HttpContext.Session.SetString("Role", user.Role);

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

        public IActionResult Register() => View();

        [HttpPost]
        public IActionResult Register(User user)
        {
            if (_db.Users.Any(u => u.Username == user.Username))
            {
                ViewBag.Error = "Username already taken.";
                return View();
            }
            user.Role = "Admin";
            _db.Users.Add(user);
            _db.SaveChanges();
            ViewBag.Success = "Account created!";
            return View();
        }
    }
}