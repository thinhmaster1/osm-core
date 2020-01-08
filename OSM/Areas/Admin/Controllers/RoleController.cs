using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.SignalR;
using OSM.Application.Interfaces;
using OSM.Application.ViewModels.System;
using OSM.Extensions;
using OSM.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OSM.Areas.Admin.Controllers
{
    public class RoleController : BaseController
    {
        private readonly IRoleService _roleService;
        private readonly IHubContext<OSMHub> _hubContext;

        public RoleController(IRoleService roleService, IHubContext<OSMHub> hubContext)
        {
            _roleService = roleService;
            _hubContext = hubContext;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> GetAll()
        {
            var model = await _roleService.GetAllAsync();

            return new OkObjectResult(model);
        }

        [HttpGet]
        public async Task<IActionResult> GetById(Guid id)
        {
            var model = await _roleService.GetById(id);
            return new OkObjectResult(model);
        }

        [HttpGet]
        public IActionResult GetAllPaging(string keyword, int page, int pageSize)
        {
            var model = _roleService.GetAllPagingAsync(keyword, page, pageSize);
            return new OkObjectResult(model);
        }

        [HttpPost]
        public async Task<IActionResult> SaveEntity(AppRoleViewModel roleVm)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return new BadRequestObjectResult(allErrors);
            }
            if (!roleVm.Id.HasValue)
            {
                var notificationId = Guid.NewGuid().ToString();
                var announcement = new AnnouncementViewModel()
                {
                    Title = "Role created",
                    DateCreated = DateTime.Now,
                    Content = $"Role {roleVm.Name} has been created",
                    Id = notificationId,
                    UserId = User.GetUserId()
                };
                var announcementUsers = new List<AnnouncementUserViewModel>()
                {
                    new AnnouncementUserViewModel(){AnnouncementId = notificationId,HasRead = false,UserId = User.GetUserId()}
                };
                await _roleService.AddAsync(announcement, announcementUsers, roleVm);
                await _hubContext.Clients.All.SendAsync("ReceiveMessage", announcement);
            }
            else
            {
                var notificationId = Guid.NewGuid().ToString();
                var announcement = new AnnouncementViewModel()
                {
                    Title = "Role edited",
                    DateCreated = DateTime.Now,
                    Content = $"Role {roleVm.Name} has been edited",
                    Id = notificationId,
                    UserId = User.GetUserId()
                };
                var announcementUsers = new List<AnnouncementUserViewModel>()
                {
                    new AnnouncementUserViewModel(){AnnouncementId = notificationId,HasRead = false,UserId = User.GetUserId()}
                };
                await _roleService.UpdateAsync(roleVm);
                await _hubContext.Clients.All.SendAsync("ReceiveMessage", announcement);
            }
            return new OkObjectResult(roleVm);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (!ModelState.IsValid)
            {
                return new BadRequestObjectResult(ModelState);
            }
            var roleName = _roleService.GetById(id);
            var notificationId = Guid.NewGuid().ToString();
            var announcement = new AnnouncementViewModel()
            {
                Title = "Role deleted",
                DateCreated = DateTime.Now,
                Content = $"Role {roleName} has been deleted.",
                Id = notificationId,
                UserId = User.GetUserId()
            };
            var announcementUsers = new List<AnnouncementUserViewModel>()
                {
                    new AnnouncementUserViewModel(){AnnouncementId = notificationId,HasRead = false,UserId = User.GetUserId()}
                };
            await _hubContext.Clients.All.SendAsync("ReceiveMessage", announcement);
            await _roleService.DeleteAsync(id);

            return new OkObjectResult(id);
        }

        [HttpPost]
        public IActionResult ListAllFunction(Guid roleId)
        {
            var functions = _roleService.GetListFunctionWithRole(roleId);
            return new OkObjectResult(functions);
        }

        [HttpPost]
        public IActionResult SavePermission(List<PermissionViewModel> listPermmission, Guid roleId)
        {
            _roleService.SavePermission(listPermmission, roleId);
            return new OkResult();
        }
    }
}