using AutoMapper;
using AutoMapper.QueryableExtensions;
using eShop.Areas.Admin.ViewModels.Category;
using eShop.Database;
using eShop.Database.Entities;
using eShop.WebConfigs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
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
        // Action này chạy trước cái action khác
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            var method = context.HttpContext.Request.Method;
            if (method == HttpMethod.Post.Method)
            {
                if (!ModelState.IsValid)
                {
                    // Trả về Json thông báo lỗi nếu dữ liệu ko hợp lệ
                    var errorModel = new SerializableError(ModelState);
                    context.Result = new BadRequestObjectResult(errorModel);
                }
            }
        }
        public IActionResult Index()
        {
            return View();
        }
        public List<ListItemCategoryVM> GetAll()
        {
            var query = _db.Categories
                .ProjectTo<ListItemCategoryVM>(AutoMapperProfile.CategoryConfig)
                .OrderByDescending(c => c.Id);
            var data = query.ToList();
            return data;
        }

        [HttpPost]
        public IActionResult Create([FromBody] AddOrUpdateCategoryVM categoryVM)
        {
            // xác thực dữ liệu
            if (ModelState.IsValid == false)
            {
                return Ok(new
                {
                    success = false,
                    mesg = "Dữ liệu không hợp lệ"
                });
            }
            // lưu vào database
            var category = new Category();
            // Map(from, to)
            _mapper.Map(categoryVM, category);

            category.CreateAt = DateTime.Now;
            category.UpdateAt = DateTime.Now;

            _db.Categories.Add(category);
            _db.SaveChanges();
            return Ok(new
            {
                success = true,
            });
        }

        public IActionResult GetForUpdate(int id)
        {
            var category = _db.Categories
                    .Where(c => c.Id == id)
                    .ProjectTo<AddOrUpdateCategoryVM>(AutoMapperProfile.CategoryConfig)
                    .FirstOrDefault();
            return Ok(category);
            //var category = _db.Categories
            //       .Select(c => new AddOrUpdateCategoryVM
            //       {
            //           Id = c.Id,
            //           Name = c.Name
            //       }).FirstOrDefault(c => c.Id == id);
            //return Ok(category);
        }
        [HttpPost]
        public IActionResult Update(int id, [FromBody] AddOrUpdateCategoryVM categoryVM)
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
                TempData["SuccessMess"] = "Cập nhật danh mục thành công!";
            }
            return Ok(new
            {
                success = true,
            });
        }

        public IActionResult Delete(int id)
        {
            // Không cho xóa nếu đã có danh mục sản phẩm
            if (_db.Products.Any(p => p.CategoryId == id))
            {
                return Ok(new
                {
                    success = false,
                    mesg = "Danh mục đã được sử dụng! Không thể xóa"
                });
            }
            var category = _db.Categories.Find(id);
            _db.Categories.Remove(category);
            _db.SaveChanges();
            return Ok(new
            {
                success = true,
            });
        }
    }
}
