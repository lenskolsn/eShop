using eShop.Areas.Admin.ViewModels.Category;
using eShop.Database;
using eShop.Database.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eShop.Areas.Admin.Controllers
{
    public class CategoryController : BaseController
    {
        public CategoryController(AppDbContext db) : base(db)
        {

        }
        public IActionResult Index()
        {
            var query = _db.Categories
                .Select(c=>new ListItemCategoryVM
                {
                    Id = c.Id,
                    Name = c.Name,
                    CreatedAt = c.CreateAt
                })
                .OrderByDescending(c => c.Id);
            ViewBag.Sql = query.ToQueryString();
            var data = query.ToList();
            return View(data);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(AddOrUpdateCategoryVM categoryVM)
        {
            // xác thực dữ liệu
            if(ModelState.IsValid == false)
            {
                return View(categoryVM);
            }
            // lưu vào database
            var category = new Category();
            category.CreateAt = DateTime.Now;
            category.UpdateAt = DateTime.Now;
            category.Name = categoryVM.Name;

            _db.Categories.Add(category);
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
    }
}
