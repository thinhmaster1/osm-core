using OSM.Data.Entities;
using OSM.Data.IRepositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace OSM.Data.EF.Repositories
{
    public class FunctionRepository : EFRepository<Function, string>, IFunctionRepository
    {
        public FunctionRepository(AppDbContext context) : base(context)
        {
        }
    }
}
