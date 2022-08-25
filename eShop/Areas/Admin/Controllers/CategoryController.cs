using eShop.Database;
using eShop.Database.Entities;
using Microsoft.AspNetCore.Mvc;

namespace eShop.Areas.Admin.Controllers
{
    public class CategoryController : BaseController
    {
        public CategoryController(AppDbContext db) : base(db)
        {

        }
        public IActionResult Index()
        {
            var data = _db.Categories.OrderByDescending(c => c.Id).ToList();
            return View(data);
        }

        public IActionResult Create()
        {
            return View();
        }

        // public IActionResult Create() => View();

        [HttpPost]
        public IActionResult Create(Category ct)
        {
            ct.CreateAt = DateTime.Now;
            ct.UpdateAt = DateTime.Now;
            _db.Categories.Add(ct);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Update(int id)
        {
            var category = _db.Categories.Find(id);
            return View(category);
        }
        [HttpPost]
        public IActionResult Update(int id, Category ct)
        {
            var category = _db.Categories.Find(id);
            category.Name = ct.Name;
            category.UpdateAt = DateTime.Now;
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            Category ct = _db.Categories.Find(id);
            _db.Categories.Remove(ct);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Detail(int id)
        {
            var category = _db.Categories.Find(id);
            return View(category);
        }
    }
}
