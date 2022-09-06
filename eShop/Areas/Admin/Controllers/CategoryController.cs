using AutoMapper;
using AutoMapper.QueryableExtensions;
using eShop.Areas.Admin.ViewModels.Category;
using eShop.Database;
using eShop.Database.Entities;
using eShop.WebConfigs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eShop.Areas.Admin.Controllers
{
    public class CategoryController : BaseController
    {
        private readonly IMapper _mapper;
        public CategoryController(AppDbContext db, IMapper mapper) : base(db)
        {
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            var query = _db.Categories
                .ProjectTo<ListItemCategoryVM>(AutoMapperProfile.CategoryConfig)
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
            if (ModelState.IsValid == false)
            {
                return View(categoryVM);
            }
            // lưu vào database
            var category = new Category();
            // Map(from, to)
            _mapper.Map(categoryVM, category);

            category.CreateAt = DateTime.Now;
            category.UpdateAt = DateTime.Now;

            _db.Categories.Add(category);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Update(int id)
        {
            var category = _db.Categories
                    .Select(c => new AddOrUpdateCategoryVM
                    {
                        Id = c.Id,
                        Name = c.Name
                    }).FirstOrDefault(c => c.Id == id);
            return View(category);
        }
        [HttpPost]
        public IActionResult Update(int id, AddOrUpdateCategoryVM categoryVM)
        {
            if (ModelState.IsValid == false)
            {
                return View(categoryVM);
            }
            var category = _db.Categories.Find(id);
            if (category != null)
            {
                _mapper.Map(categoryVM, category);
                _db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            // Không cho xóa nếu đã có danh mục sản phẩm
            if (_db.Products.Any(p => p.CategoryId == id))
            {
                TempData["Err"] = "Danh mục đã được sử dụng! Không thể xóa";
                return RedirectToAction("Index");
            }
            var category = _db.Categories.Find(id);
            if (category != null)
            {
                _db.Categories.Remove(category);
                _db.SaveChanges();
                TempData["SuccessMess"] = "Xóa danh mục thành công!";
            }
            return RedirectToAction("Index");
        }
    }
}
