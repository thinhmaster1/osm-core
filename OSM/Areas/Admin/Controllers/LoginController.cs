using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OSM.Data.Entities;
using OSM.Models.AccountViewModels;
using OSM.Utilities.Dtos;

namespace OSM.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class LoginController : Controller
    {

        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ILogger _logger;
        
        public LoginController(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            ILogger<LoginController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Authen(LoginViewModel model)
        {
           
            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User logged in.");
                    
                    return new OkObjectResult(new GenericResult(true));
                }
               
                if (result.IsLockedOut)
                {
                    _logger.LogWarning("User account locked out.");
                    return new OkObjectResult(new GenericResult(false, "User account locked out."));
                }
                else
                {
                    return new OkObjectResult(new GenericResult(false, "Username or password is wrong."));


                }
            }

            // If we got this far, something failed, redisplay form
            return new OkObjectResult(new GenericResult(false, model));

        }
    }
}