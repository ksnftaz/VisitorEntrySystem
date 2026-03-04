using Microsoft.AspNetCore.Mvc;
using VisitorEntrySystem.Data;
using VisitorEntrySystem.Models;

namespace VisitorEntrySystem.Controllers
{
    public class VisitorController : Controller
    {
        private readonly AppDbContext _db;
        public VisitorController(AppDbContext db) { _db = db; }

        private bool IsLoggedIn() => HttpContext.Session.GetString("Username") != null;
        private int AdminId() => int.Parse(HttpContext.Session.GetString("UserId"));

        // Show only this admin's visitors
        public IActionResult Index()
        {
            if (!IsLoggedIn()) return RedirectToAction("Login", "Account");
            return View(_db.Visitors.Where(v => v.AdminId == AdminId()).ToList());
        }

        public IActionResult Create()
        {
            if (!IsLoggedIn()) return RedirectToAction("Login", "Account");
            return View();
        }

        // Save visitor and link to this admin
        [HttpPost]
        public IActionResult Create(VisitorRecord v)
        {
            if (!IsLoggedIn()) return RedirectToAction("Login", "Account");
            v.CheckInTime = DateTime.Now;
            v.CreatedAt = DateTime.Now;
            v.RegisteredBy = HttpContext.Session.GetString("Username");
            v.AdminId = AdminId();
            _db.Visitors.Add(v);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            if (!IsLoggedIn()) return RedirectToAction("Login", "Account");
            var v = _db.Visitors.Find(id);
            if (v == null) return NotFound();
            return View(v);
        }

        [HttpPost]
        public IActionResult Edit(VisitorRecord v)
        {
            if (!IsLoggedIn()) return RedirectToAction("Login", "Account");
            _db.Visitors.Update(v);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            if (!IsLoggedIn()) return RedirectToAction("Login", "Account");
            var v = _db.Visitors.Find(id);
            if (v != null) { _db.Visitors.Remove(v); _db.SaveChanges(); }
            return RedirectToAction("Index");
        }

        public IActionResult CheckOut(int id)
        {
            if (!IsLoggedIn()) return RedirectToAction("Login", "Account");
            var v = _db.Visitors.Find(id);
            if (v != null) { v.CheckOutTime = DateTime.Now; _db.SaveChanges(); }
            return RedirectToAction("Index");
        }
    }
}