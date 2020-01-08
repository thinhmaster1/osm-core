using Microsoft.AspNetCore.Mvc;
using OSM.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OSM.Controllers.Components
{
    public class BrandViewComponent : ViewComponent
    {
        private readonly ICommonService _commonService;
        public BrandViewComponent(ICommonService commonService)
        {
            _commonService = commonService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View(_commonService.GetSlides("brand"));
        }
    }
}
