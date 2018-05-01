using AutoMapper;
using MessengerApi.DAL.Entities;
using MessengerApi.Models;
using MessengerApi.BLL.Dto;
using MessengerApi.BLL.Infrastucture;
using MessengerApi.BLL.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MessengerApi.Controllers {
    public class AuthController : Controller 
    {
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly SignInManager<ApplicationUser> _authenticationManager;

        public AuthController(IMapper mapper, IUserService userService, SignInManager<ApplicationUser> authManager)
        {
            _mapper = mapper;
            _userService = userService;
            _authenticationManager = authManager;
        }

        [HttpGet]
        [Route("api/auth/currentuser")]
        public async Task<IActionResult> GetCurrentUser()
        {
            if (User.Identity.IsAuthenticated)
            {
                ApplicationUser item = await _authenticationManager.UserManager.FindByNameAsync(User.Identity.Name);
                UserModel currentUser = _mapper.Map<UserDto, UserModel>(await _userService.GetUserById(item.Id));
                return Ok(currentUser);
            }
            return NotFound("User is not logged in");
        }

        [HttpPost]
        [Route("api/auth/login")]
        public async Task<IActionResult> Login([FromBody]LoginModel item)
        {
            if (ModelState.IsValid) 
            {
                var result = await _authenticationManager.PasswordSignInAsync(item.Username, item.Password, true, false);
                if (result.Succeeded) 
                {
                    return Ok();
                }
            }
            return BadRequest(ModelState);
        }

        [HttpGet]
        [Route("api/auth/logout")]
        public async Task<IActionResult> Logout() 
        {
            await _authenticationManager.SignOutAsync();
            return Ok();
        }

        [HttpPost]
        [Route("api/auth/register")]
        public async Task<IActionResult> Register([FromBody]RegisterModel model) 
        {
            if (ModelState.IsValid)
            {
                UserDto userDto = _mapper.Map<RegisterModel, UserDto>(model);
                userDto.Roles = new List<string>(new string[]{ "User" });
                OperationDetails operationDetails = await _userService.Create(userDto);
                if (operationDetails.Succedeed)
                {
                    return Ok(operationDetails);
                }
            }
            return BadRequest(ModelState);
        }
    }
}
