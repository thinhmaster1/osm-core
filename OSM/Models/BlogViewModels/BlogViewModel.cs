using OSM.Utilities.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OSM.Models.BlogViewModels
{
    public class BlogViewModel
    {
        public PagedResult<OSM.Application.ViewModels.Blog.BlogViewModel> Data { get; set; }
    }
}
