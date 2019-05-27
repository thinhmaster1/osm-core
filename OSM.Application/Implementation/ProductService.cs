﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using OfficeOpenXml;
using OSM.Application.Interfaces;
using OSM.Application.ViewModels.Common;
using OSM.Application.ViewModels.Product;
using OSM.Data.Entities;
using OSM.Data.Enums;
using OSM.Infrastructure.Interfaces;
using OSM.Utilities.Constants;
using OSM.Utilities.Dtos;
using OSM.Utilities.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace OSM.Application.Implementation
{
    public class ProductService : IProductService
    {
        private IRepository<ProductCategory, int> _productCategoryRepository;
        private IRepository<Product, int> _productRepository;
        private IRepository<Tag, string> _tagRepository;
        private IRepository<ProductTag, int> _productTagRepository;
        private IRepository<ProductQuantity, int> _productQuantityRepository;
        private IRepository<ProductImage, int> _productImageRepository;
        private IRepository<WholePrice, int> _wholePriceRepository;
        private IRepository<Color, int> _colorRepository;
        private IRepository<Size, int> _sizeRepository;
        private IUnitOfWork _unitOfWork;

        public ProductService(
            IRepository<Product, int> productRepository,
            IRepository<ProductCategory, int> productCategoryRepository,
            IRepository<Tag, string> tagRepository,
            IRepository<ProductQuantity, int> productQuantityRepository,
            IRepository<ProductImage, int> productImageRepository,
            IRepository<WholePrice, int> wholePriceRepository,
            IRepository<ProductTag, int> productTagRepository,
            IRepository<Color, int> colorRepository,
            IRepository<Size, int> sizeRepository,
            IUnitOfWork unitOfWork)
        {
            _productCategoryRepository = productCategoryRepository;
            _productRepository = productRepository;
            _tagRepository = tagRepository;
            _productTagRepository = productTagRepository;
            _productQuantityRepository = productQuantityRepository;
            _productImageRepository = productImageRepository;
            _wholePriceRepository = wholePriceRepository;
            _colorRepository = colorRepository;
            _sizeRepository = sizeRepository;
            _unitOfWork = unitOfWork;
        }

        public void AddQuantity(int productId, List<ProductQuantityViewModel> quantities)
        {
            _productQuantityRepository.RemoveMultiple(_productQuantityRepository.FindAll(x => x.ProductId == productId).ToList());
            foreach (var quantity in quantities)
            {
                _productQuantityRepository.Add(new ProductQuantity()
                {
                    ProductId = productId,                   
                    SizeId = quantity.SizeId,
                    ColorId = quantity.ColorId,
                    Quantity = quantity.Quantity
                });
            }
        }

        public List<ProductQuantityViewModel> GetQuantities(int productId)
        {
            return _productQuantityRepository.FindAll(x => x.ProductId == productId).ProjectTo<ProductQuantityViewModel>().ToList();
        }

        public ProductViewModel Add(ProductViewModel productVm)
        {
            List<ProductTag> productTags = new List<ProductTag>();
            if (!string.IsNullOrEmpty(productVm.Tags))
            {
                string[] tags = productVm.Tags.Split(',');
                foreach (string t in tags)
                {
                    var tagId = TextHelper.ToUnsignString(t);
                    if (!_tagRepository.FindAll(x => x.Id == tagId).Any())
                    {
                        Tag tag = new Tag
                        {
                            Id = tagId,
                            Name = t,
                            Type = CommonConstants.ProductTag
                        };
                        _tagRepository.Add(tag);
                    }

                    ProductTag productTag = new ProductTag
                    {
                        TagId = tagId
                    };
                    productTags.Add(productTag);
                }
            }
            var product = Mapper.Map<ProductViewModel, Product>(productVm);
            foreach (var productTag in productTags)
            {
                product.ProductTags.Add(productTag);
            }
            _productRepository.Add(product);
            return productVm;
        }

        public void Delete(int id)
        {
            _productRepository.Remove(id);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public List<ProductViewModel> GetAll()
        {
            return _productRepository.FindAll(x => x.ProductCategory).ProjectTo<ProductViewModel>().ToList();
        }

        public PagedResult<ProductViewModel> GetAllPaging(int? categoryId, string keyword, int page, int pageSize, string sortBy)
        {
            var query = _productRepository.FindAll(x => x.Status == Status.Active);
            if (!string.IsNullOrEmpty(keyword))
                query = query.Where(x => x.Name.Contains(keyword));
            var categories = _productCategoryRepository.FindAll();
            if (categoryId.HasValue)
            {
                var level1Items = query.Where(x => x.CategoryId == categoryId.Value);

                var level2Categories = _productCategoryRepository.FindAll(c => c.ParentId == categoryId);
                var level2Items = from z in query join s in level2Categories on z.CategoryId equals s.Id select z;

                
                var level3Categories = from z in categories join x in level2Categories on z.ParentId equals x.Id select z;
                var level3Items = from z in query join s in level3Categories on z.CategoryId equals s.Id select z;
                var re = level1Items.Concat(level2Items);
                query = re.Concat(level3Items);
            }
            var filterQuery = from i in query join c in categories on i.CategoryId equals c.Id where c.Status == Status.Active select i;
            query = filterQuery;
            int totalRow = query.Count();
            if (sortBy == null || sortBy.Equals("lastest"))
            {
                query = query.OrderByDescending(x => x.DateCreated)
                .Skip((page - 1) * pageSize).Take(pageSize);
            }
            else if (sortBy.Equals("name"))
            {
                query = query.OrderBy(x => x.Name).Skip((page - 1) * pageSize).Take(pageSize);
            }
            else
            {
                query = query.OrderBy(x => x.Price).Skip((page - 1) * pageSize).Take(pageSize);
            }

            var data = query.ProjectTo<ProductViewModel>().ToList();

            var paginationSet = new PagedResult<ProductViewModel>()
            {
                Results = data,
                CurrentPage = page,
                RowCount = totalRow,
                PageSize = pageSize
            };
            return paginationSet;
        }

        public PagedResult<ProductViewModel> GetAllPaging(int? categoryId, string keyword, int page, int pageSize)
        {
            var query = _productRepository.FindAll();
            if (!string.IsNullOrEmpty(keyword))
                query = query.Where(x => x.Name.Contains(keyword));
            if (categoryId.HasValue)
                query = query.Where(x => x.CategoryId == categoryId.Value);

            int totalRow = query.Count();

            query = query.OrderByDescending(x => x.DateCreated)
            .Skip((page - 1) * pageSize).Take(pageSize);

            var data = query.ProjectTo<ProductViewModel>().ToList();

            var paginationSet = new PagedResult<ProductViewModel>()
            {
                Results = data,
                CurrentPage = page,
                RowCount = totalRow,
                PageSize = pageSize
            };
            return paginationSet;
        }

        public ProductViewModel GetById(int id)
        {
            return Mapper.Map<Product, ProductViewModel>(_productRepository.FindById(id));
        }

        public void ImportExcel(string filePath, int categoryId)
        {
            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                ExcelWorksheet workSheet = package.Workbook.Worksheets[1];
                Product product;
                for (int i = workSheet.Dimension.Start.Row + 1; i <= workSheet.Dimension.End.Row; i++)
                {
                    product = new Product();
                    product.CategoryId = categoryId;
                    product.Name = workSheet.Cells[i, 1].Value.ToString();
                    product.Description = workSheet.Cells[i, 2].Value.ToString();
                    decimal.TryParse(workSheet.Cells[i, 3].Value.ToString(), out var originalPrice);
                    product.OriginalPrice = originalPrice;
                    decimal.TryParse(workSheet.Cells[i, 4].Value.ToString(), out var price);
                    product.Price = price;
                    decimal.TryParse(workSheet.Cells[i, 5].Value.ToString(), out var promotionPrice);
                    product.PromotionPrice = promotionPrice;
                    product.Content = workSheet.Cells[i, 6].Value.ToString();
                    product.SeoKeywords = workSheet.Cells[i, 7].Value.ToString();
                    product.SeoDescription = workSheet.Cells[i, 8].Value.ToString();
                    bool.TryParse(workSheet.Cells[i, 9].Value.ToString(), out var hotFlag);
                    product.HotFlag = hotFlag;
                    bool.TryParse(workSheet.Cells[i, 10].Value.ToString(), out var homeFlag);
                    product.HomeFlag = homeFlag;
                    product.Status = Status.Active;
                    _productRepository.Add(product);
                }
            }
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void Update(ProductViewModel productVm)
        {
            List<ProductTag> productTags = new List<ProductTag>();

            if (!string.IsNullOrEmpty(productVm.Tags))
            {
                string[] tags = productVm.Tags.Split(',');
                foreach (string t in tags)
                {
                    var tagId = TextHelper.ToUnsignString(t);
                    if (!_tagRepository.FindAll(x => x.Id == tagId).Any())
                    {
                        Tag tag = new Tag();
                        tag.Id = tagId;
                        tag.Name = t;
                        tag.Type = CommonConstants.ProductTag;
                        _tagRepository.Add(tag);
                    }
                    _productTagRepository.RemoveMultiple(_productTagRepository.FindAll(x => x.Id == productVm.Id).ToList());
                    ProductTag productTag = new ProductTag
                    {
                        TagId = tagId
                    };
                    productTags.Add(productTag);
                }
            }

            var product = Mapper.Map<ProductViewModel, Product>(productVm);
            foreach (var productTag in productTags)
            {
                product.ProductTags.Add(productTag);
            }
            product.DateModified = DateTime.Now;
            _productRepository.Update(product);
        }

        public List<ProductImageViewModel> GetImages(int productId)
        {
            return _productImageRepository.FindAll(x => x.ProductId == productId)
                .ProjectTo<ProductImageViewModel>().ToList();
        }

        public void AddImages(int productId, string[] images)
        {
            _productImageRepository.RemoveMultiple(_productImageRepository.FindAll(x => x.ProductId == productId).ToList());
            foreach (var image in images)
            {
                _productImageRepository.Add(new ProductImage()
                {
                    Path = image,
                    ProductId = productId,
                    Caption = string.Empty
                });
            }
        }

        public void AddWholePrice(int productId, List<WholePriceViewModel> wholePrices)
        {
            _wholePriceRepository.RemoveMultiple(_wholePriceRepository.FindAll(x => x.ProductId == productId).ToList());
            foreach (var wholePrice in wholePrices)
            {
                _wholePriceRepository.Add(new WholePrice()
                {
                    ProductId = productId,
                    FromQuantity = wholePrice.FromQuantity,
                    ToQuantity = wholePrice.ToQuantity,
                    Price = wholePrice.Price
                });
            }
        }

        public List<WholePriceViewModel> GetWholePrices(int productId)
        {
            return _wholePriceRepository.FindAll(x => x.ProductId == productId).ProjectTo<WholePriceViewModel>().ToList();
        }

        public List<ProductViewModel> GetLastest(int top)
        {
            return _productRepository.FindAll(x => x.Status == Status.Active).OrderByDescending(x => x.DateCreated)
                .Take(top).ProjectTo<ProductViewModel>().ToList();
        }

        public List<ProductViewModel> GetHotProduct(int top)
        {
            return _productRepository.FindAll(x => x.Status == Status.Active && x.HotFlag == true)
                .OrderByDescending(x => x.DateCreated)
                .Take(top)
                .ProjectTo<ProductViewModel>()
                .ToList();
        }

        public List<ProductViewModel> GetRelatedProducts(int id, int top)
        {
            var product = _productRepository.FindById(id);
            return _productRepository.FindAll(x => x.Status == Status.Active
                && x.Id != id && x.CategoryId == product.CategoryId)
            .OrderByDescending(x => x.DateCreated)
            .Take(top)
            .ProjectTo<ProductViewModel>()
            .ToList();
        }

        public List<ProductViewModel> GetUpsellProducts(int top)
        {
            return _productRepository.FindAll(x => x.PromotionPrice != null && x.Status == Status.Active)
               .OrderByDescending(x => x.DateModified)
               .Take(top)
               .ProjectTo<ProductViewModel>().ToList();
        }

        public List<TagViewModel> GetProductTags(int productId)
        {
            var tags = _tagRepository.FindAll();
            var productTags = _productTagRepository.FindAll();
            var query = from t in tags
                        join pt in productTags
                        on t.Id equals pt.TagId
                        where pt.ProductId == productId
                        select new TagViewModel()
                        {
                            Id = t.Id,
                            Name = t.Name
                        };
            return query.ToList();
        }

        public bool CheckAvailability(int productId, int size, int color)
        {
            var quantity = _productQuantityRepository.FindSingle(x => x.ColorId == color && x.SizeId == size && x.ProductId == productId);
            if (quantity == null)
                return false;
            return quantity.Quantity > 0;
        }

        public void IncreaseView(int id)
        {
            var product = _productRepository.FindById(id);
            if (product.ViewCount.HasValue)
                product.ViewCount += 1;
            else
                product.ViewCount = 1;
            _productRepository.Update(product);
        }

        public void AddColor(ColorViewModel color)
        {
            var _color = _colorRepository.FindAll(x => x.Name.Equals(color.Name)).SingleOrDefault();
            if (_color == null)
            {
                _color = Mapper.Map<ColorViewModel, Color>(color);
                _colorRepository.Add(_color);
            }
        }

        public void AddSize(SizeViewModel size)
        {
            var _size = _sizeRepository.FindAll(x => x.Name.Equals(size.Name)).SingleOrDefault();
            if (_size == null)
            {
                _size = Mapper.Map<SizeViewModel, Size>(size);
                _sizeRepository.Add(_size);
            }
        }



        public SizeViewModel GetSize(string name)
        {
            var _size = _sizeRepository.FindSingle(x => x.Name.Equals(name));
            return Mapper.Map<Size, SizeViewModel>(_size);
        }

        public ColorViewModel GetColor(string name)
        {
            var _color = _colorRepository.FindSingle(x => x.Name.Equals(name));
            return Mapper.Map<Color, ColorViewModel>(_color);
        }

        public SizeViewModel GetSizeById(int id)
        {
            var _size = _sizeRepository.FindById(id);
            return Mapper.Map<Size, SizeViewModel>(_size);
        }

        public ColorViewModel GetColorById(int id)
        {
            var _color = _colorRepository.FindById(id);
            return Mapper.Map<Color, ColorViewModel>(_color);
        }

        public ProductQuantityViewModel GetQuantity(int productId, int colorId, int sizeId)
        {
            var row =  _productQuantityRepository.FindSingle(x => x.ColorId == colorId && x.ProductId == productId && x.SizeId == sizeId);
            if( row == null)
            {
                return new ProductQuantityViewModel(0);
            }
            return Mapper.Map<ProductQuantity, ProductQuantityViewModel>(row);
        }

        public void UpdateQuantity(int id, int quantity)
        {
            var productQuantity = _productQuantityRepository.FindById(id);
            productQuantity.Quantity = quantity;
            _productQuantityRepository.Update(productQuantity);
        }
    }
}