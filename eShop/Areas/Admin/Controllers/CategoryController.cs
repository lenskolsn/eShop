using eShop.Database;
using eShop.Database.Entities;
using Microsoft.AspNetCore.Mvc;

namespace eShop.Areas.Admin.Controllers
{
    public class CategoryController : BaseController
    {
        public CategoryController(AppDbContext db): base(db)
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


    }
}
