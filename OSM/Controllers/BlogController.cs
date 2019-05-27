using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OSM.Application.Interfaces;
using OSM.Application.ViewModels.Blog;
using OSM.Models.BlogViewModels;
using OSM.Utilities.Constants;

namespace OSM.Controllers
{
    public class BlogController : Controller
    {
        IBlogService _blogService;

        public BlogController(IBlogService blogService)
        {
            _blogService = blogService;
        }
        [Route("blogs.html")]
        public IActionResult Index(string keyword, int page = 1)
        {
            var pageSize = CommonConstants.PageSize;
            var data = _blogService.GetAllPaging(keyword, pageSize, page);
            var blogs = new Models.BlogViewModels.BlogViewModel
            {
                Data = data
            };
            return View(blogs);
        }
        [Route("{alias}-b.{id}.html", Name = "BlogDetail")]
        public IActionResult Details(int id)
        {
            ViewData["BodyClass"] = "blog-page";
            var model = _blogService.GetById(id);
            _blogService.IncreaseView(id);
            _blogService.Save();
            return View(model);
        }
    }
}