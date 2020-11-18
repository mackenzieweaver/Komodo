using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Komodo.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Komodo.Models.ViewModels;
using Komodo.Data;
using Microsoft.EntityFrameworkCore;
using Komodo.Services;
using Komodo.Utilities;

namespace Komodo.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<BTUser> _userManager;
        private readonly IBTProjectService _projectService;
        private readonly IBTRolesService _rolesService;

        public HomeController(ApplicationDbContext context,
            ILogger<HomeController> logger,
            UserManager<BTUser> userManager,
            IBTProjectService projectService,
            IBTRolesService rolesService)
        {
            _context = context;
            _logger = logger;
            _userManager = userManager;
            _projectService = projectService;
            _rolesService = rolesService;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var vm = new HomePMViewModel();
            var tickets = await _context.Tickets
                .Include(t => t.DeveloperUser)
                .Include(t => t.OwnerUser)
                .Include(t => t.Project)
                .Include(t => t.TicketPriority)
                .Include(t => t.TicketStatus)
                .Include(t => t.TicketType)
                .Include(t => t.Comments).ThenInclude(tc => tc.User)
                .Include(t => t.Attachments)
                .Include(t => t.Notifications)
                .Include(t => t.Histories)
                .ToListAsync();
            vm.numTickets = tickets.Count;
            vm.numCritical = tickets.Where(t => t.TicketPriority.Name == "Critical").ToList().Count;
            vm.numOpen = tickets.Where(t => t.TicketStatus.Name == "Opened").ToList().Count;
            vm.numUnassigned = tickets.Where(t => t.DeveloperUserId == null).ToList().Count;
            vm.UsersOnProject = await _context.Users.ToListAsync();
            
            // give pm personalized data
            if (await _userManager.IsInRoleAsync(user, "ProjectManager"))
            {
                // all pm projects
                var projects = await _projectService.ListUserProjects(user.Id);
                // all users on all projects --> list of lists
                var users = new List<ICollection<BTUser>>();
                var ticketSet = new List<List<Ticket>>();
                foreach (var project in projects)
                {
                    users.Add(await _projectService.UsersOnProject(project.Id));                    
                    ticketSet.Add(tickets.Where(t => t.Project.Id == project.Id).ToList());
                }
                // flatten list of lists
                tickets = ticketSet.SelectMany(t => t).ToList();
                vm.UsersOnProject = users.SelectMany(u => u).Distinct().ToList();

                // remove users that are not developers
                List<BTUser> devs = new List<BTUser>();
                foreach (var flatuser in vm.UsersOnProject)
                {
                    if (await _rolesService.IsUserInRole(flatuser, "Developer"))
                    {
                        devs.Add(flatuser);
                    }
                }

                // reassign view model properties if you're a pm
                vm.numTickets = ticketSet.SelectMany(t => t).ToList().Count;
                vm.numCritical = tickets.Where(t => t.TicketPriority.Name == "Critical").ToList().Count;
                vm.numUnassigned = tickets.Where(t => t.DeveloperUserId == null).ToList().Count;
                vm.numOpen = tickets.Where(t => t.TicketStatus.Name == "Opened").ToList().Count;

                // if we have developers make suggestion
                if (devs.Count > 0)
                {
                    // maximum suggestions
                    var max = 5;
                    // minimum suggestions = number of tickets
                    var min = tickets.Where(t => t.DeveloperUserId == null).ToList().Count;
                    // if it's less than max
                    var num = min < max ? min : max;
                    for (var i = 0; i < num; i++)
                    {
                        // get ticket
                        var ticket = tickets.Where(t => t.DeveloperUserId == null)
                            .OrderBy(t => t.TicketPriorityId).ThenBy(t => t.TicketStatusId)
                            .Skip(i).Take(1).ToList()[0];
                        vm.Tickets.Add(ticket);

                        // get dev
                        devs = await _projectService.SortListOfDevsByTicketCountAsync(devs, tickets);
                        //var dev = devs.Count > i ? devs[i] : devs[0];
                        vm.Developers.Add(devs[0]);
                        // get task count
                        vm.Count.Add(tickets.Where(t => t.DeveloperUserId == devs[0].Id).ToList().Count);
                    }
                }
            }
            return View(vm);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult LandingPage()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
