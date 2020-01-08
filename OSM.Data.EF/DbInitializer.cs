using Microsoft.AspNetCore.Identity;
using OSM.Data.Entities;
using OSM.Data.Enums;
using OSM.Utilities.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OSM.Data.EF
{
    public class DbInitializer
    {
        private readonly AppDbContext _context;
        private UserManager<AppUser> _userManager;
        private RoleManager<AppRole> _roleManager;

        public DbInitializer(AppDbContext context, UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task Seed()
        {
            if (!_roleManager.Roles.Any())
            {
                await _roleManager.CreateAsync(new AppRole()
                {
                    Name = "Admin",
                    NormalizedName = "Admin",
                    Description = "Top manager"
                });
                await _roleManager.CreateAsync(new AppRole()
                {
                    Name = "Staff",
                    NormalizedName = "Staff",
                    Description = "Staff"
                });
                await _roleManager.CreateAsync(new AppRole()
                {
                    Name = "Customer",
                    NormalizedName = "Customer",
                    Description = "Customer"
                });
            }
            if (!_userManager.Users.Any())
            {
                await _userManager.CreateAsync(new AppUser()
                {
                    UserName = "admin@gmail.com",
                    FullName = "Administrator",
                    Email = "admin@gmail.com",
                    Balance = 0,
                    DateCreated = DateTime.Now,
                    DateModified = DateTime.Now,
                    Status = Status.Active
                }, "123654$");
                var user =  await _userManager.FindByEmailAsync("admin@gmail.com");
                await _userManager.AddToRoleAsync(user, "Admin");
            }
            if (_context.Functions.Count() == 0)
            {
                _context.Functions.AddRange(new List<Function>()
                {
                    new Function() {Id = "SYSTEM", Name = "System",ParentId = null,SortOrder = 1,Status = Status.Active,URL = "/",IconCss = "fa-desktop"  },
                    new Function() {Id = "ROLE", Name = "Role",ParentId = "SYSTEM",SortOrder = 1,Status = Status.Active,URL = "/admin/role/index",IconCss = "fa-home"  },
                    new Function() {Id = "FUNCTION", Name = "Function",ParentId = "SYSTEM",SortOrder = 2,Status = Status.Active,URL = "/admin/function/index",IconCss = "fa-home"  },
                    new Function() {Id = "USER", Name = "User",ParentId = "SYSTEM",SortOrder =3,Status = Status.Active,URL = "/admin/user/index",IconCss = "fa-home"  },
                    new Function() {Id = "ACTIVITY", Name = "Activity",ParentId = "SYSTEM",SortOrder = 4,Status = Status.Active,URL = "/admin/activity/index",IconCss = "fa-home"  },
                    new Function() {Id = "ERROR", Name = "Error",ParentId = "SYSTEM",SortOrder = 5,Status = Status.Active,URL = "/admin/error/index",IconCss = "fa-home"  },
                    new Function() {Id = "SETTING", Name = "Configuration",ParentId = "SYSTEM",SortOrder = 6,Status = Status.Active,URL = "/admin/setting/index",IconCss = "fa-home"  },
                    new Function() {Id = "PRODUCT",Name = "Product Management",ParentId = null,SortOrder = 2,Status = Status.Active,URL = "/",IconCss = "fa-chevron-down"  },
                    new Function() {Id = "PRODUCT_CATEGORY",Name = "Category",ParentId = "PRODUCT",SortOrder =1,Status = Status.Active,URL = "/admin/productcategory/index",IconCss = "fa-chevron-down"  },
                    new Function() {Id = "PRODUCT_LIST",Name = "Product",ParentId = "PRODUCT",SortOrder = 2,Status = Status.Active,URL = "/admin/product/index",IconCss = "fa-chevron-down"  },
                    new Function() {Id = "BILL",Name = "Bill",ParentId = "PRODUCT",SortOrder = 3,Status = Status.Active,URL = "/admin/bill/index",IconCss = "fa-chevron-down"  },
                    new Function() {Id = "CONTENT",Name = "Content",ParentId = null,SortOrder = 3,Status = Status.Active,URL = "/",IconCss = "fa-table"  },
                    new Function() {Id = "BLOG",Name = "Blog",ParentId = "CONTENT",SortOrder = 1,Status = Status.Active,URL = "/admin/blog/index",IconCss = "fa-table"  },
                     new Function() {Id = "PAGE",Name = "Page",ParentId = "CONTENT",SortOrder = 2,Status = Status.Active,URL = "/admin/page/index",IconCss = "fa-table"  },
                    new Function() {Id = "UTILITY",Name = "Utilities",ParentId = null,SortOrder = 4,Status = Status.Active,URL = "/",IconCss = "fa-clone"  },
                    new Function() {Id = "FOOTER",Name = "Footer",ParentId = "UTILITY",SortOrder = 1,Status = Status.Active,URL = "/admin/footer/index",IconCss = "fa-clone"  },
                    new Function() {Id = "FEEDBACK",Name = "Feedback",ParentId = "UTILITY",SortOrder = 2,Status = Status.Active,URL = "/admin/feedback/index",IconCss = "fa-clone"  },
                    new Function() {Id = "ANNOUNCEMENT",Name = "Announcement",ParentId = "UTILITY",SortOrder = 3,Status = Status.Active,URL = "/admin/announcement/index",IconCss = "fa-clone"  },
                    new Function() {Id = "CONTACT",Name = "Contact",ParentId = "UTILITY",SortOrder = 4,Status = Status.Active,URL = "/admin/contact/index",IconCss = "fa-clone"  },
                    new Function() {Id = "SLIDE",Name = "Slide",ParentId = "UTILITY",SortOrder = 5,Status = Status.Active,URL = "/admin/slide/index",IconCss = "fa-clone"  },
                    new Function() {Id = "ADVERTISMENT",Name = "Advertisment",ParentId = "UTILITY",SortOrder = 6,Status = Status.Active,URL = "/admin/advertistment/index",IconCss = "fa-clone"  },

                    new Function() {Id = "REPORT",Name = "Report",ParentId = null,SortOrder = 5,Status = Status.Active,URL = "/",IconCss = "fa-bar-chart-o"  },
                    new Function() {Id = "REVENUES",Name = "Revenue report",ParentId = "REPORT",SortOrder = 1,Status = Status.Active,URL = "/admin/report/revenues",IconCss = "fa-bar-chart-o"  },
                    new Function() {Id = "ACCESS",Name = "Visitor Report",ParentId = "REPORT",SortOrder = 2,Status = Status.Active,URL = "/admin/report/visitor",IconCss = "fa-bar-chart-o"  },
                    new Function() {Id = "READER",Name = "Reader Report",ParentId = "REPORT",SortOrder = 3,Status = Status.Active,URL = "/admin/report/reader",IconCss = "fa-bar-chart-o"  },
                });
            }

            if (_context.Footers.Count(x => x.Id == CommonConstants.DefaultFooterId) == 0)
            {
                string content = "Footer";
                _context.Footers.Add(new Footer()
                {
                    Id = CommonConstants.DefaultFooterId,
                    Content = content
                });
            }

            if (_context.Colors.Count() == 0)
            {
                List<Color> listColor = new List<Color>()
                {
                    new Color() {Name="Black" },
                    new Color() {Name="White"},
                    new Color() {Name="Red"},
                    new Color() {Name="Blue"},
                };
                _context.Colors.AddRange(listColor);
            }
            if (_context.AdvertistmentPages.Count() == 0)
            {
                List<AdvertistmentPage> pages = new List<AdvertistmentPage>()
                {
                    new AdvertistmentPage() {Id="home", Name="Home",AdvertistmentPositions = new List<AdvertistmentPosition>(){
                        new AdvertistmentPosition(){Id="home-left",Name="Left Advertisment Page"}
                    } },
                    new AdvertistmentPage() {Id="product-cate", Name="Product category" ,
                        AdvertistmentPositions = new List<AdvertistmentPosition>(){
                        new AdvertistmentPosition(){Id="product-cate-left",Name="Left Product Category"}
                    }},
                    new AdvertistmentPage() {Id="product-detail", Name="Product detail",
                        AdvertistmentPositions = new List<AdvertistmentPosition>(){
                        new AdvertistmentPosition(){Id="product-detail-left",Name="Left Product Detail"}
                    } },
                };
                _context.AdvertistmentPages.AddRange(pages);
            }

            if (_context.Slides.Count() == 0)
            {
                List<Slide> slides = new List<Slide>()
                {
                    new Slide() {Name="Slide 1",Image="/client-side/images/slider/slide-1.jpg",Url="#",DisplayOrder = 0,GroupAlias = "top",Status = true },
                    new Slide() {Name="Slide 2",Image="/client-side/images/slider/slide-2.jpg",Url="#",DisplayOrder = 1,GroupAlias = "top",Status = true },
                    new Slide() {Name="Slide 3",Image="/client-side/images/slider/slide-3.jpg",Url="#",DisplayOrder = 2,GroupAlias = "top",Status = true },

                    new Slide() {Name="Slide 1",Image="/client-side/images/brand1.png",Url="#",DisplayOrder = 1,GroupAlias = "brand",Status = true },
                    new Slide() {Name="Slide 2",Image="/client-side/images/brand2.png",Url="#",DisplayOrder = 2,GroupAlias = "brand",Status = true },
                    new Slide() {Name="Slide 3",Image="/client-side/images/brand3.png",Url="#",DisplayOrder = 3,GroupAlias = "brand",Status = true },
                    new Slide() {Name="Slide 4",Image="/client-side/images/brand4.png",Url="#",DisplayOrder = 4,GroupAlias = "brand",Status = true },
                    new Slide() {Name="Slide 5",Image="/client-side/images/brand5.png",Url="#",DisplayOrder = 5,GroupAlias = "brand",Status = true },
                    new Slide() {Name="Slide 6",Image="/client-side/images/brand6.png",Url="#",DisplayOrder = 6,GroupAlias = "brand",Status = true },
                    new Slide() {Name="Slide 7",Image="/client-side/images/brand7.png",Url="#",DisplayOrder = 7,GroupAlias = "brand",Status = true },
                    new Slide() {Name="Slide 8",Image="/client-side/images/brand8.png",Url="#",DisplayOrder = 8,GroupAlias = "brand",Status = true },
                    new Slide() {Name="Slide 9",Image="/client-side/images/brand9.png",Url="#",DisplayOrder = 9,GroupAlias = "brand",Status = true },
                    new Slide() {Name="Slide 10",Image="/client-side/images/brand10.png",Url="#",DisplayOrder = 10,GroupAlias = "brand",Status = true },
                    new Slide() {Name="Slide 11",Image="/client-side/images/brand11.png",Url="#",DisplayOrder = 11,GroupAlias = "brand",Status = true },
                };
                _context.Slides.AddRange(slides);
            }

            if (_context.Sizes.Count() == 0)
            {
                List<Size> listSize = new List<Size>()
                {
                    new Size() { Name="XXL" },
                    new Size() { Name="XL"},
                    new Size() { Name="L" },
                    new Size() { Name="M" },
                    new Size() { Name="S" },
                    new Size() { Name="XS" }
                };
                _context.Sizes.AddRange(listSize);
            }

            //if (_context.ProductCategories.Count() == 0)
            //{
            //    List<ProductCategory> listProductCategory = new List<ProductCategory>()
            //    {
            //        new ProductCategory() {  Name="Men shirt",SeoAlias="men-shirt",ParentId = null,Status=Status.Active,SortOrder=1,
            //            Products = new List<Product>()
            //            {
            //                new Product(){Name = "Product 1",DateCreated=DateTime.Now, DateModified=DateTime.Now,Image="/client-side/images/products/product-1.jpg",SeoAlias = "san-pham-1",Price = 1000,Status = Status.Active,OriginalPrice = 1000},
            //                new Product(){Name = "Product 2",DateCreated=DateTime.Now, DateModified=DateTime.Now,Image="/client-side/images/products/product-1.jpg",SeoAlias = "san-pham-2",Price = 1000,Status = Status.Active,OriginalPrice = 1000},
            //                new Product(){Name = "Product 3",DateCreated=DateTime.Now, DateModified=DateTime.Now,Image="/client-side/images/products/product-1.jpg",SeoAlias = "san-pham-3",Price = 1000,Status = Status.Active,OriginalPrice = 1000},
            //                new Product(){Name = "Product 4",DateCreated=DateTime.Now, DateModified=DateTime.Now,Image="/client-side/images/products/product-1.jpg",SeoAlias = "san-pham-4",Price = 1000,Status = Status.Active,OriginalPrice = 1000},
            //                new Product(){Name = "Product 5",DateCreated=DateTime.Now, DateModified=DateTime.Now,Image="/client-side/images/products/product-1.jpg",SeoAlias = "san-pham-5",Price = 1000,Status = Status.Active,OriginalPrice = 1000},
            //            }
            //        },
            //        new ProductCategory() { Name="Women shirt",SeoAlias="women-shirt",ParentId = null,Status=Status.Active ,SortOrder=2,
            //            Products = new List<Product>()
            //            {
            //                new Product(){Name = "Product 6",DateCreated=DateTime.Now, DateModified=DateTime.Now,Image="/client-side/images/products/product-1.jpg",SeoAlias = "san-pham-6",Price = 1000,Status = Status.Active,OriginalPrice = 1000},
            //                new Product(){Name = "Product 7",DateCreated=DateTime.Now, DateModified=DateTime.Now,Image="/client-side/images/products/product-1.jpg",SeoAlias = "san-pham-7",Price = 1000,Status = Status.Active,OriginalPrice = 1000},
            //                new Product(){Name = "Product 8",DateCreated=DateTime.Now, DateModified=DateTime.Now,Image="/client-side/images/products/product-1.jpg",SeoAlias = "san-pham-8",Price = 1000,Status = Status.Active,OriginalPrice = 1000},
            //                new Product(){Name = "Product 9",DateCreated=DateTime.Now, DateModified=DateTime.Now,Image="/client-side/images/products/product-1.jpg",SeoAlias = "san-pham-9",Price = 1000,Status = Status.Active,OriginalPrice = 1000},
            //                new Product(){Name = "Product 10",DateCreated=DateTime.Now, DateModified=DateTime.Now,Image="/client-side/images/products/product-1.jpg",SeoAlias = "san-pham-10",Price = 1000,Status = Status.Active,OriginalPrice = 1000},
            //            }},
            //        new ProductCategory() { Name="Men shoes",SeoAlias="men-shoes",ParentId = null,Status=Status.Active ,SortOrder=3,
            //            Products = new List<Product>()
            //            {
            //                new Product(){Name = "Product 11",DateCreated=DateTime.Now, DateModified=DateTime.Now,Image="/client-side/images/products/product-1.jpg",SeoAlias = "san-pham-11",Price = 1000,Status = Status.Active,OriginalPrice = 1000},
            //                new Product(){Name = "Product 12",DateCreated=DateTime.Now, DateModified=DateTime.Now,Image="/client-side/images/products/product-1.jpg",SeoAlias = "san-pham-12",Price = 1000,Status = Status.Active,OriginalPrice = 1000},
            //                new Product(){Name = "Product 13",DateCreated=DateTime.Now, DateModified=DateTime.Now,Image="/client-side/images/products/product-1.jpg",SeoAlias = "san-pham-13",Price = 1000,Status = Status.Active,OriginalPrice = 1000},
            //                new Product(){Name = "Product 14",DateCreated=DateTime.Now, DateModified=DateTime.Now,Image="/client-side/images/products/product-1.jpg",SeoAlias = "san-pham-14",Price = 1000,Status = Status.Active,OriginalPrice = 1000},
            //                new Product(){Name = "Product 15",DateCreated=DateTime.Now, DateModified=DateTime.Now,Image="/client-side/images/products/product-1.jpg",SeoAlias = "san-pham-15",Price = 1000,Status = Status.Active,OriginalPrice = 1000},
            //            }},
            //        new ProductCategory() { Name="Woment shoes",SeoAlias="women-shoes",ParentId = null,Status=Status.Active,SortOrder=4,
            //            Products = new List<Product>()
            //            {
            //                new Product(){Name = "Product 16",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="/client-side/images/products/product-1.jpg",SeoAlias = "san-pham-16",Price = 1000,Status = Status.Active,OriginalPrice = 1000},
            //                new Product(){Name = "Product 17",DateCreated=DateTime.Now, DateModified=DateTime.Now,Image="/client-side/images/products/product-1.jpg",SeoAlias = "san-pham-17",Price = 1000,Status = Status.Active,OriginalPrice = 1000},
            //                new Product(){Name = "Product 18",DateCreated=DateTime.Now, DateModified=DateTime.Now,Image="/client-side/images/products/product-1.jpg",SeoAlias = "san-pham-18",Price = 1000,Status = Status.Active,OriginalPrice = 1000},
            //                new Product(){Name = "Product 19",DateCreated=DateTime.Now, DateModified=DateTime.Now,Image="/client-side/images/products/product-1.jpg",SeoAlias = "san-pham-19",Price = 1000,Status = Status.Active,OriginalPrice = 1000},
            //                new Product(){Name = "Product 20",DateCreated=DateTime.Now, DateModified=DateTime.Now,Image="/client-side/images/products/product-1.jpg",SeoAlias = "san-pham-20",Price = 1000,Status = Status.Active,OriginalPrice = 1000},
            //            }}
            //    };
            //    _context.ProductCategories.AddRange(listProductCategory);
            //}

            if (!_context.SystemConfigs.Any(x => x.Id == "HomeTitle"))
            {
                _context.SystemConfigs.Add(new SystemConfig()
                {
                    Id = "HomeTitle",
                    Name = "Home's title",
                    Value1 = "OSM Homepage",
                    Status = Status.Active
                });
            }
            if (!_context.SystemConfigs.Any(x => x.Id == "HomeMetaKeyword"))
            {
                _context.SystemConfigs.Add(new SystemConfig()
                {
                    Id = "HomeMetaKeyword",
                    Name = "Home Keyword",
                    Value1 = "shopping, sales",
                    Status = Status.Active
                });
            }
            if (!_context.SystemConfigs.Any(x => x.Id == "HomeMetaDescription"))
            {
                _context.SystemConfigs.Add(new SystemConfig()
                {
                    Id = "HomeMetaDescription",
                    Name = "Home Description",
                    Value1 = "Home OSM",
                    Status = Status.Active
                });
            }
            if (!_context.Contacts.Any())
            {
                _context.Contacts.Add(new Contact()
                {
                    Id = CommonConstants.DefaultContactId,
                    Address = "Đường Nam Kỳ Khởi Nghĩa, Phường Hòa Phú, Thành phố mới Bình Dương, Tỉnh Bình Dương",
                    Email = "osm@gmail.com",
                    Name = "Online Shopping Mart",
                    Phone = "035 2978 519",
                    Status = Status.Active,
                    Website = "http://osm.com",
                    Lat = 11.0609094,
                    Lng = 106.6699562
                });
            }
            if (_context.ProductCategories.Count() == 0)
            {
                List<ProductCategory> listProductCategory = new List<ProductCategory>()
                {
                     new ProductCategory() {DateCreated = DateTime.Now, DateModified = DateTime.Now, Name ="PC & PC Accessories",SeoAlias="pc-pc-accessories",ParentId = null,Status=Status.Active,SortOrder=2,   },
                     new ProductCategory() {DateCreated = DateTime.Now, DateModified = DateTime.Now, Name="Women's Fashion",SeoAlias="women-fashion",ParentId = null,Status=Status.Active,SortOrder=2,   Products = new List<Product>(){ } },
                     new ProductCategory() {DateCreated = DateTime.Now, DateModified = DateTime.Now, Name="Men's Fashion",SeoAlias="men-fashion",ParentId = null,Status=Status.Active,SortOrder=2,   Products = new List<Product>(){ } },
                     new ProductCategory() {DateCreated = DateTime.Now, DateModified = DateTime.Now, Name="Home & Lifestyle",SeoAlias="home-lifestyle",ParentId = null,Status=Status.Active,SortOrder=1 ,   Products = new List<Product>(){ } },
                     new ProductCategory() {DateCreated = DateTime.Now, DateModified = DateTime.Now, Name="Clocks",SeoAlias="clocks",ParentId = null,Status=Status.Active,SortOrder=1 ,   Products = new List<Product>(){
                               new Product(){Name = "Clock 1",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="/client-side/images/products/clocks/clock1.jpg",SeoAlias = "clock-1",Price = 150000,Status = Status.Active,OriginalPrice = 120000},
                               new Product(){Name = "Clock 2",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="/client-side/images/products/clocks/clock2.jpg",SeoAlias = "clock-2",Price = 200000,Status = Status.Active,OriginalPrice = 180000},
                               new Product(){Name = "Clock 3",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="/client-side/images/products/clocks/clock3.jpg",SeoAlias = "clock-3",Price = 350000,Status = Status.Active,OriginalPrice = 290000},
                               new Product(){Name = "Clock 4",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="/client-side/images/products/clocks/clock4.jpg",SeoAlias = "clock-4",Price = 550000,Status = Status.Active,OriginalPrice = 450000},
                               new Product(){Name = "Clock 5",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="/client-side/images/products/clocks/clock5.jpg",SeoAlias = "clock-5",Price = 750000,Status = Status.Active,OriginalPrice = 650000},
                               new Product(){Name = "Clock 6",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="/client-side/images/products/clocks/clock6.jpg",SeoAlias = "clock-6",Price = 950000,Status = Status.Active,OriginalPrice = 800000},
                               new Product(){Name = "Clock 7",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="/client-side/images/products/clocks/clock7.jpg",SeoAlias = "clock-7",Price = 1500000,Status = Status.Active,OriginalPrice = 1200000},
                               new Product(){Name = "Clock 8",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="/client-side/images/products/clocks/clock8.jpg",SeoAlias = "clock-8",Price = 2500000,Status = Status.Active,OriginalPrice = 2300000},
                               new Product(){Name = "Clock 9",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="/client-side/images/products/clocks/clock9.jpg",SeoAlias = "clock-9",Price = 3500000,Status = Status.Active,OriginalPrice = 2900000},
                               new Product(){Name = "Clock 10",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="/client-side/images/products/clocks/clock10.jpg",SeoAlias = "clock-10",Price = 4500000,Status = Status.Active,OriginalPrice = 4200000},
                               new Product(){Name = "Clock 11",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="/client-side/images/products/clocks/clock11.jpg",SeoAlias = "clock-11",Price = 5500000,Status = Status.Active,OriginalPrice = 5200000},
                               new Product(){Name = "Clock 12",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="/client-side/images/products/clocks/clock12.jpg",SeoAlias = "clock-12",Price = 6500000,Status = Status.Active,OriginalPrice = 6000000},
                     } },
                     new ProductCategory() {DateCreated = DateTime.Now, DateModified = DateTime.Now, Name="Books",SeoAlias="book",ParentId = null,Status=Status.Active,SortOrder=1 ,   Products = new List<Product>(){
                               new Product(){Name = "Book 1",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="/client-side/images/productsbooks/book1.jpg",SeoAlias = "book-1",Price = 150000,Status = Status.Active,OriginalPrice = 120000},
                               new Product(){Name = "Book 2",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="/client-side/images/products/books/book2.jpg",SeoAlias = "book-2",Price = 150000,Status = Status.Active,OriginalPrice = 120000},
                               new Product(){Name = "Book 3",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="/client-side/images/products/books/book3.jpg",SeoAlias = "book-3",Price = 150000,Status = Status.Active,OriginalPrice = 120000},
                               new Product(){Name = "Book 4",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="/client-side/images/products/books/book4.jpg",SeoAlias = "book-4",Price = 150000,Status = Status.Active,OriginalPrice = 120000},
                               new Product(){Name = "Book 5",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="/client-side/images/products/books/book5.jpg",SeoAlias = "book-5",Price = 150000,Status = Status.Active,OriginalPrice = 120000},
                               new Product(){Name = "Book 6",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="/client-side/images/products/books/book6.jpg",SeoAlias = "book-6",Price = 150000,Status = Status.Active,OriginalPrice = 120000},
                               new Product(){Name = "Book 7",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="/client-side/images/products/books/book7.jpg",SeoAlias = "book-7",Price = 150000,Status = Status.Active,OriginalPrice = 120000},
                               new Product(){Name = "Book 8",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="/client-side/images/products/books/book8.jpg",SeoAlias = "book-8",Price = 150000,Status = Status.Active,OriginalPrice = 120000},
                               new Product(){Name = "Book 9",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="/client-side/images/products/books/book9.jpg",SeoAlias = "book-9",Price = 150000,Status = Status.Active,OriginalPrice = 120000},
                               new Product(){Name = "Book 10",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="/client-side/images/products/books/book10.jpg",SeoAlias = "book-10",Price = 150000,Status = Status.Active,OriginalPrice = 120000},
                               new Product(){Name = "Book 11",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="/client-side/images/products/books/book11.jpg",SeoAlias = "book-11",Price = 150000,Status = Status.Active,OriginalPrice = 120000},
                               new Product(){Name = "Book 12",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="/client-side/images/products/books/book12.jpg",SeoAlias = "book-12",Price = 150000,Status = Status.Active,OriginalPrice = 120000},
                     } },
                     new ProductCategory() {DateCreated = DateTime.Now, DateModified = DateTime.Now, Name="Toys",SeoAlias="toys",ParentId = null,Status=Status.Active,SortOrder=1 ,   Products = new List<Product>(){
                               new Product(){Name = "Toy 1",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="/client-side/images/products/toys/toy1.jpg",SeoAlias = "toy-1",Price = 150000,Status = Status.Active,OriginalPrice = 120000},
                               new Product(){Name = "Toy 2",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="/client-side/images/products/toys/toy2.jpg",SeoAlias = "toy-2",Price = 150000,Status = Status.Active,OriginalPrice = 120000},
                               new Product(){Name = "Toy 3",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="/client-side/images/products/toys/toy3.jpg",SeoAlias = "toy-3",Price = 150000,Status = Status.Active,OriginalPrice = 120000},
                               new Product(){Name = "Toy 4",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="/client-side/images/products/toys/toy4.jpg",SeoAlias = "toy-4",Price = 150000,Status = Status.Active,OriginalPrice = 120000},
                               new Product(){Name = "Toy 5",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="/client-side/images/products/toys/toy5.jpg",SeoAlias = "toy-5",Price = 150000,Status = Status.Active,OriginalPrice = 120000},
                               new Product(){Name = "Toy 6",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="/client-side/images/products/toys/toy6.jpg",SeoAlias = "toy-6",Price = 150000,Status = Status.Active,OriginalPrice = 120000},
                               new Product(){Name = "Toy 7",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="/client-side/images/products/toys/toy7.jpg",SeoAlias = "toy-7",Price = 150000,Status = Status.Active,OriginalPrice = 120000},
                               new Product(){Name = "Toy 8",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="/client-side/images/products/toys/toy8.jpg",SeoAlias = "toy-8",Price = 150000,Status = Status.Active,OriginalPrice = 120000},
                               new Product(){Name = "Toy 9",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="/client-side/images/products/toys/toy9.jpg",SeoAlias = "toy-9",Price = 150000,Status = Status.Active,OriginalPrice = 120000},
                               new Product(){Name = "Toy 10",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="/client-side/images/products/toys/toy10.jpg",SeoAlias = "toy-10",Price = 150000,Status = Status.Active,OriginalPrice = 120000},
                               new Product(){Name = "Toy 11",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="/client-side/images/products/toys/toy11.jpg",SeoAlias = "toy-11",Price = 150000,Status = Status.Active,OriginalPrice = 120000},
                               new Product(){Name = "Toy 12",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="/client-side/images/products/toys/toy12.jpg",SeoAlias = "toy-12",Price = 150000,Status = Status.Active,OriginalPrice = 120000},
                     } },

                          new ProductCategory() {DateCreated = DateTime.Now, DateModified = DateTime.Now, Name ="Mouse",SeoAlias="mouse",ParentId = null,Status=Status.Active,SortOrder=1,   Products = new List<Product>(){} },
                          new ProductCategory() {DateCreated = DateTime.Now, DateModified = DateTime.Now, Name ="Keyboard",SeoAlias="keyboard",ParentId = null,Status=Status.Active,SortOrder=1,   Products = new List<Product>(){} },
                          new ProductCategory() {DateCreated = DateTime.Now, DateModified = DateTime.Now, Name ="Headphone",SeoAlias="headphone",ParentId = null,Status=Status.Active,SortOrder=1,   Products = new List<Product>(){} },




                          //new ProductCategory() {DateCreated = DateTime.Now, DateModified = DateTime.Now, Name ="Shirts",SeoAlias="Women-shirts",ParentId = null,Status=Status.Active,SortOrder=1,   Products = new List<Product>(){
                          //       new Product(){Name = "Women's Shirt 1",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="/client-side/images/products/women-shirts/shirt1.jpg",SeoAlias = "women-shirt-1",Price = 150000,Status = Status.Active,OriginalPrice = 120000},
                          //     new Product(){Name = "Women's Shirt 2",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="/client-side/images/products/women-shirts/shirt2.jpg",SeoAlias = "women-shirt-2",Price = 150000,Status = Status.Active,OriginalPrice = 120000},
                          //     new Product(){Name = "Women's Shirt 3",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="/client-side/images/products/women-shirts/shirt3.jpg",SeoAlias = "women-shirt-3",Price = 150000,Status = Status.Active,OriginalPrice = 120000},
                          //     new Product(){Name = "Women's Shirt 4",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="/client-side/images/products/women-shirts/shirt4.jpg",SeoAlias = "women-shirt-4",Price = 150000,Status = Status.Active,OriginalPrice = 120000},
                          //     new Product(){Name = "Women's Shirt 5",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="/client-side/images/products/women-shirts/shirt5.jpg",SeoAlias = "women-shirt-5",Price = 150000,Status = Status.Active,OriginalPrice = 120000},
                          //     new Product(){Name = "Women's Shirt 6",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="/client-side/images/products/women-shirts/shirt6.jpg",SeoAlias = "women-shirt-6",Price = 150000,Status = Status.Active,OriginalPrice = 120000},
                          //     new Product(){Name = "Women's Shirt 7",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="/client-side/images/products/women-shirts/shirt7.jpg",SeoAlias = "women-shirt-7",Price = 150000,Status = Status.Active,OriginalPrice = 120000},
                          //     new Product(){Name = "Women's Shirt 8",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="/client-side/images/products/women-shirts/shirt8.jpg",SeoAlias = "women-shirt-8",Price = 150000,Status = Status.Active,OriginalPrice = 120000},
                          //     new Product(){Name = "Women's Shirt 9",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="/client-side/images/products/women-shirts/shirt9.jpg",SeoAlias = "women-shirt-9",Price = 150000,Status = Status.Active,OriginalPrice = 120000},
                          //     new Product(){Name = "Women's Shirt 10",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="/client-side/images/products/women-shirts/shirt10.jpg",SeoAlias = "women-shirt-10",Price = 150000,Status = Status.Active,OriginalPrice = 120000},
                          //     new Product(){Name = "Women's Shirt 11",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="/client-side/images/products/women-shirts/shirt11.jpg",SeoAlias = "women-shirt-11",Price = 150000,Status = Status.Active,OriginalPrice = 120000},
                          //     new Product(){Name = "Women's Shirt 12",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="/client-side/images/products/women-shirts/shirt12.jpg",SeoAlias = "women-shirt-12",Price = 150000,Status = Status.Active,OriginalPrice = 120000},
                          // } },
                          //new ProductCategory() {DateCreated = DateTime.Now, DateModified = DateTime.Now, Name ="T-Shirts",SeoAlias="Women-t-shirt",ParentId = null,Status=Status.Active,SortOrder=1,   Products = new List<Product>(){
                          //     new Product(){Name = "Women's T-Shirt 1",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="/client-side/images/products/women-tshirts/tshirt1.jpg",SeoAlias = "women-t-shirt-1",Price = 150000,Status = Status.Active,OriginalPrice = 120000},
                          //     new Product(){Name = "Women's T-Shirt 2",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="/client-side/images/products/women-tshirts/tshirt2.jpg",SeoAlias = "women-t-shirt-2",Price = 150000,Status = Status.Active,OriginalPrice = 120000},
                          //     new Product(){Name = "Women's T-Shirt 3",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="/client-side/images/products/women-tshirts/tshirt3.jpg",SeoAlias = "women-t-shirt-3",Price = 150000,Status = Status.Active,OriginalPrice = 120000},
                          //     new Product(){Name = "Women's T-Shirt 4",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="/client-side/images/products/women-tshirts/tshirt4.jpg",SeoAlias = "women-t-shirt-4",Price = 150000,Status = Status.Active,OriginalPrice = 120000},
                          //     new Product(){Name = "Women's T-Shirt 5",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="/client-side/images/products/women-tshirts/tshirt5.jpg",SeoAlias = "women-t-shirt-5",Price = 150000,Status = Status.Active,OriginalPrice = 120000},
                          //     new Product(){Name = "Women's T-Shirt 6",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="/client-side/images/products/women-tshirts/tshirt6.jpg",SeoAlias = "women-t-shirt-6",Price = 150000,Status = Status.Active,OriginalPrice = 120000},
                          //     new Product(){Name = "Women's T-Shirt 7",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="/client-side/images/products/women-tshirts/tshirt7.jpg",SeoAlias = "women-t-shirt-7",Price = 150000,Status = Status.Active,OriginalPrice = 120000},
                          //     new Product(){Name = "Women's T-Shirt 8",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="/client-side/images/products/women-tshirts/tshirt8.jpg",SeoAlias = "women-t-shirt-8",Price = 150000,Status = Status.Active,OriginalPrice = 120000},
                          //     new Product(){Name = "Women's T-Shirt 9",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="/client-side/images/products/women-tshirts/tshirt9.jpg",SeoAlias = "women-t-shirt-9",Price = 150000,Status = Status.Active,OriginalPrice = 120000},
                          //     new Product(){Name = "Women's T-Shirt 10",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="/client-side/images/products/women-tshirts/tshirt10.jpg",SeoAlias = "women-t-shirt-10",Price = 150000,Status = Status.Active,OriginalPrice = 120000},
                          //     new Product(){Name = "Women's T-Shirt 11",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="/client-side/images/products/women-tshirts/tshirt11.jpg",SeoAlias = "women-t-shirt-11",Price = 150000,Status = Status.Active,OriginalPrice = 120000},
                          //     new Product(){Name = "Women's T-Shirt 12",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="/client-side/images/products/women-tshirts/tshirt12.jpg",SeoAlias = "women-t-shirt-12",Price = 150000,Status = Status.Active,OriginalPrice = 120000},
                          // } },
                          //new ProductCategory() {DateCreated = DateTime.Now, DateModified = DateTime.Now, Name ="Trousers & Jeans",SeoAlias="Women-trousers-jeans",ParentId = null,Status=Status.Active,SortOrder=1,   Products = new List<Product>(){
                          //     new Product(){Name = "Women's Trouser 1",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="/client-side/images/products/women-trousers/trouser1.jpg",SeoAlias = "women-trouser-1",Price = 150000,Status = Status.Active,OriginalPrice = 120000},
                          //     new Product(){Name = "Women's Trouser 2",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="/client-side/images/products/women-trousers/trouser2.jpg",SeoAlias = "women-trouser-2",Price = 150000,Status = Status.Active,OriginalPrice = 120000},
                          //     new Product(){Name = "Women's Trouser 3",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="/client-side/images/products/women-trousers/trouser3.jpg",SeoAlias = "women-trouser-3",Price = 150000,Status = Status.Active,OriginalPrice = 120000},
                          //     new Product(){Name = "Women's Trouser 4",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="/client-side/images/products/women-trousers/trouser4.jpg",SeoAlias = "women-trouser-4",Price = 150000,Status = Status.Active,OriginalPrice = 120000},
                          //     new Product(){Name = "Women's Trouser 5",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="/client-side/images/products/women-trousers/trouser5.jpg",SeoAlias = "women-trouser-5",Price = 150000,Status = Status.Active,OriginalPrice = 120000},
                          //     new Product(){Name = "Women's Trouser 6",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="/client-side/images/products/women-trousers/trouser6.jpg",SeoAlias = "women-trouser-6",Price = 150000,Status = Status.Active,OriginalPrice = 120000},
                          //     new Product(){Name = "Women's Trouser 7",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="/client-side/images/products/women-trousers/trouser7.jpg",SeoAlias = "women-trouser-7",Price = 150000,Status = Status.Active,OriginalPrice = 120000},
                          //     new Product(){Name = "Women's Trouser 8",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="/client-side/images/products/women-trousers/trouser8.jpg",SeoAlias = "women-trouser-8",Price = 150000,Status = Status.Active,OriginalPrice = 120000},
                          //     new Product(){Name = "Women's Trouser 9",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="/client-side/images/products/women-trousers/trouser9.jpg",SeoAlias = "women-trouser-9",Price = 150000,Status = Status.Active,OriginalPrice = 120000},
                          //     new Product(){Name = "Women's Trouser 10",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="/client-side/images/products/women-trousers/trouser10.jpg",SeoAlias = "women-trouser-10",Price = 150000,Status = Status.Active,OriginalPrice = 120000},
                          //     new Product(){Name = "Women's Trouser 11",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="/client-side/images/products/women-trousers/trouser11.jpg",SeoAlias = "women-trouser-11",Price = 150000,Status = Status.Active,OriginalPrice = 120000},
                          //     new Product(){Name = "Women's Trouser 12",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="/client-side/images/products/women-trousers/trouser12.jpg",SeoAlias = "women-trouser-12",Price = 150000,Status = Status.Active,OriginalPrice = 120000},

                          //     new Product(){Name = "Women's Jean 1",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="/client-side/images/products/women-jeans/jean1.jpg",SeoAlias = "women-jean-1",Price = 150000,Status = Status.Active,OriginalPrice = 120000},
                          //     new Product(){Name = "Women's Jean 2",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="/client-side/images/products/women-jeans/jean2.jpg",SeoAlias = "women-jean-2",Price = 150000,Status = Status.Active,OriginalPrice = 120000},
                          //     new Product(){Name = "Women's Jean 3",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="/client-side/images/products/women-jeans/jean3.jpg",SeoAlias = "women-jean-3",Price = 150000,Status = Status.Active,OriginalPrice = 120000},
                          //     new Product(){Name = "Women's Jean 4",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="/client-side/images/products/women-jeans/jean4.jpg",SeoAlias = "women-jean-4",Price = 150000,Status = Status.Active,OriginalPrice = 120000},
                          //     new Product(){Name = "Women's Jean 5",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="/client-side/images/products/women-jeans/jean5.jpg",SeoAlias = "women-jean-5",Price = 150000,Status = Status.Active,OriginalPrice = 120000},
                          //     new Product(){Name = "Women's Jean 6",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="/client-side/images/products/women-jeans/jean6.jpg",SeoAlias = "women-jean-6",Price = 150000,Status = Status.Active,OriginalPrice = 120000},
                          //     new Product(){Name = "Women's Jean 7",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="/client-side/images/products/women-jeans/jean7.jpg",SeoAlias = "women-jean-7",Price = 150000,Status = Status.Active,OriginalPrice = 120000},
                          //     new Product(){Name = "Women's Jean 8",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="/client-side/images/products/women-jeans/jean8.jpg",SeoAlias = "women-jean-8",Price = 150000,Status = Status.Active,OriginalPrice = 120000},
                          //     new Product(){Name = "Women's Jean 9",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="/client-side/images/products/women-jeans/jean9.jpg",SeoAlias = "women-jean-9",Price = 150000,Status = Status.Active,OriginalPrice = 120000},
                          //     new Product(){Name = "Women's Jean 10",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="/client-side/images/products/women-jeans/jean10.jpg",SeoAlias = "women-jean-10",Price = 150000,Status = Status.Active,OriginalPrice = 120000},
                          //     new Product(){Name = "Women's Jean 11",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="/client-side/images/products/women-jeans/jean11.jpg",SeoAlias = "women-jean-11",Price = 150000,Status = Status.Active,OriginalPrice = 120000},
                          //     new Product(){Name = "Women's Jean 12",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="/client-side/images/products/women-jeans/jean12.jpg",SeoAlias = "women-jean-12",Price = 150000,Status = Status.Active,OriginalPrice = 120000},
                          // } },


                          //new ProductCategory() {DateCreated = DateTime.Now, DateModified = DateTime.Now, Name ="Shirts",SeoAlias="men-shirts",ParentId = 4,Status=Status.Active,SortOrder=2,   Products = new List<Product>(){
                          //       new Product(){Name = "Men's Shirt 1",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "men-shirt-1",Price = 150000,Status = Status.Active,OriginalPrice = 120000},
                          //     new Product(){Name = "Men's Shirt 2",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "men-shirt-2",Price = 150000,Status = Status.Active,OriginalPrice = 120000},
                          //     new Product(){Name = "Men's Shirt 3",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "men-shirt-3",Price = 150000,Status = Status.Active,OriginalPrice = 120000},
                          //     new Product(){Name = "Men's Shirt 4",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "men-shirt-4",Price = 150000,Status = Status.Active,OriginalPrice = 120000},
                          //     new Product(){Name = "Men's Shirt 5",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "men-shirt-5",Price = 150000,Status = Status.Active,OriginalPrice = 120000},
                          //     new Product(){Name = "Men's Shirt 6",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "men-shirt-6",Price = 150000,Status = Status.Active,OriginalPrice = 120000},
                          //     new Product(){Name = "Men's Shirt 7",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "men-shirt-7",Price = 150000,Status = Status.Active,OriginalPrice = 120000},
                          //     new Product(){Name = "Men's Shirt 8",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "men-shirt-8",Price = 150000,Status = Status.Active,OriginalPrice = 120000},
                          //     new Product(){Name = "Men's Shirt 9",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "men-shirt-9",Price = 150000,Status = Status.Active,OriginalPrice = 120000},
                          //     new Product(){Name = "Men's Shirt 10",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "men-shirt-10",Price = 150000,Status = Status.Active,OriginalPrice = 120000},
                          //     new Product(){Name = "Men's Shirt 11",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "men-shirt-11",Price = 150000,Status = Status.Active,OriginalPrice = 120000},
                          //     new Product(){Name = "Men's Shirt 12",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "men-shirt-12",Price = 150000,Status = Status.Active,OriginalPrice = 120000},
                          // } },
                          //new ProductCategory() {DateCreated = DateTime.Now, DateModified = DateTime.Now, Name ="T-Shirts",SeoAlias="men-t-shirt",ParentId = 4,Status=Status.Active,SortOrder=2,   Products = new List<Product>(){
                          //     new Product(){Name = "Men's T-Shirt 1",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "men-t-shirt-1",Price = 150000,Status = Status.Active,OriginalPrice = 120000},
                          //     new Product(){Name = "Men's T-Shirt 2",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "men-t-shirt-2",Price = 150000,Status = Status.Active,OriginalPrice = 120000},
                          //     new Product(){Name = "Men's T-Shirt 3",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "men-t-shirt-3",Price = 150000,Status = Status.Active,OriginalPrice = 120000},
                          //     new Product(){Name = "Men's T-Shirt 4",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "men-t-shirt-4",Price = 150000,Status = Status.Active,OriginalPrice = 120000},
                          //     new Product(){Name = "Men's T-Shirt 5",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "men-t-shirt-5",Price = 150000,Status = Status.Active,OriginalPrice = 120000},
                          //     new Product(){Name = "Men's T-Shirt 6",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "men-t-shirt-6",Price = 150000,Status = Status.Active,OriginalPrice = 120000},
                          //     new Product(){Name = "Men's T-Shirt 7",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "men-t-shirt-7",Price = 150000,Status = Status.Active,OriginalPrice = 120000},
                          //     new Product(){Name = "Men's T-Shirt 8",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "men-t-shirt-8",Price = 150000,Status = Status.Active,OriginalPrice = 120000},
                          //     new Product(){Name = "Men's T-Shirt 9",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "men-t-shirt-9",Price = 150000,Status = Status.Active,OriginalPrice = 120000},
                          //     new Product(){Name = "Men's T-Shirt 10",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "men-t-shirt-10",Price = 150000,Status = Status.Active,OriginalPrice = 120000},
                          //     new Product(){Name = "Men's T-Shirt 11",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "men-t-shirt-11",Price = 150000,Status = Status.Active,OriginalPrice = 120000},
                          //     new Product(){Name = "Men's T-Shirt 12",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "men-t-shirt-12",Price = 150000,Status = Status.Active,OriginalPrice = 120000},
                          // } },
                          //new ProductCategory() {DateCreated = DateTime.Now, DateModified = DateTime.Now, Name ="Trousers & Jeans",SeoAlias="men-trousers-jeans",ParentId = 4,Status=Status.Active,SortOrder=2,   Products = new List<Product>(){
                          //     new Product(){Name = "Men's Trouser 1",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "men-trouser-1",Price = 150000,Status = Status.Active,OriginalPrice = 120000},
                          //     new Product(){Name = "Men's Trouser 2",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "men-trouser-2",Price = 150000,Status = Status.Active,OriginalPrice = 120000},
                          //     new Product(){Name = "Men's Trouser 3",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "men-trouser-3",Price = 150000,Status = Status.Active,OriginalPrice = 120000},
                          //     new Product(){Name = "Men's Trouser 4",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "men-trouser-4",Price = 150000,Status = Status.Active,OriginalPrice = 120000},
                          //     new Product(){Name = "Men's Trouser 5",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "men-trouser-5",Price = 150000,Status = Status.Active,OriginalPrice = 120000},
                          //     new Product(){Name = "Men's Trouser 6",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "men-trouser-6",Price = 150000,Status = Status.Active,OriginalPrice = 120000},
                          //     new Product(){Name = "Men's Trouser 7",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "men-trouser-7",Price = 150000,Status = Status.Active,OriginalPrice = 120000},
                          //     new Product(){Name = "Men's Trouser 8",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "men-trouser-8",Price = 150000,Status = Status.Active,OriginalPrice = 120000},
                          //     new Product(){Name = "Men's Trouser 9",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "men-trouser-9",Price = 150000,Status = Status.Active,OriginalPrice = 120000},
                          //     new Product(){Name = "Men's Trouser 10",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "men-trouser-10",Price = 150000,Status = Status.Active,OriginalPrice = 120000},
                          //     new Product(){Name = "Men's Trouser 11",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "men-trouser-11",Price = 150000,Status = Status.Active,OriginalPrice = 120000},
                          //     new Product(){Name = "Men's Trouser 12",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "men-trouser-12",Price = 150000,Status = Status.Active,OriginalPrice = 120000},

                          //     new Product(){Name = "Men's Jean 1",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "men-jean-1",Price = 150000,Status = Status.Active,OriginalPrice = 120000},
                          //     new Product(){Name = "Men's Jean 2",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "men-jean-2",Price = 150000,Status = Status.Active,OriginalPrice = 120000},
                          //     new Product(){Name = "Men's Jean 3",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "men-jean-3",Price = 150000,Status = Status.Active,OriginalPrice = 120000},
                          //     new Product(){Name = "Men's Jean 4",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "men-jean-4",Price = 150000,Status = Status.Active,OriginalPrice = 120000},
                          //     new Product(){Name = "Men's Jean 5",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "men-jean-5",Price = 150000,Status = Status.Active,OriginalPrice = 120000},
                          //     new Product(){Name = "Men's Jean 6",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "men-jean-6",Price = 150000,Status = Status.Active,OriginalPrice = 120000},
                          //     new Product(){Name = "Men's Jean 7",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "men-jean-7",Price = 150000,Status = Status.Active,OriginalPrice = 120000},
                          //     new Product(){Name = "Men's Jean 8",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "men-jean-8",Price = 150000,Status = Status.Active,OriginalPrice = 120000},
                          //     new Product(){Name = "Men's Jean 9",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "men-jean-9",Price = 150000,Status = Status.Active,OriginalPrice = 120000},
                          //     new Product(){Name = "Men's Jean 10",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "men-jean-10",Price = 150000,Status = Status.Active,OriginalPrice = 120000},
                          //     new Product(){Name = "Men's Jean 11",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "men-jean-11",Price = 150000,Status = Status.Active,OriginalPrice = 120000},
                          //     new Product(){Name = "Men's Jean 12",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "men-jean-12",Price = 150000,Status = Status.Active,OriginalPrice = 120000},
                          // } },

                          //new ProductCategory() {DateCreated = DateTime.Now, DateModified = DateTime.Now, Name ="Chairs",SeoAlias="chairs",ParentId = 6,Status=Status.Active,SortOrder=2,   Products = new List<Product>(){
                          //          new Product(){Name = "Chair 1",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "chair-1",Price = 200000,Status = Status.Active,OriginalPrice = 134000},
                          //          new Product(){Name = "Chair 2",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "chair-2",Price = 400000,Status = Status.Active,OriginalPrice = 320000},
                          //          new Product(){Name = "Chair 3",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "chair-3",Price = 800000,Status = Status.Active,OriginalPrice = 670000},
                          //          new Product(){Name = "Chair 4",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "chair-4",Price = 1600000,Status = Status.Active,OriginalPrice = 1400000},
                          //          new Product(){Name = "Chair 5",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "chair-5",Price = 220000,Status = Status.Active,OriginalPrice = 2000000},
                          //          new Product(){Name = "Chair 6",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "chair-6",Price = 2500000,Status = Status.Active,OriginalPrice = 2300000},
                          //          new Product(){Name = "Chair 7",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "chair-7",Price = 2200000,Status = Status.Active,OriginalPrice = 2000000},
                          //          new Product(){Name = "Chair 8",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "chair-8",Price = 2500000,Status = Status.Active,OriginalPrice = 2300000},
                          //} },
                          //new ProductCategory() {DateCreated = DateTime.Now, DateModified = DateTime.Now, Name ="Tables",SeoAlias="tables",ParentId = 6,Status=Status.Active,SortOrder=2,   Products = new List<Product>(){
                          //          new Product(){Name = "Table 1",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "table-1",Price = 200000,Status = Status.Active,OriginalPrice = 134000},
                          //          new Product(){Name = "Table 2",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "table-2",Price = 400000,Status = Status.Active,OriginalPrice = 320000},
                          //          new Product(){Name = "Table 3",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "table-3",Price = 800000,Status = Status.Active,OriginalPrice = 670000},
                          //          new Product(){Name = "Table 4",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "table-4",Price = 1600000,Status = Status.Active,OriginalPrice = 1400000},
                          //          new Product(){Name = "Table 5",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "table-5",Price = 220000,Status = Status.Active,OriginalPrice = 2000000},
                          //          new Product(){Name = "Table 6",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "table-6",Price = 2500000,Status = Status.Active,OriginalPrice = 2300000},
                          //          new Product(){Name = "Table 7",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "table-7",Price = 2200000,Status = Status.Active,OriginalPrice = 2000000},
                          //          new Product(){Name = "Table 8",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "table-8",Price = 2500000,Status = Status.Active,OriginalPrice = 2300000},
                          //} },
                          //new ProductCategory() {DateCreated = DateTime.Now, DateModified = DateTime.Now, Name ="Lamps",SeoAlias="lamps",ParentId = 6,Status=Status.Active,SortOrder=2,   Products = new List<Product>(){
                          //          new Product(){Name = "Lamp 1",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "lamp-1",Price = 200000,Status = Status.Active,OriginalPrice = 134000},
                          //          new Product(){Name = "Lamp 2",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "lamp-2",Price = 400000,Status = Status.Active,OriginalPrice = 320000},
                          //          new Product(){Name = "Lamp 3",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "lamp-3",Price = 800000,Status = Status.Active,OriginalPrice = 670000},
                          //          new Product(){Name = "Lamp 4",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "lamp-4",Price = 1600000,Status = Status.Active,OriginalPrice = 1400000},
                          //          new Product(){Name = "Lamp 5",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "lamp-5",Price = 220000,Status = Status.Active,OriginalPrice = 2000000},
                          //          new Product(){Name = "Lamp 6",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "lamp-6",Price = 2500000,Status = Status.Active,OriginalPrice = 2300000},
                          //          new Product(){Name = "Lamp 7",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "lamp-7",Price = 2200000,Status = Status.Active,OriginalPrice = 2000000},
                          //          new Product(){Name = "Lamp 8",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "lamp-8",Price = 2500000,Status = Status.Active,OriginalPrice = 2300000},
                          //} },

                          //new ProductCategory() {DateCreated = DateTime.Now, DateModified = DateTime.Now, Name ="Camera",SeoAlias="camera",ParentId = 7,Status=Status.Active,SortOrder=2,   Products = new List<Product>(){
                          //          new Product(){Name = "Camera 1",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "camera-1",Price = 2500000,Status = Status.Active,OriginalPrice = 2200000},
                          //          new Product(){Name = "Camera 2",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "camera-2",Price = 5000000,Status = Status.Active,OriginalPrice = 4200000},
                          //          new Product(){Name = "Camera 3",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "camera-3",Price = 8000000,Status = Status.Active,OriginalPrice = 6700000},
                          //          new Product(){Name = "Camera 4",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "camera-4",Price = 16000000,Status = Status.Active,OriginalPrice = 14000000},
                          //          new Product(){Name = "Camera 5",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "camera-5",Price = 22000000,Status = Status.Active,OriginalPrice = 20000000},
                          //          new Product(){Name = "Camera 6",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "camera-6",Price = 25000000,Status = Status.Active,OriginalPrice = 23000000},
                          //          new Product(){Name = "Camera 7",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "camera-7",Price = 32000000,Status = Status.Active,OriginalPrice = 30000000},
                          //          new Product(){Name = "Camera 8",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "camera-8",Price = 45000000,Status = Status.Active,OriginalPrice = 45000000},
                          //} },
                          //new ProductCategory() {DateCreated = DateTime.Now, DateModified = DateTime.Now, Name ="Accessories",SeoAlias="camera-accessories",ParentId = 7,Status=Status.Active,SortOrder=2,   Products = new List<Product>(){
                          //          new Product(){Name = "Camera Battery 1",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "camera-battery-1",Price = 2500000,Status = Status.Active,OriginalPrice = 2200000},
                          //          new Product(){Name = "Camera Battery 2",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "camera-battery-2",Price = 2500000,Status = Status.Active,OriginalPrice = 2200000},
                          //          new Product(){Name = "Camera Battery 3",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "camera-battery-3",Price = 2500000,Status = Status.Active,OriginalPrice = 2200000},
                          //          new Product(){Name = "Camera Battery 4",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "camera-battery-4",Price = 2500000,Status = Status.Active,OriginalPrice = 2200000},
                          //          new Product(){Name = "Camera Battery 5",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "camera-battery-5",Price = 2500000,Status = Status.Active,OriginalPrice = 2200000},
                          //          new Product(){Name = "Camera Battery 6",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "camera-battery-6",Price = 2500000,Status = Status.Active,OriginalPrice = 2200000},
                          //          new Product(){Name = "Camera Battery 7",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "camera-battery-7",Price = 2500000,Status = Status.Active,OriginalPrice = 2200000},
                          //          new Product(){Name = "Camera Battery 8",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "camera-battery-8",Price = 2500000,Status = Status.Active,OriginalPrice = 2200000},

                          //          new Product(){Name = "Camera Len 1",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "camera-len-1",Price = 2500000,Status = Status.Active,OriginalPrice = 2200000},
                          //          new Product(){Name = "Camera Len 2",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "camera-len-2",Price = 2500000,Status = Status.Active,OriginalPrice = 2200000},
                          //          new Product(){Name = "Camera Len 3",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "camera-len-3",Price = 2500000,Status = Status.Active,OriginalPrice = 2200000},
                          //          new Product(){Name = "Camera Len 4",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "camera-len-4",Price = 2500000,Status = Status.Active,OriginalPrice = 2200000},
                          //          new Product(){Name = "Camera Len 5",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "camera-len-5",Price = 2500000,Status = Status.Active,OriginalPrice = 2200000},
                          //          new Product(){Name = "Camera Len 6",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "camera-len-6",Price = 2500000,Status = Status.Active,OriginalPrice = 2200000},
                          //          new Product(){Name = "Camera Len 7",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "camera-len-7",Price = 2500000,Status = Status.Active,OriginalPrice = 2200000},
                          //          new Product(){Name = "Camera Len 8",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "camera-len-8",Price = 2500000,Status = Status.Active,OriginalPrice = 2200000},
                          //} },

                          //      new ProductCategory() { DateCreated = DateTime.Now, DateModified = DateTime.Now, Name ="Dell",SeoAlias="laptop-dell",ParentId = 11,Status=Status.Active,SortOrder=3,   Products = new List<Product>(){
                          //          new Product(){Name = "Laptop Dell 1",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "laptop-dell-1",Price = 15000000,Status = Status.Active,OriginalPrice = 12500000},
                          //          new Product(){Name = "Laptop Dell 2",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "laptop-dell-2",Price = 25000000,Status = Status.Active,OriginalPrice = 22500000},
                          //          new Product(){Name = "Laptop Dell 3",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "laptop-dell-3",Price = 30000000,Status = Status.Active,OriginalPrice = 27500000},
                          //          new Product(){Name = "Laptop Dell 4",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "laptop-dell-4",Price = 37000000,Status = Status.Active,OriginalPrice = 32500000},
                          //          new Product(){Name = "Laptop Dell 5",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "laptop-dell-5",Price = 50000000,Status = Status.Active,OriginalPrice = 45000000},
                          //          new Product(){Name = "Laptop Dell 6",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "laptop-dell-6",Price = 75000000,Status = Status.Active,OriginalPrice = 62500000},
                          //      } },
                          //      new ProductCategory() { DateCreated = DateTime.Now, DateModified = DateTime.Now, Name ="Asus",SeoAlias="laptop-asus",ParentId = 11,Status=Status.Active,SortOrder=3,   Products = new List<Product>(){
                          //          new Product(){Name = "Laptop Asus 1",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "laptop-asus-1",Price = 15000000,Status = Status.Active,OriginalPrice = 12500000},
                          //          new Product(){Name = "Laptop Asus 2",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "laptop-asus-2",Price = 25000000,Status = Status.Active,OriginalPrice = 22500000},
                          //          new Product(){Name = "Laptop Asus 3",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "laptop-asus-3",Price = 30000000,Status = Status.Active,OriginalPrice = 27500000},
                          //          new Product(){Name = "Laptop Asus 4",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "laptop-asus-4",Price = 37000000,Status = Status.Active,OriginalPrice = 32500000},
                          //          new Product(){Name = "Laptop Asus 5",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "laptop-asus-5",Price = 50000000,Status = Status.Active,OriginalPrice = 45000000},
                          //          new Product(){Name = "Laptop Asus 6",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "laptop-asus-6",Price = 75000000,Status = Status.Active,OriginalPrice = 62500000},
                          //      } },
                          //      new ProductCategory() { DateCreated = DateTime.Now, DateModified = DateTime.Now, Name ="MSI",SeoAlias="laptop-msi",ParentId = 11,Status=Status.Active,SortOrder=3,   Products = new List<Product>(){
                          //          new Product(){Name = "Laptop MSI 1",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "laptop-msi-1",Price = 15000000,Status = Status.Active,OriginalPrice = 12500000},
                          //          new Product(){Name = "Laptop MSI 2",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "laptop-msi-2",Price = 25000000,Status = Status.Active,OriginalPrice = 22500000},
                          //          new Product(){Name = "Laptop MSI 3",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "laptop-msi-3",Price = 30000000,Status = Status.Active,OriginalPrice = 27500000},
                          //          new Product(){Name = "Laptop MSI 4",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "laptop-msi-4",Price = 37000000,Status = Status.Active,OriginalPrice = 32500000},
                          //          new Product(){Name = "Laptop MSI 5",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "laptop-msi-5",Price = 50000000,Status = Status.Active,OriginalPrice = 45000000},
                          //          new Product(){Name = "Laptop MSI 6",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "laptop-msi-6",Price = 75000000,Status = Status.Active,OriginalPrice = 62500000},
                          //      } },
                          //      new ProductCategory() { DateCreated = DateTime.Now, DateModified = DateTime.Now, Name ="Dell",SeoAlias="pc-case-dell",ParentId = 12,Status=Status.Active,SortOrder=3,   Products = new List<Product>(){
                          //          new Product(){Name = "PC case Dell 1",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "pc-case-dell-1",Price = 15000000,Status = Status.Active,OriginalPrice = 12500000},
                          //          new Product(){Name = "PC case Dell 2",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "pc-case-dell-2",Price = 25000000,Status = Status.Active,OriginalPrice = 22500000},
                          //          new Product(){Name = "PC case Dell 3",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "pc-case-dell-3",Price = 30000000,Status = Status.Active,OriginalPrice = 27500000},
                          //          new Product(){Name = "PC case Dell 4",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "pc-case-dell-4",Price = 37000000,Status = Status.Active,OriginalPrice = 32500000},
                          //          new Product(){Name = "PC case Dell 5",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "pc-case-dell-5",Price = 50000000,Status = Status.Active,OriginalPrice = 45000000},
                          //          new Product(){Name = "PC case Dell 6",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "pc-case-dell-6",Price = 75000000,Status = Status.Active,OriginalPrice = 62500000},
                          //      } },
                          //      new ProductCategory() { DateCreated = DateTime.Now, DateModified = DateTime.Now, Name ="Asus",SeoAlias="pc-case-asus",ParentId = 12,Status=Status.Active,SortOrder=3,   Products = new List<Product>(){
                          //          new Product(){Name = "PC case Asus 1",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "pc-case-asus-1",Price = 15000000,Status = Status.Active,OriginalPrice = 12500000},
                          //          new Product(){Name = "PC case Asus 2",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "pc-case-asus-2",Price = 25000000,Status = Status.Active,OriginalPrice = 22500000},
                          //          new Product(){Name = "PC case Asus 3",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "pc-case-asus-3",Price = 30000000,Status = Status.Active,OriginalPrice = 27500000},
                          //          new Product(){Name = "PC case Asus 4",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "pc-case-asus-4",Price = 37000000,Status = Status.Active,OriginalPrice = 32500000},
                          //          new Product(){Name = "PC case Asus 5",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "pc-case-asus-5",Price = 50000000,Status = Status.Active,OriginalPrice = 45000000},
                          //          new Product(){Name = "PC case Asus 6",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "pc-case-asus-6",Price = 75000000,Status = Status.Active,OriginalPrice = 62500000},
                          //      } },
                          //      new ProductCategory() { DateCreated = DateTime.Now, DateModified = DateTime.Now, Name ="MSI",SeoAlias="pc-case-msi",ParentId = 12,Status=Status.Active,SortOrder=3,   Products = new List<Product>(){
                          //          new Product(){Name = "PC case MSI 1",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "pc-case-msi-1",Price = 15000000,Status = Status.Active,OriginalPrice = 12500000},
                          //          new Product(){Name = "PC case MSI 2",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "pc-case-msi-2",Price = 25000000,Status = Status.Active,OriginalPrice = 22500000},
                          //          new Product(){Name = "PC case MSI 3",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "pc-case-msi-3",Price = 30000000,Status = Status.Active,OriginalPrice = 27500000},
                          //          new Product(){Name = "PC case MSI 4",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "pc-case-msi-4",Price = 37000000,Status = Status.Active,OriginalPrice = 32500000},
                          //          new Product(){Name = "PC case MSI 5",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "pc-case-msi-5",Price = 50000000,Status = Status.Active,OriginalPrice = 45000000},
                          //          new Product(){Name = "PC case MSI 6",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "pc-case-msi-6",Price = 75000000,Status = Status.Active,OriginalPrice = 62500000},
                          //      } },

                          //      new ProductCategory() { DateCreated = DateTime.Now, DateModified = DateTime.Now, Name ="Razor",SeoAlias="razor-mouse",ParentId = 13,Status=Status.Active,SortOrder=3,   Products = new List<Product>(){
                          //          new Product(){Name = "Razor Mouse 1",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "razor-mouse-1",Price = 500000,Status = Status.Active,OriginalPrice = 340000},
                          //          new Product(){Name = "Razor Mouse 2",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "razor-mouse-2",Price = 750000,Status = Status.Active,OriginalPrice = 650000},
                          //          new Product(){Name = "Razor Mouse 3",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "razor-mouse-3",Price = 1250000,Status = Status.Active,OriginalPrice = 1100000},
                          //          new Product(){Name = "Razor Mouse 4",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "razor-mouse-4",Price = 2400000,Status = Status.Active,OriginalPrice = 2200000},
                          //          new Product(){Name = "Razor Mouse 5",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "razor-mouse-5",Price = 2200000,Status = Status.Active,OriginalPrice = 2000000},
                          //          new Product(){Name = "Razor Mouse 6",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "razor-mouse-6",Price = 2500000,Status = Status.Active,OriginalPrice = 2300000},
                          //      } },
                          //      new ProductCategory() { DateCreated = DateTime.Now, DateModified = DateTime.Now, Name ="Logitech",SeoAlias="logitech-mouse",ParentId = 13,Status=Status.Active,SortOrder=3,   Products = new List<Product>(){
                          //          new Product(){Name = "Logitech Mouse G102",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "logitech-mouse-g102",Price = 400000,Status = Status.Active,OriginalPrice = 300000},
                          //          new Product(){Name = "Logitech Mouse G304",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "logitech-mouse-g304",Price = 1000000,Status = Status.Active,OriginalPrice = 750000},
                          //          new Product(){Name = "Logitech Mouse G403",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "logitech-mouse-g403",Price = 1250000,Status = Status.Active,OriginalPrice = 1100000},
                          //          new Product(){Name = "Logitech Mouse G502",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "logitech-mouse-g502",Price = 1500000,Status = Status.Active,OriginalPrice = 1250000},
                          //          new Product(){Name = "Logitech Mouse G603",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "logitech-mouse-g603",Price = 1800000,Status = Status.Active,OriginalPrice = 1600000},
                          //          new Product(){Name = "Logitech Mouse G703",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "logitech-mouse-g703",Price = 1900000,Status = Status.Active,OriginalPrice = 1700000},
                          //          new Product(){Name = "Logitech Mouse G903",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "logitech-mouse-g903",Price = 3100000,Status = Status.Active,OriginalPrice = 2500000},} },
                          //      new ProductCategory() { DateCreated = DateTime.Now, DateModified = DateTime.Now, Name ="Cosair",SeoAlias="corsair-mouse",ParentId = 13,Status=Status.Active,SortOrder=3,   Products = new List<Product>(){
                          //          new Product(){Name = "Corsair Mouse 1",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "corsair-mouse-1",Price = 400000,Status = Status.Active,OriginalPrice = 300000},
                          //          new Product(){Name = "Corsair Mouse 2",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "corsair-mouse-2",Price = 1000000,Status = Status.Active,OriginalPrice = 750000},
                          //          new Product(){Name = "Corsair Mouse 3",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "corsair-mouse-3",Price = 1250000,Status = Status.Active,OriginalPrice = 1100000},
                          //          new Product(){Name = "Corsair Mouse 4",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "corsair-mouse-4",Price = 1500000,Status = Status.Active,OriginalPrice = 1250000},
                          //          new Product(){Name = "Corsair Mouse 5",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "corsair-mouse-5",Price = 1800000,Status = Status.Active,OriginalPrice = 1600000},
                          //          new Product(){Name = "Corsair Mouse 6",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "corsair-mouse-6",Price = 1900000,Status = Status.Active,OriginalPrice = 1700000},
                          //          new Product(){Name = "Corsair Mouse 7",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "corsair-mouse-7",Price = 3100000,Status = Status.Active,OriginalPrice = 2500000},
                          //      } },
                          //      new ProductCategory() { DateCreated = DateTime.Now, DateModified = DateTime.Now, Name ="Fulhen",SeoAlias="fulhen-mouse",ParentId = 13,Status=Status.Active,SortOrder=3,   Products = new List<Product>(){
                          //          new Product(){Name = "Fulhen Mouse 1",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "fulhen-mouse-1",Price = 400000,Status = Status.Active,OriginalPrice = 300000},
                          //          new Product(){Name = "Fulhen Mouse 2",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "fulhen-mouse-2",Price = 1000000,Status = Status.Active,OriginalPrice = 750000},
                          //          new Product(){Name = "Fulhen Mouse 3",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "fulhen-mouse-3",Price = 1250000,Status = Status.Active,OriginalPrice = 1100000},
                          //          new Product(){Name = "Fulhen Mouse 4",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "fulhen-mouse-4",Price = 1500000,Status = Status.Active,OriginalPrice = 1250000},
                          //          new Product(){Name = "Fulhen Mouse 5",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "fulhen-mouse-5",Price = 1800000,Status = Status.Active,OriginalPrice = 1600000},
                          //          new Product(){Name = "Fulhen Mouse 6",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "fulhen-mouse-6",Price = 1900000,Status = Status.Active,OriginalPrice = 1700000},
                          //          new Product(){Name = "Fulhen Mouse 7",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "fulhen-mouse-7",Price = 3100000,Status = Status.Active,OriginalPrice = 2500000},
                          //      } },
                          //      new ProductCategory() { DateCreated = DateTime.Now, DateModified = DateTime.Now, Name ="Zowie",SeoAlias="zowie-mouse",ParentId = 13,Status=Status.Active,SortOrder=3,   Products = new List<Product>(){
                          //          new Product(){Name = "Zowie Mouse 1",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "zowie-mouse-1",Price = 400000,Status = Status.Active,OriginalPrice = 300000},
                          //          new Product(){Name = "Zowie Mouse 2",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "zowie-mouse-2",Price = 1000000,Status = Status.Active,OriginalPrice = 750000},
                          //          new Product(){Name = "Zowie Mouse 3",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "zowie-mouse-3",Price = 1250000,Status = Status.Active,OriginalPrice = 1100000},
                          //          new Product(){Name = "Zowie Mouse 4",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "zowie-mouse-4",Price = 1500000,Status = Status.Active,OriginalPrice = 1250000},
                          //          new Product(){Name = "Zowie Mouse 5",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "zowie-mouse-5",Price = 1800000,Status = Status.Active,OriginalPrice = 1600000},
                          //          new Product(){Name = "Zowie Mouse 6",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "zowie-mouse-6",Price = 1900000,Status = Status.Active,OriginalPrice = 1700000},
                          //          new Product(){Name = "Zowie Mouse 7",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "zowie-mouse-7",Price = 3100000,Status = Status.Active,OriginalPrice = 2500000},
                          //      } },
                          //      new ProductCategory() { DateCreated = DateTime.Now, DateModified = DateTime.Now, Name ="SteelSeries",SeoAlias="steelseries-mouse",ParentId = 13,Status=Status.Active,SortOrder=3,   Products = new List<Product>(){
                          //          new Product(){Name = "SteelSeries Mouse 1",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "steelseries-mouse-1",Price = 400000,Status = Status.Active,OriginalPrice = 300000},
                          //          new Product(){Name = "SteelSeries Mouse 2",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "steelseries-mouse-2",Price = 1000000,Status = Status.Active,OriginalPrice = 750000},
                          //          new Product(){Name = "SteelSeries Mouse 3",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "steelseries-mouse-3",Price = 1250000,Status = Status.Active,OriginalPrice = 1100000},
                          //          new Product(){Name = "SteelSeries Mouse 4",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "steelseries-mouse-4",Price = 1500000,Status = Status.Active,OriginalPrice = 1250000},
                          //          new Product(){Name = "SteelSeries Mouse 5",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "steelseries-mouse-5",Price = 1800000,Status = Status.Active,OriginalPrice = 1600000},
                          //          new Product(){Name = "SteelSeries Mouse 6",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "steelseries-mouse-6",Price = 1900000,Status = Status.Active,OriginalPrice = 1700000},
                          //          new Product(){Name = "SteelSeries Mouse 7",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "steelseries-mouse-7",Price = 3100000,Status = Status.Active,OriginalPrice = 2500000},
                          //      } },
                          //      new ProductCategory() { DateCreated = DateTime.Now, DateModified = DateTime.Now, Name ="Dare-U",SeoAlias="dare-u-mouse",ParentId = 13,Status=Status.Active,SortOrder=3,   Products = new List<Product>(){
                          //           new Product(){Name = "Dare-U Mouse 1",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "dare-u-mouse-1",Price = 400000,Status = Status.Active,OriginalPrice = 300000},
                          //          new Product(){Name = "Dare-U Mouse 2",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "dare-u-mouse-2",Price = 1000000,Status = Status.Active,OriginalPrice = 750000},
                          //          new Product(){Name = "Dare-U Mouse 3",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "dare-u-mouse-3",Price = 1250000,Status = Status.Active,OriginalPrice = 1100000},
                          //          new Product(){Name = "Dare-U Mouse 4",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "dare-u-mouse-4",Price = 1500000,Status = Status.Active,OriginalPrice = 1250000},
                          //          new Product(){Name = "Dare-U Mouse 5",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "dare-u-mouse-5",Price = 1800000,Status = Status.Active,OriginalPrice = 1600000},
                          //          new Product(){Name = "Dare-U Mouse 6",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "dare-u-mouse-6",Price = 1900000,Status = Status.Active,OriginalPrice = 1700000},
                          //          new Product(){Name = "Dare-U Mouse 7",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "dare-u-mouse-7",Price = 3100000,Status = Status.Active,OriginalPrice = 2500000},

                          //      } },

                          //      new ProductCategory() { DateCreated = DateTime.Now, DateModified = DateTime.Now, Name ="Razor",SeoAlias="razor-keyboard",ParentId = 14,Status=Status.Active,SortOrder=3,   Products = new List<Product>(){
                          //          new Product(){Name = "Razor Keyboard 1",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "razor-keyboard-1",Price = 500000,Status = Status.Active,OriginalPrice = 340000},
                          //          new Product(){Name = "Razor Keyboard 2",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "razor-keyboard-2",Price = 750000,Status = Status.Active,OriginalPrice = 650000},
                          //          new Product(){Name = "Razor Keyboard 3",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "razor-keyboard-3",Price = 1250000,Status = Status.Active,OriginalPrice = 1100000},
                          //          new Product(){Name = "Razor Keyboard 4",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "razor-keyboard-4",Price = 2400000,Status = Status.Active,OriginalPrice = 2200000},
                          //          new Product(){Name = "Razor Keyboard 5",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "razor-keyboard-5",Price = 2200000,Status = Status.Active,OriginalPrice = 2000000},
                          //          new Product(){Name = "Razor Keyboard 6",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "razor-keyboard-6",Price = 2500000,Status = Status.Active,OriginalPrice = 2300000},} },
                          //      new ProductCategory() { DateCreated = DateTime.Now, DateModified = DateTime.Now, Name ="Logitech",SeoAlias="logitech-keyboard",ParentId = 14,Status=Status.Active,SortOrder=3,   Products = new List<Product>(){

                          //          new Product(){Name = "Logitech Keyboard G413",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "logitech-keyboard-g413",Price = 1250000,Status = Status.Active,OriginalPrice = 1100000},
                          //          new Product(){Name = "Logitech Keyboard G513",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "logitech-keyboard-g513",Price = 1500000,Status = Status.Active,OriginalPrice = 1250000},
                          //          new Product(){Name = "Logitech Keyboard G Pro",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "logitech-keyboard-g-pro",Price = 1800000,Status = Status.Active,OriginalPrice = 1600000},
                          //          new Product(){Name = "Logitech Keyboard G613",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "logitech-keyboard-g613",Price = 1900000,Status = Status.Active,OriginalPrice = 1700000},
                          //          new Product(){Name = "Logitech Keyboard G913",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "logitech-keyboard-g913",Price = 3100000,Status = Status.Active,OriginalPrice = 2500000},

                          //      } },
                          //      new ProductCategory() { DateCreated = DateTime.Now, DateModified = DateTime.Now, Name ="Cosair",SeoAlias="corsair-keyboard",ParentId = 14,Status=Status.Active,SortOrder=3,   Products = new List<Product>(){
                          //          new Product(){Name = "Corsair Keyboard 1",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "corsair-keyboard-1",Price = 400000,Status = Status.Active,OriginalPrice = 300000},
                          //          new Product(){Name = "Corsair Keyboard 2",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "corsair-keyboard-2",Price = 1000000,Status = Status.Active,OriginalPrice = 750000},
                          //          new Product(){Name = "Corsair Keyboard 3",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "corsair-keyboard-3",Price = 1250000,Status = Status.Active,OriginalPrice = 1100000},
                          //          new Product(){Name = "Corsair Keyboard 4",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "corsair-keyboard-4",Price = 1500000,Status = Status.Active,OriginalPrice = 1250000},
                          //          new Product(){Name = "Corsair Keyboard 5",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "corsair-keyboard-5",Price = 1800000,Status = Status.Active,OriginalPrice = 1600000},
                          //          new Product(){Name = "Corsair Keyboard 6",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "corsair-keyboard-6",Price = 1900000,Status = Status.Active,OriginalPrice = 1700000},
                          //          new Product(){Name = "Corsair Keyboard 7",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "corsair-keyboard-7",Price = 3100000,Status = Status.Active,OriginalPrice = 2500000},
                          //      } },
                          //      new ProductCategory() { DateCreated = DateTime.Now, DateModified = DateTime.Now, Name ="Fulhen",SeoAlias="fulhen-keyboard",ParentId = 14,Status=Status.Active,SortOrder=3,   Products = new List<Product>(){
                          //          new Product(){Name = "Fulhen Keyboard 1",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "fulhen-keyboard-1",Price = 400000,Status = Status.Active,OriginalPrice = 300000},
                          //          new Product(){Name = "Fulhen Keyboard 2",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "fulhen-keyboard-2",Price = 1000000,Status = Status.Active,OriginalPrice = 750000},
                          //          new Product(){Name = "Fulhen Keyboard 3",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "fulhen-keyboard-3",Price = 1250000,Status = Status.Active,OriginalPrice = 1100000},
                          //          new Product(){Name = "Fulhen Keyboard 4",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "fulhen-keyboard-4",Price = 1500000,Status = Status.Active,OriginalPrice = 1250000},
                          //          new Product(){Name = "Fulhen Keyboard 5",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "fulhen-keyboard-5",Price = 1800000,Status = Status.Active,OriginalPrice = 1600000},
                          //          new Product(){Name = "Fulhen Keyboard 6",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "fulhen-keyboard-6",Price = 1900000,Status = Status.Active,OriginalPrice = 1700000},
                          //          new Product(){Name = "Fulhen Keyboard 7",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "fulhen-keyboard-7",Price = 3100000,Status = Status.Active,OriginalPrice = 2500000},
                          //      } },
                          //      new ProductCategory() { DateCreated = DateTime.Now, DateModified = DateTime.Now, Name ="SteelSeries",SeoAlias="steelseries-keyboard",ParentId = 14,Status=Status.Active,SortOrder=3,   Products = new List<Product>(){
                          //          new Product(){Name = "SteelSeries Keyboard 1",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "steelseries-keyboard-1",Price = 400000,Status = Status.Active,OriginalPrice = 300000},
                          //          new Product(){Name = "SteelSeries Keyboard 2",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "steelseries-keyboard-2",Price = 1000000,Status = Status.Active,OriginalPrice = 750000},
                          //          new Product(){Name = "SteelSeries Keyboard 3",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "steelseries-keyboard-3",Price = 1250000,Status = Status.Active,OriginalPrice = 1100000},
                          //          new Product(){Name = "SteelSeries Keyboard 4",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "steelseries-keyboard-4",Price = 1500000,Status = Status.Active,OriginalPrice = 1250000},
                          //          new Product(){Name = "SteelSeries Keyboard 5",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "steelseries-keyboard-5",Price = 1800000,Status = Status.Active,OriginalPrice = 1600000},
                          //          new Product(){Name = "SteelSeries Keyboard 6",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "steelseries-keyboard-6",Price = 1900000,Status = Status.Active,OriginalPrice = 1700000},
                          //          new Product(){Name = "SteelSeries Keyboard 7",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "steelseries-keyboard-7",Price = 3100000,Status = Status.Active,OriginalPrice = 2500000},
                          //      } },
                          //      new ProductCategory() { DateCreated = DateTime.Now, DateModified = DateTime.Now, Name ="Dare-U",SeoAlias="dare-u-keyboard",ParentId = 14,Status=Status.Active,SortOrder=3,   Products = new List<Product>(){
                          //         new Product(){Name = "Dare-U Keyboard 1",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "dare-u-keyboard-1",Price = 400000,Status = Status.Active,OriginalPrice = 300000},
                          //          new Product(){Name = "Dare-U Keyboard 2",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "dare-u-keyboard-2",Price = 1000000,Status = Status.Active,OriginalPrice = 750000},
                          //          new Product(){Name = "Dare-U Keyboard 3",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "dare-u-keyboard-3",Price = 1250000,Status = Status.Active,OriginalPrice = 1100000},
                          //          new Product(){Name = "Dare-U Keyboard 4",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "dare-u-keyboard-4",Price = 1500000,Status = Status.Active,OriginalPrice = 1250000},
                          //          new Product(){Name = "Dare-U Keyboard 5",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "dare-u-keyboard-5",Price = 1800000,Status = Status.Active,OriginalPrice = 1600000},
                          //          new Product(){Name = "Dare-U Keyboard 6",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "dare-u-keyboard-6",Price = 1900000,Status = Status.Active,OriginalPrice = 1700000},
                          //          new Product(){Name = "Dare-U Keyboard 7",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "dare-u-keyboard-7",Price = 3100000,Status = Status.Active,OriginalPrice = 2500000},
                          //      } },

                          //      new ProductCategory() { DateCreated = DateTime.Now, DateModified = DateTime.Now, Name ="Razor",SeoAlias="razor-headphone",ParentId = 15,Status=Status.Active,SortOrder=3,   Products = new List<Product>(){
                          //        new Product(){Name = "Razor Headphone 1",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "razor-headphone-1",Price = 500000,Status = Status.Active,OriginalPrice = 340000},
                          //          new Product(){Name = "Razor Headphone 2",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "razor-headphone-2",Price = 750000,Status = Status.Active,OriginalPrice = 650000},
                          //          new Product(){Name = "Razor Headphone 3",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "razor-headphone-3",Price = 1250000,Status = Status.Active,OriginalPrice = 1100000},
                          //          new Product(){Name = "Razor Headphone 4",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "razor-headphone-4",Price = 2400000,Status = Status.Active,OriginalPrice = 2200000},
                          //          new Product(){Name = "Razor Headphone 5",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "razor-headphone-5",Price = 2200000,Status = Status.Active,OriginalPrice = 2000000},
                          //          new Product(){Name = "Razor Headphone 6",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "razor-headphone-6",Price = 2500000,Status = Status.Active,OriginalPrice = 2300000},} },
                          //      new ProductCategory() { DateCreated = DateTime.Now, DateModified = DateTime.Now, Name ="Logitech",SeoAlias="logitech-headphone",ParentId = 15,Status=Status.Active,SortOrder=3,   Products = new List<Product>(){
                          //          new Product(){Name = "Logitech Headphone G433",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "logitech-headphone-g433",Price = 1250000,Status = Status.Active,OriginalPrice = 1100000},
                          //          new Product(){Name = "Logitech Headphone G533",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "logitech-headphone-g533",Price = 1500000,Status = Status.Active,OriginalPrice = 1250000},
                          //          new Product(){Name = "Logitech Headphone G Pro",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "logitech-headphone-g-pro",Price = 1800000,Status = Status.Active,OriginalPrice = 1600000},
                          //          new Product(){Name = "Logitech Headphone G633",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "logitech-headphone-g633",Price = 1900000,Status = Status.Active,OriginalPrice = 1700000},
                          //          new Product(){Name = "Logitech Headphone G993",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "logitech-headphone-g933",Price = 3100000,Status = Status.Active,OriginalPrice = 2500000},} },
                          //      new ProductCategory() { DateCreated = DateTime.Now, DateModified = DateTime.Now, Name ="Cosair",SeoAlias="corsair-headphone",ParentId = 15,Status=Status.Active,SortOrder=3,   Products = new List<Product>(){
                          //          new Product(){Name = "Corsair Headphone 1",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "corsair-headphone-1",Price = 400000,Status = Status.Active,OriginalPrice = 300000},
                          //          new Product(){Name = "Corsair Headphone 2",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "corsair-headphone-2",Price = 1000000,Status = Status.Active,OriginalPrice = 750000},
                          //          new Product(){Name = "Corsair Headphone 3",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "corsair-headphone-3",Price = 1250000,Status = Status.Active,OriginalPrice = 1100000},
                          //          new Product(){Name = "Corsair Headphone 4",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "corsair-headphone-4",Price = 1500000,Status = Status.Active,OriginalPrice = 1250000},
                          //          new Product(){Name = "Corsair Headphone 5",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "corsair-headphone-5",Price = 1800000,Status = Status.Active,OriginalPrice = 1600000},
                          //          new Product(){Name = "Corsair Headphone 6",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "corsair-headphone-6",Price = 1900000,Status = Status.Active,OriginalPrice = 1700000},
                          //          new Product(){Name = "Corsair Headphone 7",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "corsair-headphone-7",Price = 3100000,Status = Status.Active,OriginalPrice = 2500000},} },
                          //      new ProductCategory() { DateCreated = DateTime.Now, DateModified = DateTime.Now, Name ="Fulhen",SeoAlias="fulhen-headphone",ParentId = 15,Status=Status.Active,SortOrder=3,   Products = new List<Product>(){
                          //          new Product(){Name = "Fulhen Headphone 1",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "fulhen-headphone-1",Price = 400000,Status = Status.Active,OriginalPrice = 300000},
                          //          new Product(){Name = "Fulhen Headphone 2",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "fulhen-headphone-2",Price = 1000000,Status = Status.Active,OriginalPrice = 750000},
                          //          new Product(){Name = "Fulhen Headphone 3",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "fulhen-headphone-3",Price = 1250000,Status = Status.Active,OriginalPrice = 1100000},
                          //          new Product(){Name = "Fulhen Headphone 4",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "fulhen-headphone-4",Price = 1500000,Status = Status.Active,OriginalPrice = 1250000},
                          //          new Product(){Name = "Fulhen Headphone 5",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "fulhen-headphone-5",Price = 1800000,Status = Status.Active,OriginalPrice = 1600000},
                          //          new Product(){Name = "Fulhen Headphone 6",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "fulhen-headphone-6",Price = 1900000,Status = Status.Active,OriginalPrice = 1700000},
                          //          new Product(){Name = "Fulhen Headphone 7",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "fulhen-headphone-7",Price = 3100000,Status = Status.Active,OriginalPrice = 2500000},
                          //      } },
                          //      new ProductCategory() { DateCreated = DateTime.Now, DateModified = DateTime.Now, Name ="SteelSeries",SeoAlias="steelseries-headphone",ParentId = 15,Status=Status.Active,SortOrder=3,   Products = new List<Product>(){
                          //          new Product(){Name = "SteelSeries Headphone 1",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "steelseries-headphone-1",Price = 400000,Status = Status.Active,OriginalPrice = 300000},
                          //          new Product(){Name = "SteelSeries Headphone 2",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "steelseries-headphone-2",Price = 1000000,Status = Status.Active,OriginalPrice = 750000},
                          //          new Product(){Name = "SteelSeries Headphone 3",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "steelseries-headphone-3",Price = 1250000,Status = Status.Active,OriginalPrice = 1100000},
                          //          new Product(){Name = "SteelSeries Headphone 4",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "steelseries-headphone-4",Price = 1500000,Status = Status.Active,OriginalPrice = 1250000},
                          //          new Product(){Name = "SteelSeries Headphone 5",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "steelseries-headphone-5",Price = 1800000,Status = Status.Active,OriginalPrice = 1600000},
                          //          new Product(){Name = "SteelSeries Headphone 6",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "steelseries-headphone-6",Price = 1900000,Status = Status.Active,OriginalPrice = 1700000},
                          //          new Product(){Name = "SteelSeries Headphone 7",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "steelseries-headphone-7",Price = 3100000,Status = Status.Active,OriginalPrice = 2500000},
                          //      } },
                          //      new ProductCategory() { DateCreated = DateTime.Now, DateModified = DateTime.Now, Name ="Dare-U",SeoAlias="dare-u-headphone",ParentId = 15,Status=Status.Active,SortOrder=3,   Products = new List<Product>(){
                          //          new Product(){Name = "Dare-U Headphone 1",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "dare-u-headphone-1",Price = 400000,Status = Status.Active,OriginalPrice = 300000},
                          //          new Product(){Name = "Dare-U Headphone 2",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "dare-u-headphone-2",Price = 1000000,Status = Status.Active,OriginalPrice = 750000},
                          //          new Product(){Name = "Dare-U Headphone 3",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "dare-u-headphone-3",Price = 1250000,Status = Status.Active,OriginalPrice = 1100000},
                          //          new Product(){Name = "Dare-U Headphone 4",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "dare-u-headphone-4",Price = 1500000,Status = Status.Active,OriginalPrice = 1250000},
                          //          new Product(){Name = "Dare-U Headphone 5",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "dare-u-headphone-5",Price = 1800000,Status = Status.Active,OriginalPrice = 1600000},
                          //          new Product(){Name = "Dare-U Headphone 6",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "dare-u-headphone-6",Price = 1900000,Status = Status.Active,OriginalPrice = 1700000},
                          //          new Product(){Name = "Dare-U Headphone 7",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "dare-u-headphone-7",Price = 3100000,Status = Status.Active,OriginalPrice = 2500000},
                          //      } },

                          //      new ProductCategory() { DateCreated = DateTime.Now, DateModified = DateTime.Now, Name ="Apple",SeoAlias="apple-phone",ParentId = 16,Status=Status.Active,SortOrder=3,   Products = new List<Product>(){
                          //          new Product(){Name = "Apple Phone 1",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "apple-phone-1",Price = 2000000,Status = Status.Active,OriginalPrice = 1340000},
                          //          new Product(){Name = "Apple Phone 2",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "apple-phone-2",Price = 4000000,Status = Status.Active,OriginalPrice = 3200000},
                          //          new Product(){Name = "Apple Phone 3",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "apple-phone-3",Price = 8000000,Status = Status.Active,OriginalPrice = 6700000},
                          //          new Product(){Name = "Apple Phone 4",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "apple-phone-4",Price = 16000000,Status = Status.Active,OriginalPrice = 14000000},
                          //          new Product(){Name = "Apple Phone 5",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "apple-phone-5",Price = 22000000,Status = Status.Active,OriginalPrice = 20000000},
                          //          new Product(){Name = "Apple Phone 6",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "apple-phone-6",Price = 25000000,Status = Status.Active,OriginalPrice = 23000000},} },
                          //      new ProductCategory() { DateCreated = DateTime.Now, DateModified = DateTime.Now, Name ="XiaoMi",SeoAlias="xiaomi-phone",ParentId = 16,Status=Status.Active,SortOrder=3,   Products = new List<Product>(){
                          //          new Product(){Name = "XiaoMi Phone 1",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "xiaomi-phone-1",Price = 2000000,Status = Status.Active,OriginalPrice = 1340000},
                          //          new Product(){Name = "XiaoMi Phone 2",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "xiaomi-phone-2",Price = 4000000,Status = Status.Active,OriginalPrice = 3200000},
                          //          new Product(){Name = "XiaoMi Phone 3",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "xiaomi-phone-3",Price = 8000000,Status = Status.Active,OriginalPrice = 6700000},
                          //          new Product(){Name = "XiaoMi Phone 4",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "xiaomi-phone-4",Price = 16000000,Status = Status.Active,OriginalPrice = 14000000},
                          //          new Product(){Name = "XiaoMi Phone 5",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "xiaomi-phone-5",Price = 22000000,Status = Status.Active,OriginalPrice = 20000000},
                          //          new Product(){Name = "XiaoMi Phone 6",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "xiaomi-phone-6",Price = 25000000,Status = Status.Active,OriginalPrice = 23000000},} },
                          //      new ProductCategory() { DateCreated = DateTime.Now, DateModified = DateTime.Now, Name ="Samsung",SeoAlias="samsung-phone",ParentId = 16,Status=Status.Active,SortOrder=3,   Products = new List<Product>(){
                          //          new Product(){Name = "Samsung Phone 1",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "samsung-phone-1",Price = 2000000,Status = Status.Active,OriginalPrice = 1340000},
                          //          new Product(){Name = "Samsung Phone 2",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "samsung-phone-2",Price = 4000000,Status = Status.Active,OriginalPrice = 3200000},
                          //          new Product(){Name = "Samsung Phone 3",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "samsung-phone-3",Price = 8000000,Status = Status.Active,OriginalPrice = 6700000},
                          //          new Product(){Name = "Samsung Phone 4",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "samsung-phone-4",Price = 16000000,Status = Status.Active,OriginalPrice = 14000000},
                          //          new Product(){Name = "Samsung Phone 5",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "samsung-phone-5",Price = 22000000,Status = Status.Active,OriginalPrice = 20000000},
                          //          new Product(){Name = "Samsung Phone 6",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "samsung-phone-6",Price = 25000000,Status = Status.Active,OriginalPrice = 23000000},} },
                          //      new ProductCategory() { DateCreated = DateTime.Now, DateModified = DateTime.Now, Name ="Oppo",SeoAlias="oppo-phone",ParentId = 16,Status=Status.Active,SortOrder=3,   Products = new List<Product>(){
                          //          new Product(){Name = "Oppo Phone 1",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "oppo-phone-1",Price = 2000000,Status = Status.Active,OriginalPrice = 1340000},
                          //          new Product(){Name = "Oppo Phone 2",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "oppo-phone-2",Price = 4000000,Status = Status.Active,OriginalPrice = 3200000},
                          //          new Product(){Name = "Oppo Phone 3",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "oppo-phone-3",Price = 8000000,Status = Status.Active,OriginalPrice = 6700000},
                          //          new Product(){Name = "Oppo Phone 4",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "oppo-phone-4",Price = 16000000,Status = Status.Active,OriginalPrice = 14000000},
                          //          new Product(){Name = "Oppo Phone 5",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "oppo-phone-5",Price = 22000000,Status = Status.Active,OriginalPrice = 20000000},
                          //          new Product(){Name = "Oppo Phone 6",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "oppo-phone-6",Price = 25000000,Status = Status.Active,OriginalPrice = 23000000},} },

                          //      new ProductCategory() { DateCreated = DateTime.Now, DateModified = DateTime.Now, Name ="Charger",SeoAlias="phone-charger",ParentId = 17,Status=Status.Active,SortOrder=3,   Products = new List<Product>(){
                          //          new Product(){Name = "Phone Charger 1",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "phone-charger-1",Price = 2000000,Status = Status.Active,OriginalPrice = 1340000},
                          //          new Product(){Name = "Phone Charger 2",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "phone-charger-2",Price = 4000000,Status = Status.Active,OriginalPrice = 3200000},
                          //          new Product(){Name = "Phone Charger 3",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "phone-charger-3",Price = 8000000,Status = Status.Active,OriginalPrice = 6700000},
                          //          new Product(){Name = "Phone Charger 4",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "phone-charger-4",Price = 16000000,Status = Status.Active,OriginalPrice = 14000000},
                          //          new Product(){Name = "Phone Charger 5",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "phone-charger-5",Price = 22000000,Status = Status.Active,OriginalPrice = 20000000},
                          //          new Product(){Name = "Phone Charger 6",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "phone-charger-6",Price = 25000000,Status = Status.Active,OriginalPrice = 23000000},} },
                          //      new ProductCategory() { DateCreated = DateTime.Now, DateModified = DateTime.Now, Name ="Power Banks",SeoAlias="phone-power-bank",ParentId = 17,Status=Status.Active,SortOrder=3,   Products = new List<Product>(){
                          //          new Product(){Name = "Phone Power Bank 1",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "phone-power-bank-1",Price = 200000,Status = Status.Active,OriginalPrice = 134000},
                          //          new Product(){Name = "Phone Power Bank 2",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "phone-power-bank-2",Price = 400000,Status = Status.Active,OriginalPrice = 320000},
                          //          new Product(){Name = "Phone Power Bank 3",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "phone-power-bank-3",Price = 800000,Status = Status.Active,OriginalPrice = 670000},
                          //          new Product(){Name = "Phone Power Bank 4",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "phone-power-bank-4",Price = 1600000,Status = Status.Active,OriginalPrice = 1400000},
                          //          new Product(){Name = "Phone Power Bank 5",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "phone-power-bank-5",Price = 2200000,Status = Status.Active,OriginalPrice = 2000000},
                          //          new Product(){Name = "Phone Power Bank 6",DateCreated=DateTime.Now, DateModified=DateTime.Now, Image="",SeoAlias = "phone-power-bank-6",Price = 2500000,Status = Status.Active,OriginalPrice = 2300000},} },
                          //      //18-19
                          //      //20-22
                          //      //23-25
                          //      //26-28
                                //29-30

                                

                };
                _context.ProductCategories.AddRange(listProductCategory);

            }
            if (_context.Blogs.Count() == 0)
            {
                var blogs = new List<Blog>
                {
                    new Blog(){DateCreated = DateTime.Now, DateModified = DateTime.Now, Name = "Blog Test 1",Status = Status.Active},
                    new Blog(){DateCreated = DateTime.Now, DateModified = DateTime.Now, Name = "Blog Test 2",Status = Status.Active},
                    new Blog(){DateCreated = DateTime.Now, DateModified = DateTime.Now, Name = "Blog Test 3",Status = Status.Active}

                };
                _context.Blogs.AddRange(blogs);
            }
            await _context.SaveChangesAsync();
        }
    }
}