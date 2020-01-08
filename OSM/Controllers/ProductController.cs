using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OSM.Application.Interfaces;
using OSM.Application.ViewModels.Product;
using OSM.Models.ProductViewModels;
using OSM.Utilities.Constants;

namespace OSM.Controllers
{
    public class ProductController : Controller
    {
        IProductService _productService;
        IProductCategoryService _productCategoryService;
        IBillService _billService;


        public ProductController(IProductService productService, IProductCategoryService productCategoryService, IBillService billService)
        {
            _productService = productService;
            _productCategoryService = productCategoryService;
            _billService = billService;
        }
        [Route("products.html")]
        public IActionResult Index()
        {
            var categories = _productCategoryService.GetAll();
            return View(categories);
        }

        [Route("{alias}-c.{id}.html")]
        public IActionResult Catalog(int id, int? pageSize, string sortBy, int page = 1)
        {
            var catalog = new CatalogViewModel();
            ViewData["BodyClass"] = "shop_grid_full_width_page";
            if (pageSize == null)
                pageSize = CommonConstants.PageSize;
            var data = _productService.GetAllPaging(id, string.Empty, page, pageSize.Value, sortBy);
            if(data == null)
            {
                
            }
            catalog.PageSize = pageSize;
            catalog.SortType = sortBy;
            catalog.Data = data;
            catalog.Category = _productCategoryService.GetById(id);

            return View(catalog);
        }

        [Route("{alias}-p.{id}.html", Name = "ProductDetail")]
        public IActionResult Details(int id)
        {
            ViewData["BodyClass"] = "product-page";
            var quantities = _productService.GetQuantities(id);
            var model = new DetailViewModel();
            var ColorList = new List<SelectListItem>();
            var SizeList = new List<SelectListItem>();
            foreach(var quantity in quantities)
            {
                ColorList.Add(new SelectListItem()
                {
                    Text = quantity.Color.Name,
                    Value = quantity.ColorId.ToString()
                });
                SizeList.Add(new SelectListItem()
                {
                    Text = quantity.Size.Name,
                    Value = quantity.SizeId.ToString()
                });
            }
            model.Colors = ColorList;
            model.Sizes = SizeList;
            model.Product = _productService.GetById(id);
            model.Category = _productCategoryService.GetById(model.Product.CategoryId);
            model.RelatedProducts = _productService.GetRelatedProducts(id, 9);
            model.UpsellProducts = _productService.GetUpsellProducts(6);
            model.ProductImages = _productService.GetImages(id);
            model.Tags = _productService.GetProductTags(id);
            model.Quantities = quantities;
            _productService.IncreaseView(id);
            return View(model);
        }
        [Route("search.html")]
        public IActionResult Search(string keyword, int? pageSize, string sortBy, int page = 1)
        {
            var catalog = new SearchResultViewModel();
            ViewData["BodyClass"] = "shop_grid_full_width_page";
            if (pageSize == null)
                pageSize = CommonConstants.PageSize;
            catalog.PageSize = pageSize;
            catalog.SortType = sortBy;
            catalog.Data = _productService.GetAllPaging(null, keyword, page, pageSize.Value, sortBy);
            catalog.Keyword = keyword;
            return View(catalog);
        }
        [HttpGet]
        public IActionResult GetQuantity(int productId, int colorId, int sizeId)
        {
            var model = _productService.GetQuantity(productId, colorId, sizeId);
            return new OkObjectResult(model.Quantity);
        }
    }
}