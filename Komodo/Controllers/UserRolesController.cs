﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Komodo.Data;
using Komodo.Models;
using Komodo.Models.ViewModels;
using Komodo.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Komodo.Controllers
{
    [Authorize]
    public class UserRolesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IBTRolesService _rolesService;
        private readonly UserManager<BTUser> _userManager;

        public UserRolesController(ApplicationDbContext context, IBTRolesService rolesService, UserManager<BTUser> userManager)
        {
            _context = context;
            _rolesService = rolesService;
            _userManager = userManager;
        }

        [Authorize(Roles = "Admin,ProjectManager")]
        public async Task<IActionResult> ManageUserRoles()
        {
            List<ManageUserRolesViewModel> model = new List<ManageUserRolesViewModel>();
            var userId = _userManager.GetUserId(User);
            List<BTUser> users = _context.Users.Where(u => u.Id != userId).ToList();
            foreach(var user in users)
            {
                ManageUserRolesViewModel vm = new ManageUserRolesViewModel();
                vm.User = user;


                var myRoles = await _rolesService.ListUserRoles(user);
                var myRole = myRoles.FirstOrDefault(role => role != "Demo");
                vm.SelectedRole = myRole;
                //var roleList = _context.Roles.ToList();
                var roleList = _context.Roles.Where(r => r.Name != "Demo" && r.Name != "NewUser").ToList();
                ViewData["Roles"] = new SelectList(roleList, "Name", "Name");

                model.Add(vm);
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ManageUserRoles(ManageUserRolesViewModel btuser)
        {
            if (User.IsInRole("Demo"))
            {
                TempData["DemoLockout"] = "Demo users can't submit data.";
                return RedirectToAction("ManageUserRoles");
            }
            BTUser user = _context.Users.Find(btuser.User.Id);
            IEnumerable<string> roles = await _rolesService.ListUserRoles(user);
            await _userManager.RemoveFromRolesAsync(user, roles);
            string userRole = btuser.SelectedRole;

            if(Enum.TryParse(userRole, out Roles roleValue))
            {
                await _rolesService.AddUserToRole(user, userRole);
                return RedirectToAction("ManageUserRoles");
            }
            return RedirectToAction("ManageUserRoles");
        }
    }
}
