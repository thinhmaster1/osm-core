using OSM.Application.ViewModels.Bill;
using OSM.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OSM.Models
{
    public class OrderViewModel
    {
        public BillStatus OrderStatus { get; set; }
        public List<BillViewModel> OrderBills { get; set; }
    }
}
