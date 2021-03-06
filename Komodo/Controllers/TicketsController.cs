﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Komodo.Data;
using Komodo.Models;
using Microsoft.AspNetCore.Http;
using Komodo.Utilities;
using Komodo.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using MimeKit;
using System.IO;
using Komodo.Models.ViewModels;

namespace Komodo.Controllers
{
    [Authorize]
    public class TicketsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<BTUser> _userManager;
        private readonly IBTHistoryService _historyService;
        private readonly IBTRolesService _rolesService;
        private readonly IBTProjectService _projectService;
        private readonly IBTNotificationService _notificationService;
        private readonly IBTFileService _fileService;

        public TicketsController(ApplicationDbContext context, 
            IBTHistoryService historyService, 
            UserManager<BTUser> userManager, 
            IBTRolesService rolesService, 
            IBTProjectService projectService, 
            IBTNotificationService notificationService,
            IBTFileService fileService)
        {
            _context = context;
            _userManager = userManager;
            _historyService = historyService;
            _rolesService = rolesService;
            _projectService = projectService;
            _notificationService = notificationService;
            _fileService = fileService;
        }

        // GET: Tickets
        public async Task<IActionResult> Index()
        {
            //var id = _userManager.GetUserId(User);
            //if(User.IsInRole("Admin"){ Where(u => u.Id == id) }
            var applicationDbContext = _context.Tickets
                .Include(t => t.DeveloperUser)
                .Include(t => t.OwnerUser)
                .Include(t => t.Project)
                .Include(t => t.TicketPriority)
                .Include(t => t.TicketStatus)
                .Include(t => t.TicketType);
            return View(await applicationDbContext.ToListAsync());
        }

        public async Task<IActionResult> Scrumboard(int id, string status)
        {
            var tickets = await _context.Tickets
                .Include(t => t.Project)
                .Include(t => t.TicketPriority)
                .Include(t => t.TicketType)
                .Include(t => t.TicketStatus)
                .ToListAsync();
            var user = await _userManager.GetUserAsync(User);

            // pm - project's tickets
            if(await _rolesService.IsUserInRole(user, "ProjectManager"))
            {
                var projects = await _projectService.ListUserProjects(user.Id);
                var ticketList = new List<ICollection<Ticket>>();
                foreach(var project in projects)
                {
                    ticketList.Add(project.Tickets);
                }
                tickets = ticketList.SelectMany(t => t).ToList();
            }

            // dev - assigned tickets
            if (await _rolesService.IsUserInRole(user, "Developer"))
            {
                tickets = tickets.Where(t => t.DeveloperUserId == user.Id).ToList();
            }

            if (status != null && !User.IsInRole("Demo"))
            {
                var ticket = tickets.FirstOrDefault(t => t.Id == id);
                Ticket oldTic = await _context.Tickets
                    .Include(t => t.TicketPriority)
                    .Include(t => t.TicketStatus)
                    .Include(t => t.TicketType)
                    .Include(t => t.DeveloperUser)
                    .Include(t => t.Project)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(t => t.Id == ticket.Id);

                ticket.TicketStatusId = _context.TicketStatuses.FirstOrDefault(ts => ts.Name == status).Id;
                ticket.Updated = DateTime.Now;
                _context.Update(ticket);
                await _context.SaveChangesAsync();

                // Add history
                Ticket newTic = await _context.Tickets
                    .Include(t => t.TicketPriority)
                    .Include(t => t.TicketStatus)
                    .Include(t => t.TicketType)
                    .Include(t => t.DeveloperUser)
                    .Include(t => t.Project)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(t => t.Id == ticket.Id);
                await _historyService.AddHistory(oldTic, newTic, user.Id);
            }
            var vm = new ScrumVM 
            { 
                Opened = tickets.Where(t => t.TicketStatus.Name == TicketStatuses.Opened.ToString()).ToList(),
                Development = tickets.Where(t => t.TicketStatus.Name == TicketStatuses.Development.ToString()).ToList(),
                QualityAssurance = tickets.Where(t => t.TicketStatus.Name == TicketStatuses.QA.ToString()).ToList(),
                Closed = tickets.Where(t => t.TicketStatus.Name == TicketStatuses.Closed.ToString()).ToList()
            };
            return View(vm);
        }

        // GET: Tickets
        public async Task<IActionResult> Filter(string filter)
        {
            var user = await _userManager.GetUserAsync(User);
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
            if (await _userManager.IsInRoleAsync(user, "ProjectManager"))
            {
                var userProjects = await _projectService.ListUserProjects(user.Id);
                var ticketSet = new List<List<Ticket>>();
                foreach (var project in userProjects)
                {
                    ticketSet.Add(tickets.Where(t => t.Project.Id == project.Id).ToList());
                }
                tickets = ticketSet.SelectMany(t => t).ToList();
            }
            switch (filter)
            {
                case "Critical":
                    tickets = tickets.Where(t => t.TicketPriority.Name == "Critical").ToList();
                    break;
                case "Assigned":
                    tickets = tickets.Where(t => t.DeveloperUserId != null).ToList();
                    break;
                case "Unassigned":
                    tickets = tickets.Where(t => t.DeveloperUserId == null).ToList();
                    break;
                case "Open":
                    tickets = tickets.Where(t => t.TicketStatus.Name == "Opened").ToList();
                    break;
                default:
                    break;
            }
            return View("Index", tickets);
        }

        [Authorize(Roles = "ProjectManager,Developer")]
        public async Task<IActionResult> ProjectTickets()
        {
            var userId = _userManager.GetUserId(User);
            var projects = await _projectService.ListUserProjects(userId);
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

            var ticketSet = new List<List<Ticket>>();
            foreach (var project in projects)
            {
                ticketSet.Add(tickets.Where(t => t.Project.Id == project.Id).ToList());
            }
            tickets = ticketSet.SelectMany(t => t).ToList();
            return View("Index", tickets);
        }

        [Authorize(Roles = "Developer")]
        public async Task<IActionResult> MyTickets()
        {
            var userId = _userManager.GetUserId(User);
            var tickets = await _context.Tickets
                .Where(t => t.DeveloperUserId == userId)
                .Include(t => t.DeveloperUser)
                .Include(t => t.OwnerUser)
                .Include(t => t.Project)
                .Include(t => t.TicketPriority)
                .Include(t => t.TicketStatus)
                .Include(t => t.TicketType)
                .Include(t => t.Comments).ThenInclude(tc => tc.User)
                .Include(t => t.Attachments)
                .ToListAsync();
            return View("Index", tickets);
        }

        [Authorize(Roles = "Submitter")]
        public async Task<IActionResult> CreatedTickets()
        {
            var userId = _userManager.GetUserId(User);
            var tickets = await _context.Tickets
                .Where(t => t.OwnerUserId == userId)
                .Include(t => t.DeveloperUser)
                .Include(t => t.OwnerUser)
                .Include(t => t.Project)
                .Include(t => t.TicketPriority)
                .Include(t => t.TicketStatus)
                .Include(t => t.TicketType)
                .Include(t => t.Comments).ThenInclude(tc => tc.User)
                .Include(t => t.Attachments)
                .ToListAsync();
            return View("Index", tickets);
        }

        // GET: Tickets/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var ticket = await _context.Tickets
                .Include(t => t.DeveloperUser)
                .Include(t => t.OwnerUser)
                .Include(t => t.Project)
                .Include(t => t.TicketPriority)
                .Include(t => t.TicketStatus)
                .Include(t => t.TicketType)
                .Include(t => t.Comments).ThenInclude(tc => tc.User)
                .Include(t => t.Attachments)
                .Include(t => t.Histories).ThenInclude(h => h.User)
                .Include(t => t.Notifications)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ticket == null)
            {
                return NotFound();
            }
            var userId = _userManager.GetUserId(User);
            foreach(var notification in ticket.Notifications.Where(n => n.RecipientId == userId && n.Viewed ==  false))
            {
                var n = _context.Notifications.FirstOrDefault(n => n.Id == notification.Id);
                n.Viewed = true;
                try
                {
                    _context.Update(n);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
            }
            return View(ticket);
        }
        // or return a Task<FileResult>
        public async Task<IActionResult> ShowFile(int? id)
        {
            if(id == null)
            {
                return null;
            }
            TicketAttachment ta = await _context.TicketAttachments.FirstOrDefaultAsync(ta => ta.Id == id);
            if (ta == null)
            {
                return null;
            }
            Response.Headers.Add("Content-Disposition", $"inline; filename={ta.FileName}");
            var type = Path.GetExtension(ta.FileName).Replace(".", "");
            return File(ta.FileData, $"application/{type}");
        }

        public async Task<IActionResult> ProjectTicketDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = await _context.Tickets
                .Include(t => t.DeveloperUser)
                .Include(t => t.OwnerUser)
                .Include(t => t.Project)
                .Include(t => t.TicketPriority)
                .Include(t => t.TicketStatus)
                .Include(t => t.TicketType)
                .Include(t => t.Comments).ThenInclude(tc => tc.User)
                .Include(t => t.Attachments)
                .Include(t => t.Histories)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (ticket == null)
            {
                return NotFound();
            }

            return View("Details", ticket);
        }
        [Authorize(Roles = "Admin,ProjectManager,Developer,Submitter")]
        // GET: Tickets/Create
        public async Task<IActionResult> Create()
        {
            var user = await _userManager.GetUserAsync(User);
            if (await _rolesService.IsUserInRole(user, "Admin"))
            {
                ViewData["DeveloperUserId"] = new SelectList(_context.Users.OrderBy(u => u.FirstName).ThenBy(u => u.LastName), "Id", "FullName");
            }
            else if (await _rolesService.IsUserInRole(user, "ProjectManager"))
            {
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
                ViewData["DeveloperUserId"] = new SelectList(devs, "Id", "FullName");
            }
            ViewData["OwnerUserId"] = new SelectList(_context.Users.OrderBy(u => u.FirstName).ThenBy(u => u.LastName), "Id", "FullName");
            ViewData["ProjectId"] = new SelectList(_context.Projects, "Id", "Name");
            ViewData["TicketPriorityId"] = new SelectList(_context.TicketPriorities, "Id", "Name");
            ViewData["TicketStatusId"] = new SelectList(_context.TicketStatuses, "Id", "Name");
            ViewData["TicketTypeId"] = new SelectList(_context.TicketTypes, "Id", "Name");
            return View();
        }

        // POST: Tickets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Description,Created,Updated,ProjectId,TicketTypeId,TicketPriorityId,TicketStatusId,OwnerUserId,DeveloperUserId")] Ticket ticket, IFormFile attachment)
        {
            if (User.IsInRole("Demo"))
            {
                TempData["DemoLockout"] = "Demo users can't submit data.";
                return RedirectToAction(nameof(Index));
            }

            ticket.OwnerUserId = _userManager.GetUserId(User);
            if (ModelState.IsValid)
            {                
                _context.Add(ticket);
                await _context.SaveChangesAsync();

                if (attachment != null)
                {
                    var userId = _userManager.GetUserId(User);
                    var data = await _fileService.ConvertFileToByteArrayAsync(attachment);
                    var fileName = attachment.FileName;
                    TicketAttachment ticketAttachment = new TicketAttachment
                    {
                        FileData = data,
                        FileName = fileName,
                        FilePath = _fileService.ConvertByteArrayToFile(data, Path.GetExtension(fileName)),
                        Created = DateTime.Now,
                        UserId = userId,
                        TicketId = ticket.Id
                    };
                    _context.TicketAttachments.Add(ticketAttachment);
                    await _context.SaveChangesAsync();
                }

                await _notificationService.NotifyPM(ticket, _userManager.GetUserId(User));
                return RedirectToAction(nameof(Index));
            }
            ViewData["DeveloperUserId"] = new SelectList(_context.Users, "Id", "Id", ticket.DeveloperUserId);
            ViewData["OwnerUserId"] = new SelectList(_context.Users, "Id", "Id", ticket.OwnerUserId);
            ViewData["ProjectId"] = new SelectList(_context.Projects, "Id", "Name", ticket.ProjectId);
            ViewData["TicketPriorityId"] = new SelectList(_context.TicketPriorities, "Id", "Id", ticket.TicketPriorityId);
            ViewData["TicketStatusId"] = new SelectList(_context.TicketStatuses, "Id", "Id", ticket.TicketStatusId);
            ViewData["TicketTypeId"] = new SelectList(_context.TicketTypes, "Id", "Id", ticket.TicketTypeId);
            return View(ticket);
        }

        [Authorize(Roles = "Admin,ProjectManager,Developer,Submitter")]
        // GET: Tickets/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket == null)
            {
                return NotFound();
            }
            // is user project manager or dev?
            var user = await _userManager.GetUserAsync(User);
            if (await _rolesService.IsUserInRole(user, "ProjectManager") || await _rolesService.IsUserInRole(user, "Developer"))
            {
                //if so is user on project?
                var projectUser = (await _context.ProjectUsers.FirstOrDefaultAsync(pu => pu.UserId == user.Id && pu.ProjectId == ticket.ProjectId));
                if (projectUser == null)
                {
                    return RedirectToAction("Index");
                }
            }
            if (await _rolesService.IsUserInRole(user, "Developer") && ticket.DeveloperUserId != user.Id)
            {
                // is developer assigned to the ticket?
                return RedirectToAction("Index");
            }
            if (await _rolesService.IsUserInRole(user, "Submitter") && ticket.OwnerUserId != user.Id)
            {
                return RedirectToAction("Index");
            }

            if (await _rolesService.IsUserInRole(user, "ProjectManager"))
            {
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
                ViewData["DeveloperUserId"] = new SelectList(devs, "Id", "FullName");
            }
            else
            {
                ViewData["DeveloperUserId"] = new SelectList(_context.Users, "Id", "FullName", ticket.DeveloperUserId);
            }
            ViewData["OwnerUserId"] = new SelectList(_context.Users, "Id", "FullName", ticket.OwnerUserId);
            ViewData["ProjectId"] = new SelectList(_context.Projects, "Id", "Name", ticket.ProjectId);
            ViewData["TicketPriorityId"] = new SelectList(_context.TicketPriorities, "Id", "Name", ticket.TicketPriorityId);
            ViewData["TicketStatusId"] = new SelectList(_context.TicketStatuses.OrderBy(t => t.Id), "Id", "Name", ticket.TicketStatusId);
            ViewData["TicketTypeId"] = new SelectList(_context.TicketTypes, "Id", "Name", ticket.TicketTypeId);
            return View(ticket);
        }

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,Created,Updated,ProjectId,TicketTypeId,TicketPriorityId,TicketStatusId,OwnerUserId,DeveloperUserId")] Ticket ticket)
        {
            if (User.IsInRole("Demo"))
            {
                TempData["DemoLockout"] = "Demo users can't submit data.";
                return RedirectToAction("Details", "Tickets", new { id = ticket.Id });
            }

            if (id != ticket.Id)
            {
                return NotFound();
            }
            // snapshot of record. AsNoTracking gives the data in the DB right now.
            Ticket oldTic = await _context.Tickets
                .Include(t => t.TicketPriority)
                .Include(t => t.TicketStatus)
                .Include(t => t.TicketType)
                .Include(t => t.DeveloperUser)
                .Include(t => t.Project)
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.Id == ticket.Id);
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ticket);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TicketExists(ticket.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                // Add History
                var userId = _userManager.GetUserId(User);
                Ticket newTic = await _context.Tickets
                    .Include(t => t.TicketPriority)
                    .Include(t => t.TicketStatus)
                    .Include(t => t.TicketType)
                    .Include(t => t.DeveloperUser)
                    .Include(t => t.Project)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(t => t.Id == ticket.Id);

                await _historyService.AddHistory(oldTic, newTic, userId);
                return RedirectToAction(nameof(Index));
            }
            ViewData["DeveloperUserId"] = new SelectList(_context.Users, "Id", "Id", ticket.DeveloperUserId);
            ViewData["OwnerUserId"] = new SelectList(_context.Users, "Id", "Id", ticket.OwnerUserId);
            ViewData["ProjectId"] = new SelectList(_context.Projects, "Id", "Name", ticket.ProjectId);
            ViewData["TicketPriorityId"] = new SelectList(_context.TicketPriorities, "Id", "Id", ticket.TicketPriorityId);
            ViewData["TicketStatusId"] = new SelectList(_context.TicketStatuses, "Id", "Id", ticket.TicketStatusId);
            ViewData["TicketTypeId"] = new SelectList(_context.TicketTypes, "Id", "Id", ticket.TicketTypeId);
            return View(ticket);
        }
        
        [Authorize(Roles = "Admin")]
        // GET: Tickets/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = await _context.Tickets
                .Include(t => t.DeveloperUser)
                .Include(t => t.OwnerUser)
                .Include(t => t.Project)
                .Include(t => t.TicketPriority)
                .Include(t => t.TicketStatus)
                .Include(t => t.TicketType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        // POST: Tickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (User.IsInRole("Demo"))
            {
                TempData["DemoLockout"] = "Demo users can't submit data.";
                return RedirectToAction("Details", "Tickets", new { id = id });
            }
            var ticket = await _context.Tickets.FindAsync(id);
            _context.Tickets.Remove(ticket);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TicketExists(int id)
        {
            return _context.Tickets.Any(e => e.Id == id);
        }
    }
}
