using Komodo.Data;
using Komodo.Models;
using Komodo.Models.ChartModels;
using Komodo.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Komodo.Controllers
{
    public class ChartsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<BTUser> _userManager;
        private readonly IBTProjectService _projectService;

        public ChartsController(ApplicationDbContext context, UserManager<BTUser> userManager, IBTProjectService projectService)
        {
            _context = context;
            _userManager = userManager;
            _projectService = projectService;
        }

        public async Task<JsonResult> PieData()
        {
            var users = await _context.Users.ToListAsync();
            int numAdmins = 0;
            int numPms = 0;
            int numDevs = 0;
            int numSubs = 0;
            int numNew = 0;
            if (User.IsInRole("ProjectManager"))
            {
                var userId = _userManager.GetUserId(User);
                var projects = await _projectService.ListUserProjects(userId);
                var usersOnMyProjects = new List<List<BTUser>>();
                foreach(var project in projects){
                    usersOnMyProjects.Add((List<BTUser>)await _projectService.UsersOnProject(project.Id));
                }
                users = usersOnMyProjects.SelectMany(u => u).Distinct().ToList();
            }
            foreach (var user in users)
            {
                foreach (var role in await _userManager.GetRolesAsync(user))
                {
                    switch (role)
                    {
                        case "Admin":
                            numAdmins += 1;
                            break;
                        case "ProjectManager":
                            numPms += 1;
                            break;
                        case "Developer":
                            numDevs += 1;
                            break;
                        case "Submitter":
                            numSubs += 1;
                            break;
                        case "NewUser":
                            numNew += 1;
                            break;
                        default:
                            break;
                    }
                }
            }
            var list = new List<int> { numAdmins, numPms, numDevs, numSubs, numNew };
            return Json(list);
        }

        public async Task<JsonResult> BarData()
        {            
            var devs = new List<BTUser>();
            var users = await _context.Users.ToListAsync();
            foreach (var user in users)
            {
                foreach (var role in await _userManager.GetRolesAsync(user))
                {
                    if(role == "Developer")
                    {
                        devs.Add(user);
                    }
                }
            }
            var list = new List<BarChartModel>();
            var tickets = _context.Tickets.Include(t => t.TicketStatus);
            foreach (var dev in devs)
            {
                var bar = new BarChartModel
                {
                    Name = dev.FirstName,
                    NumAssigned = tickets
                        .Where(t => t.DeveloperUserId == dev.Id && t.TicketStatus.Name != "Closed")
                        .ToList().Count,
                    NumClosed = tickets.Where(t => t.DeveloperUserId == dev.Id && t.TicketStatus.Name == "Closed")
                        .ToList().Count
                };
                list.Add(bar);
            }
            return Json(list);
        }
    }
}
