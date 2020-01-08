using OSM.Application.ViewModels.Common;
using OSM.Application.ViewModels.Product;
using System;
using System.Collections.Generic;
using System.Text;

namespace OSM.Application.ViewModels.Bill
{
    public class BillDetailViewModel
    {
        public int Id { get; set; }

        public int BillId { set; get; }

        public int ProductId { set; get; }

        public int Quantity { set; get; }

        public decimal Price { set; get; }

        public int ColorId { get; set; }

        public int SizeId { get; set; }

        public BillViewModel Bill { set; get; }

        public ProductViewModel Product { set; get; }
        public ColorViewModel Color { set; get; }

        public SizeViewModel Size { set; get; }
    }
}
