using Microsoft.AspNetCore.Mvc;
using OSM.Application.Interfaces;
using OSM.Models;
using OSM.Utilities.Constants;
using System.Diagnostics;

namespace OSM.Controllers
{
    public class HomeController : Controller
    {
        private IProductService _productService;
        private IProductCategoryService _productCategoryService;
        private IContactService _contactService;
        private IBlogService _blogService;
        private ICommonService _commonService;

        public HomeController(IContactService contactService,
            IProductService productService, IBlogService blogService,
            ICommonService commonService, IProductCategoryService productCategoryService)
        {
            _blogService = blogService;
            _commonService = commonService;
            _productService = productService;
            _productCategoryService = productCategoryService;
            _contactService = contactService;
        }
        [ResponseCache(CacheProfileName = "Default")]
        public IActionResult Index()
        {
            ViewData["BodyClass"] = "cms-index-index cms-home-page";
            var homeVm = new HomeViewModel();
            homeVm.HomeCategories = _productCategoryService.GetHomeCategories(3);
            homeVm.SpecialOffers = _productService.GetUpsellProducts(6);
            homeVm.HotProducts = _productService.GetHotProduct(6);
            homeVm.LastestProducts = _productService.GetLastest(6);
            homeVm.LastestBlogs = _blogService.GetLastest(6);
            homeVm.HomeSlides = _commonService.GetSlides("top");
            return View(homeVm);
        }





        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}