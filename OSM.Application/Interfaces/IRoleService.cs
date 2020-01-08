using OSM.Application.ViewModels.System;
using OSM.Utilities.Dtos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OSM.Application.Interfaces
{
    public interface IRoleService
    {

        Task DeleteAsync(Guid id);
        Task<List<AppRoleViewModel>> GetAllAsync();
        PagedResult<AppRoleViewModel> GetAllPagingAsync(string keyword, int page, int pageSize);
        Task<AppRoleViewModel> GetById(Guid id);
        Task<Guid> GetByName(string name);
        Task UpdateAsync(AppRoleViewModel userVm);
        Task<List<PermissionViewModel>> GetListFunctionWithRole(Guid roleId);
        void SavePermission(List<PermissionViewModel> permissions, Guid roleId);
        Task<bool> CheckPermission(string functionId, string action, string[] roles);
        Task<bool> AddAsync(AnnouncementViewModel announcement, List<AnnouncementUserViewModel> announcementUsers, AppRoleViewModel userVm);
    }
}
