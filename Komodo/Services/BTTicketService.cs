using Komodo.Data;
using Komodo.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Komodo.Services
{
    public class BTTicketService : IBTTicketService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<BTUser> _userManager;
        private readonly IBTRolesService _rolesService;
        private readonly IBTProjectService _projectService;

        public BTTicketService(ApplicationDbContext context, UserManager<BTUser> userManager, IBTRolesService rolesService, IBTProjectService projectService)
        {
            _context = context;
            _userManager = userManager;
            _rolesService = rolesService;
            _projectService = projectService;
        }

        public async Task<SelectList> GetDevs(BTUser user)
        {
            if(user == null)
            {
                return new SelectList("empty", "Id", "FullName");
            }

            if (await _rolesService.IsUserInRole(user, "Admin"))
            {
                return new SelectList(_context.Users.OrderBy(u => u.FirstName).ThenBy(u => u.LastName), "Id", "FullName");
            }

            var projects = await _projectService.ListUserProjects(user.Id);
            var users = new List<ICollection<BTUser>>();
            foreach (var project in projects)
            {
                users.Add(await _projectService.UsersOnProject(project.Id));
            }
            List<BTUser> flatUsers = users.SelectMany(u => u).Distinct().ToList();
            List<BTUser> devs = new List<BTUser>();
            foreach (var flatuser in flatUsers)
            {
                if (await _rolesService.IsUserInRole(flatuser, "Developer"))
                {
                    devs.Add(flatuser);
                }
            }
            return new SelectList(devs, "Id", "FullName");
        }

        public SelectList GetProjects()
        {
            return new SelectList(_context.Projects, "Id", "Name");
        }

        public SelectList GetPriorities()
        {
            return new SelectList(_context.TicketPriorities, "Id", "Name");
        }

        public SelectList GetStatuses()
        {
            return new SelectList(_context.TicketStatuses, "Id", "Name");
        }

        public SelectList GetTypes()
        {
            return new SelectList(_context.TicketTypes, "Id", "Name");
        }
    }
}
