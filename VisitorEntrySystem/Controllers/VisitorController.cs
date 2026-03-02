using Microsoft.AspNetCore.Mvc;
using VisitorEntrySystem.Data;
using VisitorEntrySystem.Models;

namespace VisitorEntrySystem.Controllers
{
    public class VisitorController : Controller
    {
        private readonly AppDbContext _db;

        public VisitorController(AppDbContext db)
        {
            _db = db;
        }

        private bool IsLoggedIn()
        {
            return HttpContext.Session.GetString("Username") != null;
        }

        public IActionResult Index()
        {
            if (!IsLoggedIn()) return RedirectToAction("Login", "Account");
            var visitors = _db.Visitors.ToList();
            return View(visitors);
        }

        public IActionResult Create()
        {
            if (!IsLoggedIn()) return RedirectToAction("Login", "Account");
            return View();
        }

        [HttpPost]
        public IActionResult Create(VisitorRecord visitor)
        {
            if (!IsLoggedIn()) return RedirectToAction("Login", "Account");
            visitor.CheckInTime = DateTime.Now;
            visitor.CreatedAt = DateTime.Now;
            visitor.RegisteredBy = HttpContext.Session.GetString("Username");
            _db.Visitors.Add(visitor);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            if (!IsLoggedIn()) return RedirectToAction("Login", "Account");
            var visitor = _db.Visitors.Find(id);
            if (visitor == null) return NotFound();
            return View(visitor);
        }

        [HttpPost]
        public IActionResult Edit(VisitorRecord visitor)
        {
            if (!IsLoggedIn()) return RedirectToAction("Login", "Account");
            _db.Visitors.Update(visitor);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            if (!IsLoggedIn()) return RedirectToAction("Login", "Account");
            var visitor = _db.Visitors.Find(id);
            if (visitor == null) return NotFound();
            _db.Visitors.Remove(visitor);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult CheckOut(int id)
        {
            if (!IsLoggedIn()) return RedirectToAction("Login", "Account");
            var visitor = _db.Visitors.Find(id);
            if (visitor != null)
            {
                visitor.CheckOutTime = DateTime.Now;
                _db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}