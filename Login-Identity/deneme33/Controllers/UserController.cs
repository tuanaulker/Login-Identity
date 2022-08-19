using Application.Infrastructure;
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
        public async Task<ActionResponse<AddUpdateRegisterUserBindingModel>> RegisterUser([FromBody] AddUpdateRegisterUserBindingModel model)
        {
            ActionResponse<AddUpdateRegisterUserBindingModel> actionResponse = new()
            {
                ResponseType = ResponseType.Ok,
                IsSuccessful = true,
            };
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
                var userCheck = await _userManager.CreateAsync(user, model.Password);
                if (userCheck.Succeeded)
                {
                    return actionResponse;
                }     
            }
            catch (Exception ex)
            {
                actionResponse.ResponseType = ResponseType.Error;
                actionResponse.IsSuccessful = false;
                actionResponse.Errors.Add(ex.Message);
               
            }
            return actionResponse;

        }

        [HttpGet("GetAllUser")]
        public async Task<ActionResponse<List<UserDTO>>> GetAllUser()
        {
            ActionResponse<List<UserDTO>> actionResponse = new()
            {
                    ResponseType = ResponseType.Ok,
                    IsSuccessful = true,
             };
            try
            {
                var users = _userManager.Users.Select(x => new  UserDTO(x.FullName , x.Email , x.UserName , x.DateCreated)).ToList();
                actionResponse.Data=users;
                //actionResponse.IsSuccessful=true;
            }
            catch(Exception ex)
            {
                actionResponse.IsSuccessful=false;
               
            }
            return actionResponse;


        }

        [HttpPost("Login")]

        public async Task<loginBindingModel> Login([FromBody] loginBindingModel model)
        {
            ActionResponse<loginBindingModel> actionResponse = new()
            {
                ResponseType = ResponseType.Ok,
                IsSuccessful = true,
            };

            try
            {
                var user = new AppUser()
                {
                    UserName=model.FullName.Replace(" ", ""),
                //NormalizedEmail=model.Email.Normalize().ToUpperInvariant(),
                    PasswordHash = model.Password
                };
                //user.PasswordHash = "AQAAAAEAACcQAAAAEBlrJCgJ83Jj8gymuesYHHER2Z/eIIdJb/AsEh2rQdfzDsn5Jhkz+53CEtubXNOFrA==";

                if (model.FullName!="" || model.Password="")
                {
                    actionResponse.IsSuccessful = false;
                }
               
                    var result = await _signInManager.PasswordSignInAsync(user.UserName, user.PasswordHash, false, false);

                if (result.Succeeded)
                {
                    return actionResponse;  
                }

                return await Task.FromResult("Invalid Email or Password");
            }
            catch(Exception ex)
            {
                return await Task.FromResult(ex.Message);
            }

        }
    }
}

