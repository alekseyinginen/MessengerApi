using AutoMapper;
using MessengerApi.BLL.Dto;
using MessengerApi.BLL.Interfaces;
using MessengerApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.NodeServices;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MessengerApi.Controllers
{
    public class GroupsController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IGroupService _groupService;
        private readonly IGroupUserService _groupUserService;

        public GroupsController(
            IMapper mapper,
            IGroupService groupService,
            IGroupUserService groupUserService)
        {
            _mapper = mapper;
            _groupService = groupService;
            _groupUserService = groupUserService;
        }

        [HttpGet]
        [Route("api/groups/")]
        [Authorize]
        public async Task<IActionResult> GetAllUserGroups()
        {
            var userGroups = await _groupService.GetUserGroups(User.Identity.Name);

            List<GroupModel> groups = new List<GroupModel>();

            foreach (var group in userGroups)
            {
                var groupUsers = await _groupUserService.GetGroupUsers(group.Id);

                GroupModel groupModel = new GroupModel
                {
                    GroupId = group.Id,
                    Usernames = groupUsers.Select(x => x.Username).ToList()
                };

                groups.Add(groupModel);
            }

            return Ok(groups);
        }

        [HttpPost]
        [Route("api/groups/create")]
        [Authorize]
        public async Task<IActionResult> CreateGroup([FromBody]GroupModel group)
        {
            var createdGroup = await _groupService.CreateGroup(new GroupDto { Username = User.Identity.Name });

            if (createdGroup == null)
            {
                return BadRequest();
            }

            await AddUsersToGroup(createdGroup.Id, group.Usernames);
            return Ok();
        }

        private async Task AddUsersToGroup(string groupId, List<string> usernames)
        {
            foreach (var username in usernames)
            {
                await _groupUserService.AddUserToGroup(new GroupUserDto { Username = username, GroupId = groupId });
            }
        }
    }
}
