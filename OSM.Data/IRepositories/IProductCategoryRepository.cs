using OSM.Data.Entities;
using OSM.Infrastructure.Interfaces;
using System.Collections.Generic;

namespace OSM.Data.IRepositories
{
    public interface IProductCategoryRepository : IRepository<ProductCategory, int>
    {
        List<ProductCategory> GetByAlias(string alias);
    }
}