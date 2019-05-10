using OSM.Application.ViewModels.Common;
using OSM.Application.ViewModels.Product;
using OSM.Utilities.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace OSM.Application.Interfaces
{
    public interface IProductService : IDisposable
    {
        List<ProductViewModel> GetAll();

        PagedResult<ProductViewModel> GetAllPaging(int? categoryId, string keyword, int page, int pageSize);
        PagedResult<ProductViewModel> GetAllPaging(int? categoryId, string keyword, int page, int pageSize, string sortBy);

        ProductViewModel Add(ProductViewModel product);

        void Update(ProductViewModel product);

        void Delete(int id);

        ProductViewModel GetById(int id);

        void ImportExcel(string filePath, int categoryId);


        void Save();

        void AddQuantity(int productId, List<ProductQuantityViewModel> quantities);

        List<ProductQuantityViewModel> GetQuantities(int productId);

        ProductQuantityViewModel GetQuantity(int productId, int colorId, int sizeId);

       

        void AddImages(int productId, string[] images);

        List<ProductImageViewModel> GetImages(int productId);

        void AddWholePrice(int productId, List<WholePriceViewModel> wholePrices);

        List<WholePriceViewModel> GetWholePrices(int productId);

        List<ProductViewModel> GetLastest(int top);

        List<ProductViewModel> GetHotProduct(int top);
        List<ProductViewModel> GetRelatedProducts(int id, int top);
        List<ProductViewModel> GetUpsellProducts(int top);
        List<TagViewModel> GetProductTags(int productId);
        bool CheckAvailability(int productId, int size, int color);
        void IncreaseView(int id);
        void UpdateQuantity(int id, int quantity);
        void AddColor(ColorViewModel color);
        void AddSize(SizeViewModel size);

        SizeViewModel GetSize(string name);

        ColorViewModel GetColor(string name);


        SizeViewModel GetSizeById(int id);

        ColorViewModel GetColorById(int id);

    }
}
