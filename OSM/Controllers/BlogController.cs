using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OSM.Application.Interfaces;
using OSM.Application.ViewModels.Blog;

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
        public IActionResult Index()
        {
            var blogs = _blogService.GetAll();
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