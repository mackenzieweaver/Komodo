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
            vm.numProjects = _context.Projects.ToList().Count;
            vm.numTickets = _context.Tickets.ToList().Count;
            vm.numClosed = _context.Tickets
                .Include(t => t.TicketStatus)
                .Where(t => t.TicketStatus.Name == "Closed")
                .ToList().Count;
            vm.numOpened = _context.Tickets
                .Include(t => t.TicketStatus)
                .Where(t => t.TicketStatus.Name == "Opened")
                .ToList().Count;

            // suggest action to pm
            if (await _userManager.IsInRoleAsync(user, "ProjectManager"))
            {
                // all pm projects
                var projects = await _projectService.ListUserProjects(user.Id);
                // all users on all projects, potential list of lists
                var users = new List<ICollection<BTUser>>();
                foreach (var project in projects)
                {
                    users.Add(await _projectService.UsersOnProject(project.Id));
                }
                // flatten multi list into single list of unique users
                List<BTUser> flatUsers = users
                    .SelectMany(u => u)
                    .Distinct()
                    .ToList();
                // remove users that are not developers
                List<BTUser> devs = new List<BTUser>();
                foreach (var flatuser in flatUsers)
                {
                    if (await _rolesService.IsUserInRole(flatuser, "Developer"))
                    {
                        devs.Add(flatuser);
                    }
                }
                if (devs.Count > 0)
                {
                    // maximum suggestions
                    var num = 5;
                    // minimum suggestions = number of tickets
                    var tCount = _context.Tickets.Where(t => t.DeveloperUserId == null).ToList().Count;
                    // if it's less than max
                    num = tCount < num ? tCount : num;
                    for (var i = 0; i < num; i++)
                    {
                        // get ticket
                        var ticket = _context.Tickets
                            .Where(t => t.DeveloperUserId == null)
                            .Include(t => t.TicketPriority)
                            .Include(t => t.TicketStatus)
                            .Include(t => t.TicketType)
                            .OrderBy(t => t.TicketPriorityId)
                            .ThenBy(t => t.TicketStatusId)
                            .Skip(i)
                            .Take(1)
                            .ToList()[0];
                        vm.Tickets.Add(ticket);

                        // get dev
                        devs = BubbleSort.SortListOfDevsByTicketCount(devs, _context);
                        vm.Developers.Add(devs[0]);

                        // get task count
                        var tickets = _context.Tickets.Where(t => t.DeveloperUserId == devs[0].Id).ToList();
                        vm.Count.Add(tickets.Count);
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
