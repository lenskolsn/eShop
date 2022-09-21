using AutoMapper;
using AutoMapper.QueryableExtensions;
using eShop.Areas.Admin.ViewModels.Product;
using eShop.Database;
using eShop.Database.Entities;
using eShop.WebConfigs;
using Microsoft.AspNetCore.Mvc;

namespace eShop.Areas.Admin.Controllers
{
    public class ProductController : BaseController
    {
        private readonly IMapper _mapper;
        public ProductController(AppDbContext db, IMapper mapper) : base(db)
        {
            _mapper = mapper;
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
            var product = new Product();
            _mapper.Map(productVM, product);

            product.CreateAt = DateTime.Now;
            product.UpdateAt = DateTime.Now;

            _db.Products.Add(product);
            _db.SaveChanges();

            return RedirectToAction("Index");
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
