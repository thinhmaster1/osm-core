using OSM.Application.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace OSM.Application.ViewModels.Product
{
    public class ProductQuantityViewModel
    {
        public ProductQuantityViewModel()
        {
        }
        public ProductQuantityViewModel(int quantity)
        {
            Quantity = quantity;
        }
        public int Id { get; set; }
        public int ProductId { get; set; }

        public int SizeId { get; set; }


        public int ColorId { get; set; }

        public int Quantity { get; set; }

        public ColorViewModel Color { get; set; }
        public SizeViewModel Size { get; set; }
    }
}
