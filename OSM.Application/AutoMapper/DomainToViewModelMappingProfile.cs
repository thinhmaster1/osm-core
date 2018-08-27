using AutoMapper;
using OSM.Application.ViewModels.Product;
using OSM.Data.Entities;

namespace OSM.Application.AutoMapper
{
    public class DomainToViewModelMappingProfile : Profile
    {
        public DomainToViewModelMappingProfile()
        {
            CreateMap<ProductCategory, ProductCategoryViewModel>();
        }
    }
}