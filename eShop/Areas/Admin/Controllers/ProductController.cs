using AutoMapper;
using AutoMapper.QueryableExtensions;
using eShop.Areas.Admin.ViewModels.Product;
using eShop.Database;
using eShop.Database.Entities;
using eShop.WebConfigs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace eShop.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    public class ProductController : BaseController
    {
        private readonly IMapper _mapper;
        public ProductController(AppDbContext db, IMapper mapper) : base(db)
        {
            _mapper = mapper;
        }
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
        public List<ListItemProductVM> GetAll()
        {
            var query = _db.Products
                .ProjectTo<ListItemProductVM>(AutoMapperProfile.ProductConfig)
                .OrderByDescending(c => c.Id);
            var data = query.ToList();
            return data;
        }
        [HttpPost]
        public IActionResult Create([FromBody] AddOrUpdateProductVM productVM)
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

            var product = new Product();
            // Map(from, to)
            _mapper.Map(productVM, product);

            product.CreateAt = DateTime.Now;
            product.UpdateAt = DateTime.Now;
            _db.Products.Add(product);
            _db.SaveChanges();
            return Ok(new
            {
                success = true,
            });
        }
        public IActionResult Delete(int id)
        {
            var pd = _db.Products.Find(id);
            _db.Remove(pd);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult GetDataForUpdate(int id)
        {
            var product = _db.Products.FirstOrDefault(c => c.Id == id);
            return Ok(product);
        }
        [HttpPost]
        public IActionResult Update([FromBody] AddOrUpdateProductVM productVM, int id)
        {
            var product = _db.Products.FirstOrDefault(c => c.Id == id);
            _mapper.Map(productVM, product);
            product.UpdateAt = DateTime.Now;
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
