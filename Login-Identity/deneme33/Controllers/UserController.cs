using deneme33.BindingModel;
using deneme33.Data.Entities;
using deneme33.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace deneme33.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;


        public UserController(ILogger<UserController> logger, UserManager<AppUser> userManager, SignInManager<AppUser> signManager)
        {
            _userManager = userManager;
            _signInManager = signManager;
            _logger = logger;
        }

        [HttpPost("RegisterUser")]
        public async Task<object> RegisterUser([FromBody] AddUpdateRegisterUserBindingModel model)
        {
            try {
                model.FullName = model.FullName.Replace(" ", "");

                var user = new AppUser()
                {
                    UserName = model.FullName,
                    Email=model.Email,
                    NormalizedEmail=model.Email.Normalize().ToUpperInvariant(),
                    NormalizedUserName=model.FullName.Normalize().ToUpperInvariant(),
                DateCreated=DateTime.UtcNow,
                DateModified=DateTime.UtcNow
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                return await Task.FromResult("User has been Registered");
            }
            return await Task.FromResult(string.Join(",",result.Errors.Select(x=>x.Description).ToArray()));
            }
            catch(Exception ex)
            {
                return await Task.FromResult(ex.Message);
            }
        }

        [HttpGet("GetAllUser")]
        public async Task<object> GetAllUser()
        {
            try
            {
                var users = _userManager.Users.Select(x => new  UserDTO(x.FullName , x.Email , x.UserName , x.DateCreated));
                return await Task.FromResult(users);
            }catch(Exception ex)
            {
                return await Task.FromResult(ex.Message);
            }
        }
    }
}

