using AutoMapper;
using MessengerApi.BLL.Interfaces;
using MessengerApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MessengerApi.Controllers
{
    public class UserController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IUserService _userService;

        public UserController(IMapper mapper, IUserService userService)
        {
            _mapper = mapper;
            _userService = userService;
        }

        [HttpGet]
        [Route("api/users/search/{query}")]
        [Authorize]
        public IActionResult SearchForUsers([Required]string query)
        {
            if (!string.IsNullOrEmpty(query))
            {
                var users = _userService.SearchForUsers(query);
                return Ok(_mapper.Map<List<UserModel>>(users));
            }
            return BadRequest();
        }
    }
}
