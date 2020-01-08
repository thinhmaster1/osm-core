using OSM.Application.ViewModels.Blog;
using OSM.Application.ViewModels.Common;
using OSM.Application.ViewModels.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OSM.Models
{
    public class HomeViewModel
    {
        public List<BlogViewModel> LastestBlogs { get; set; }
        public List<SlideViewModel> HomeSlides { get; set; }
        public List<ProductViewModel> HotProducts { get; set; }
        public List<ProductViewModel> LastestProducts { get; set; }
        public List<ProductViewModel> SpecialOffers { get; set; }
        public List<ProductCategoryViewModel> HomeCategories { set; get; }
        public string Title { set; get; }
        public string MetaKeyword { set; get; }
        public string MetaDescription { set; get; }

    }
}
