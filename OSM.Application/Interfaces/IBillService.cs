using OSM.Application.ViewModels.Bill;
using OSM.Application.ViewModels.Common;
using OSM.Data.Enums;
using OSM.Utilities.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace OSM.Application.Interfaces
{
    public interface IBillService
    {
        void Create(BillViewModel billVm);
        void Update(BillViewModel billVm);

        PagedResult<BillViewModel> GetAllPaging(string startDate, string endDate, string keyword,
            int pageIndex, int pageSize);
        List<BillViewModel> GetAllByCustomerId(Guid id);
        BillViewModel GetDetail(int billId);

        BillDetailViewModel CreateDetail(BillDetailViewModel billDetailVm);

        void DeleteDetail(int productId, int billId, int colorId, int sizeId);

        void UpdateStatus(int orderId, BillStatus status);

        List<BillDetailViewModel> GetBillDetails(int billId);

        List<ColorViewModel> GetColors();

        List<SizeViewModel> GetSizes();
        ColorViewModel GetColor(int id);
        SizeViewModel GetSize(int id);

        BillViewModel GetBill(int id);
        void Save();
    }
}
