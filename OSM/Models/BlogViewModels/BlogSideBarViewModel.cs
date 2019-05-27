using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OSM.Models.BlogViewModels
{
    public class BlogSideBarViewModel
    {
        public List<Application.ViewModels.Blog.BlogViewModel> LastestBlogs { get; set; }
        public List<Application.ViewModels.Blog.BlogViewModel> HotestBlogs { get; set; }
    }
}
