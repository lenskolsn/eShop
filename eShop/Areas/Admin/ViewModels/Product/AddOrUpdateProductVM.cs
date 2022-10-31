using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace eShop.Areas.Admin.ViewModels.Product
{
    public class AddOrUpdateProductVM
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="Name is required")]
        public string Name { get; set; }
        public string? Description { get; set; }
        [Required(ErrorMessage = "Price is required")]
        public decimal Price { get; set; }
        public decimal? DiscountPrice { get; set; }
        public string? CoverImg { get; set; }
        [Required(ErrorMessage = "InStock is required")]
        public int InStock { get; set; }
        public int? CategoryId { get; set; }
    }
}
