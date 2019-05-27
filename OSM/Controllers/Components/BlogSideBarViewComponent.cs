using Microsoft.AspNetCore.Mvc;
using OSM.Application.Interfaces;
using OSM.Models.BlogViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OSM.Controllers.Components
{
    public class BlogSideBarViewComponent : ViewComponent
    {
        private readonly IBlogService _blogService; 
        public BlogSideBarViewComponent(IBlogService blogService)
        {
            _blogService = blogService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var hostestPosts = _blogService.GetHotProduct(3);
            var lastestPosts = _blogService.GetLastest(3);
            var blogSideBar = new BlogSideBarViewModel();
            blogSideBar.HotestBlogs = hostestPosts;
            blogSideBar.LastestBlogs = lastestPosts;
            return View(blogSideBar);
        }
    }
}
