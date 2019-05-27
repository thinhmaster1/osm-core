using OSM.Application.ViewModels.Product;
using OSM.Utilities.Dtos;
using System.Collections.Generic;

namespace OSM.Application.Interfaces
{
    public interface IProductCategoryService
    {
        ProductCategoryViewModel Add(ProductCategoryViewModel productCategoryVm);

        PagedResult<ProductCategoryViewModel> GetAllPaging(string keyword, int page, int pageSize);

        void Update(ProductCategoryViewModel productCategoryVm);

        void Delete(int id);

        List<ProductCategoryViewModel> GetAll();
        List<ProductCategoryViewModel> GetAllActive();

        List<ProductCategoryViewModel> GetAll(string keyword);

        List<ProductCategoryViewModel> GetAllByParentId(int parentId);

        ProductCategoryViewModel GetById(int id);

        void UpdateParentId(int sourceId, int targetId, Dictionary<int, int> items);

        void ReOrder(int sourceId, int targetId);

        List<ProductCategoryViewModel> GetHomeCategories(int top);

        void Save();
    }
}