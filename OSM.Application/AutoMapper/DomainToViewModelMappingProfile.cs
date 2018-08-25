using AutoMapper;
using OSM.Application.ViewModels.Product;
using OSM.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

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
