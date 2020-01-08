using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using OSM.Application.Interfaces;
using OSM.Application.ViewModels.Bill;
using OSM.Data.Enums;
using OSM.Extensions;
using OSM.Models;
using OSM.Services;
using OSM.Utilities.Constants;

namespace OSM.Controllers
{
    public class CartController : Controller
    {
        IProductService _productService;
        IBillService _billService;
        IViewRenderService _viewRenderService;
        IConfiguration  _configuration;
        IEmailSender _emailSender;
        public CartController(IProductService productService, IBillService billService, IViewRenderService viewRenderService, IConfiguration configuration, IEmailSender emailSender)
        {
            _productService = productService;
            _billService = billService;
            _viewRenderService = viewRenderService;
            _configuration = configuration;
            _emailSender = emailSender;
        }
        [Route("cart.html", Name = "Cart")]
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        [Route("checkout.html", Name = "Checkout")]
        public IActionResult Checkout()
        {
            var model = new CheckoutViewModel();
            var session = HttpContext.Session.Get<List<ShoppingCartViewModel>>(CommonConstants.CartSession);
            if (session == null || session.Any(x => x.Color == null || x.Size == null))
            {
                return Redirect("/cart.html");
            }
            model.Carts = session;
            return View(model);
        }
        [Route("checkout.html", Name = "Checkout")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Checkout(CheckoutViewModel model)
        {
            var session = HttpContext.Session.Get<List<ShoppingCartViewModel>>(CommonConstants.CartSession);
            
            if (ModelState.IsValid)
            {
                if (session != null)
                {
                    var details = new List<BillDetailViewModel>();
                    foreach (var item in session)
                    {
                        details.Add(new BillDetailViewModel()
                        {
                            Product = item.Product,
                            Price = item.Price,
                            ColorId = item.Color.Id,
                            SizeId = item.Size.Id,
                            Quantity = item.Quantity,
                            ProductId = item.Product.Id
                            
                        });
                        var quantity = _productService.GetQuantity(item.Product.Id, item.Color.Id, item.Size.Id);
                        
                        _productService.UpdateQuantity(quantity.Id, item.Quantity);
                        
                    }
                    var billViewModel = new BillViewModel()
                    {
                        CustomerMobile = model.CustomerMobile,
                        BillStatus = BillStatus.New,
                        CustomerAddress = model.CustomerAddress,
                        CustomerName = model.CustomerName,
                        CustomerMessage = model.CustomerMessage,
                        BillDetails = details
                    };
                    if (User.Identity.IsAuthenticated == true)
                    {
                        billViewModel.CustomerId = Guid.Parse(User.GetSpecificClaim("UserId"));
                    }
                    _billService.Create(billViewModel);

                    try
                    {
                        _billService.Save();
                        var content = await _viewRenderService.RenderToStringAsync("Cart/_BillMail", billViewModel);
                        //Send mail
                        await _emailSender.SendEmailAsync(_configuration["MailSettings:AdminMail"], "New bill from Online Shopping Mart", content);
                        ViewData["Success"] = true;
                        HttpContext.Session.Remove(CommonConstants.CartSession);
                    }
                    catch (Exception ex)
                    {
                        ViewData["Success"] = false;
                        ModelState.AddModelError("", ex.Message);
                    }
                }
            }
            model.Carts = session;
            return View(model);
        }
        #region AJAX Request
        /// <summary>
        /// Get list item
        /// </summary>
        /// <returns></returns>
        public IActionResult GetCart()
        {
            var session = HttpContext.Session.GetString(CommonConstants.CartSession);
            var cart = new List<ShoppingCartViewModel>();
            if (session != null)
                cart = JsonConvert.DeserializeObject<List<ShoppingCartViewModel>>(session);
            return new OkObjectResult(cart);
        }
        /// <summary>
        /// Remove all products in cart
        /// </summary>
        /// <returns></returns>
        public IActionResult ClearCart()
        {
            HttpContext.Session.Remove(CommonConstants.CartSession);
            return new OkObjectResult("OK");
        }
        /// <summary>
        /// Add product to cart
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="quantity"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult AddToCart(int productId, int quantity, int color, int size)
        {
            //Get product detail
            var product = _productService.GetById(productId);
            //Get session with item list from cart
            var session = HttpContext.Session.Get<List<ShoppingCartViewModel>>(CommonConstants.CartSession);
            if (session != null)
            {
                //Convert string to list object
                bool hasChanged = false;
                //Check exist with item product id
                if (session.Any(x => x.Product.Id == productId))
                {
                    var newCartList = new List<ShoppingCartViewModel>();
                    foreach (var item in session)
                    {
                        var maxQuantity = _productService.GetQuantity(item.Product.Id, item.Color.Id, item.Size.Id).Quantity;
                        //Update quantity for product if match product id
                        if (item.Product.Id == productId && item.Color.Id == color && item.Size.Id == size)
                        {
                            if(maxQuantity > item.Quantity + quantity)
                            {
                                item.Quantity += quantity;
                            }
                            else
                            {
                                item.Quantity = maxQuantity;
                            }

                            item.Price = product.PromotionPrice ?? product.Price;
                            hasChanged = true;
                        }
                        else
                        {
                            if (quantity > maxQuantity)
                            {
                                quantity = maxQuantity;
                            }
                            newCartList.Add(new ShoppingCartViewModel()
                            {
                                Product = product,
                                Quantity = quantity,
                                Color = _billService.GetColor(color),
                                Size = _billService.GetSize(size),
                                Price = product.PromotionPrice ?? product.Price
                            });
                        }
                    }
                    if(newCartList.Count() != 0)
                    {
                        hasChanged = true;
                        session.AddRange(newCartList);
                    }
                }
                else
                {
                    var maxQuantity = _productService.GetQuantity(productId, color, size).Quantity;
                    if (quantity > maxQuantity)
                    {
                        quantity = maxQuantity;
                    }
                    session.Add(new ShoppingCartViewModel()
                    {
                        Product = product,
                        Quantity = quantity,
                        Color = _billService.GetColor(color),
                        Size = _billService.GetSize(size),
                        Price = product.PromotionPrice ?? product.Price
                    });
                    hasChanged = true;
                }
                //Update back to cart
                if (hasChanged)
                {
                    HttpContext.Session.Set(CommonConstants.CartSession, session);
                }
            }
            else
            {
                //Add new cart
                var cart = new List<ShoppingCartViewModel>();
                cart.Add(new ShoppingCartViewModel()
                {
                    Product = product,
                    Quantity = quantity,
                    Color = _billService.GetColor(color),
                    Size = _billService.GetSize(size),
                    Price = product.PromotionPrice ?? product.Price
                });
                HttpContext.Session.Set(CommonConstants.CartSession, cart);
            }
            return new OkObjectResult(productId);
        }
        /// <summary>
        /// Remove a product
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public IActionResult RemoveFromCart(int productId)
        {
            var session = HttpContext.Session.Get<List<ShoppingCartViewModel>>(CommonConstants.CartSession);
            if (session != null)
            {
                bool hasChanged = false;
                foreach (var item in session)
                {
                    if (item.Product.Id == productId)
                    {
                        session.Remove(item);
                        hasChanged = true;
                        break;
                    }
                }
                if (hasChanged)
                {
                    HttpContext.Session.Set(CommonConstants.CartSession, session);
                }
                return new OkObjectResult(productId);
            }
            return new EmptyResult();
        }
        /// <summary>
        /// Update product quantity
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="quantity"></param>
        /// <returns></returns>
        public IActionResult UpdateCart(int productId, int quantity, int color, int size)
        {
            var session = HttpContext.Session.Get<List<ShoppingCartViewModel>>(CommonConstants.CartSession);
            if (session != null)
            {
                bool hasChanged = false;
                foreach (var item in session)
                {
                    if (item.Product.Id == productId)
                    {
                        var product = _productService.GetById(productId);
                        item.Product = product;
                        item.Quantity = quantity;
                        item.Size = _billService.GetSize(size);
                        item.Color = _billService.GetColor(color);
                        item.Price = product.PromotionPrice ?? product.Price;
                        hasChanged = true;
                    }
                }
                if (hasChanged)
                {
                    HttpContext.Session.Set(CommonConstants.CartSession, session);
                }
                return new OkObjectResult(productId);
            }
            return new EmptyResult();
        }
        [HttpGet]
        public IActionResult GetColors()
        {
            var colors = _billService.GetColors();
            return new OkObjectResult(colors);
        }
        [HttpGet]
        public IActionResult GetSizes()
        {
            var sizes = _billService.GetSizes();
            return new OkObjectResult(sizes);
        }
        #endregion
    }
}
