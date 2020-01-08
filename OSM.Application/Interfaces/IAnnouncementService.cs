using OSM.Application.ViewModels.System;
using OSM.Utilities.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace OSM.Application.Interfaces
{
    public interface IAnnouncementService
    {
        PagedResult<AnnouncementViewModel> GetAllUnReadPaging(Guid userId, int pageIndex, int pageSize);

        bool MarkAsRead(Guid userId, string id);
    }
}
