using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace eShop.Areas.Admin.ViewModels.Product
{
    public class AddOrUpdateProductVM
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="{0} là bắt buộc!")]
        [MinLength(3,ErrorMessage ="{0} không được nhỏ hơn 3 ký tự!")]
        [DisplayName("Tên danh mục")] // dùng cho asp-for
        public string Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public decimal? DiscountPrice { get; set; }
        public string? CoverImg { get; set; }
        public int InStock { get; set; }
        public int? CategoryId { get; set; }
    }
}
