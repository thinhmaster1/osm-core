﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OSM.Application.Dapper.Implementation;
using OSM.Application.Dapper.Interface;
using OSM.Extensions;

namespace OSM.Areas.Admin.Controllers
{

    public class HomeController : BaseController
    {
        private readonly IReportService _reportService;
        public HomeController(IReportService reportService)
        {
            _reportService = reportService;
        }
        public IActionResult Index()
        {
            var email = User.GetSpecificClaim("Email");
            return View();
        }
        public async Task<IActionResult> GetRevenue(string fromDate, string toDate)
        {
            return new OkObjectResult(await _reportService.GetReportAsync(fromDate, toDate));
        }
    }
}