using eShop.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace eShop.Areas.Admin.Views.Shared.Components.SelectListCategory
{
    public class SelectListCategory : ViewComponent
    {
        private readonly AppDbContext _db;
        public SelectListCategory(AppDbContext db)
        {
            _db = db;
        }
        
        public IViewComponentResult Invoke(string vModel = null)
        {
            var data = _db.Categories
                .OrderByDescending(x => x.Id)
                .ToList();
            var selectList = new SelectList(data, "Id", "Name");
            ViewBag.vModel = vModel;
            return View(selectList);
        }
    }
}
