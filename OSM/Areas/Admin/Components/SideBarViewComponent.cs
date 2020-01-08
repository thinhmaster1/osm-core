using Microsoft.AspNetCore.Mvc;
using OSM.Application.Interfaces;
using OSM.Application.ViewModels.System;
using OSM.Extensions;
using OSM.Utilities.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OSM.Areas.Admin.Components
{
    public class SideBarViewComponent : ViewComponent
    {
        private IFunctionService _functionService;
        private IRoleService _roleService;

        public SideBarViewComponent(IFunctionService functionService, IRoleService roleService)
        {
            _functionService = functionService;
            _roleService = roleService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var stringRoles = ((ClaimsPrincipal)User).GetSpecificClaim("Roles");
            List<FunctionViewModel> functions;
            if (stringRoles.Split(";").Contains(CommonConstants.AppRole.AdminRole))
            {
                functions = await _functionService.GetAll(string.Empty);
            }
            else
            {
                //TODO: Get by permission
                var roles = stringRoles.Split(";");

                var roleId = _roleService.GetByName(roles[0]).Result;

                var permissions = _roleService.GetListFunctionWithRole(roleId).Result;

                functions = new List<FunctionViewModel>();

                foreach (var permission in permissions)
                {
                    if(permission.CanRead == true)
                    {
                        functions.Add(_functionService.GetById(permission.FunctionId));
                    }

                }
            }
            return View(functions);
        }

        public static Guid ToGuid(int value)
        {
            byte[] bytes = new byte[16];
            BitConverter.GetBytes(value).CopyTo(bytes, 0);
            return new Guid(bytes);
        }
    }
}