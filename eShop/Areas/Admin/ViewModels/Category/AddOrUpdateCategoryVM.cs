﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace eShop.Areas.Admin.ViewModels.Category
{
    public class AddOrUpdateCategoryVM
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="{0} là bắt buộc!")]
        [MinLength(3,ErrorMessage ="{0} không được nhỏ hơn 3 ký tự!")]
        [DisplayName("Tên danh mục")] // dùng cho asp-for
        public string Name { get; set; }
    }
}
