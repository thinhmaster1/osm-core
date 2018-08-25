using OSM.Data.Entities;
using OSM.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace OSM.Data.IRepositories
{
    public interface IProductCategoryRepository : IRepository<ProductCategory, int>
    {
        List<ProductCategory> GetByAlias(string alias);
    }
}
