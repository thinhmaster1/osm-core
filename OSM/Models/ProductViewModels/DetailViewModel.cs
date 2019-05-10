using Microsoft.AspNetCore.Mvc.Rendering;
using OSM.Application.ViewModels.Common;
using OSM.Application.ViewModels.Product;
using System;
using System.Collections.Generic;
using System.Text;

namespace OSM.Models.ProductViewModels
{
    public class DetailViewModel
    {
        public ProductViewModel Product { get; set; }

        public List<ProductViewModel> RelatedProducts { get; set; }

        public ProductCategoryViewModel Category { get; set; }

        public List<ProductImageViewModel> ProductImages { set; get; }

        public List<ProductViewModel> UpsellProducts { get; set; }

        public List<ProductViewModel> LastestProducts { get; set; }

        public List<TagViewModel> Tags { set; get; }
        public List<SelectListItem> Colors { get;  set; }
        public List<SelectListItem> Sizes { get;  set; }
        public bool Available { set; get; }
        public List<ProductQuantityViewModel> Quantities { get; set; }
    }
}
