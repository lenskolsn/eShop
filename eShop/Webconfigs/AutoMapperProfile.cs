using AutoMapper;
using eShop.Areas.Admin.ViewModels.Category;
using eShop.Areas.Admin.ViewModels.Product;
using eShop.Database.Entities;

namespace eShop.WebConfigs
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // Cấu hình các class được phép map
            CreateMap<Category, AddOrUpdateCategoryVM>().ReverseMap();
            CreateMap<Product, AddOrUpdateProductVM>().ReverseMap();
        }
        public static MapperConfiguration CategoryConfig = new MapperConfiguration(opt =>
        {
            opt.CreateMap<Category, AddOrUpdateCategoryVM>();
            opt.CreateMap<Category, ListItemCategoryVM>();
        });
        public static MapperConfiguration ProductConfig = new MapperConfiguration(opt =>
        {
            opt.CreateMap<Product, AddOrUpdateProductVM>().ReverseMap(); 
            opt.CreateMap<Product, ListItemProductVM>()
            .ForMember(vm => vm.CategoryName, opt => opt.MapFrom(entity => entity.Category.Name == null ? "" : entity.Category.Name));
        });
    }
}
