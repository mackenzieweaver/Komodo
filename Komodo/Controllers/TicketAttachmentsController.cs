using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Komodo.Data;
using Komodo.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.IO;
using Komodo.Services;
using Microsoft.AspNetCore.Identity;

namespace Komodo.Controllers
{
    [Authorize]
    public class TicketAttachmentsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<BTUser> _userManager;
        private readonly IBTNotificationService _notificationService;

        public TicketAttachmentsController(ApplicationDbContext context, UserManager<BTUser> userManager, IBTNotificationService notificationService)
        {
            _context = context;
            _userManager = userManager;
            _notificationService = notificationService;
        }
        [Authorize(Roles = "Admin,ProjectManager")]
        // GET: TicketAttachments
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.TicketAttachments.Include(t => t.Ticket).Include(t => t.User);
            return View(await applicationDbContext.ToListAsync());
        }
        [Authorize(Roles = "Admin,ProjectManager")]
        // GET: TicketAttachments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticketAttachment = await _context.TicketAttachments
                .Include(t => t.Ticket)
                .Include(t => t.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ticketAttachment == null)
            {
                return NotFound();
            }

            return View(ticketAttachment);
        }
        [Authorize(Roles = "Admin,ProjectManager")]
        // GET: TicketAttachments/Create
        public IActionResult Create()
        {
            ViewData["TicketId"] = new SelectList(_context.Tickets, "Id", "Description");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: TicketAttachments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FormFile,Image,Description,Created,TicketId,UserId")] TicketAttachment ticketAttachment)
        {
            if (ModelState.IsValid)
            {
                if (ticketAttachment != null)
                {
                    MemoryStream ms = new MemoryStream();
                    await ticketAttachment.FormFile.CopyToAsync(ms);

                    ticketAttachment.FileData = ms.ToArray();
                    ticketAttachment.FileName = ticketAttachment.FormFile.FileName;

                    var binary = Convert.ToBase64String(ticketAttachment.FileData);
                    var ext = Path.GetExtension(ticketAttachment.FileName);
                    ticketAttachment.FilePath = $"data:image/{ext};base64,{binary}";

                    ticketAttachment.Created = DateTimeOffset.Now;
                    ticketAttachment.UserId = _userManager.GetUserId(User);
                    _context.Add(ticketAttachment);
                    await _context.SaveChangesAsync();
                    Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\images", ticketAttachment.FileData.ToString());

                    var ticket = await _context.Tickets.Include(t => t.DeveloperUser).Include(t => t.Project).Include(t => t.TicketPriority).FirstOrDefaultAsync(t => t.Id == ticketAttachment.TicketId);
                    if (ticket.DeveloperUserId != null)
                    {
                        var userId = _userManager.GetUserId(User);
                        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
                        var description = $"{user.FullName} added an attachment on Ticket titled: '{ticket.Title}': '{ticketAttachment.Description}'";
                        await _notificationService.Notify(userId, ticket, description);
                    }
                    return RedirectToAction("Details", "Tickets", new { id = ticketAttachment.TicketId });

                }
            }
            ViewData["TicketId"] = new SelectList(_context.Tickets, "Id", "Description", ticketAttachment.TicketId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", ticketAttachment.UserId);
            return RedirectToAction("Details", "Tickets", new { id = ticketAttachment.TicketId });
        }


        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("Id,FilePath,FileData,Description,Created,TicketId,UserId")] TicketAttachment ticketAttachment, IFormFile attachment)
        //{
        //    if (User.IsInRole("Demo"))
        //    {
        //        TempData["DemoLockout"] = "Demo users can't submit data.";
        //        return RedirectToAction(nameof(Index));
        //    }
        //    if (ModelState.IsValid)
        //    {
        //        if (attachment != null)
        //        {
        //            var memoryStream = new MemoryStream();
        //            attachment.CopyTo(memoryStream);
        //            byte[] bytes = memoryStream.ToArray();
        //            memoryStream.Close();
        //            memoryStream.Dispose();
        //            var binary = Convert.ToBase64String(bytes);
        //            var ext = Path.GetExtension(attachment.FileName);

        //            ticketAttachment.FilePath = $"data:image/{ext};base64,{binary}";
        //            ticketAttachment.FileData = bytes;
        //            ticketAttachment.Description = attachment.FileName;
        //            ticketAttachment.Created = DateTime.Now;

        //            _context.Add(ticketAttachment);
        //            await _context.SaveChangesAsync();

        //            var ticket = await _context.Tickets
        //                .Include(t => t.TicketPriority)
        //                .Include(t => t.TicketStatus)
        //                .Include(t => t.TicketType)
        //                .Include(t => t.DeveloperUser)
        //                .Include(t => t.Project)
        //                .FirstOrDefaultAsync(t => t.Id == ticketAttachment.TicketId);
        //            if(ticket.DeveloperUserId != null)
        //            {
        //                await _notificationService.NotifyOfAttachment(_userManager.GetUserId(User), ticket, ticketAttachment);
        //            }
        //            return RedirectToAction(nameof(Index));
        //        }
        //        return RedirectToAction("Details", "Tickets", new { id = ticketAttachment.TicketId });
        //    }
        //    ViewData["TicketId"] = new SelectList(_context.Tickets, "Id", "Description", ticketAttachment.TicketId);
        //    ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", ticketAttachment.UserId);
        //    return View(ticketAttachment);
        //}

        [Authorize(Roles = "Admin,ProjectManager")]
        // GET: TicketAttachments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticketAttachment = await _context.TicketAttachments.FindAsync(id);
            if (ticketAttachment == null)
            {
                return NotFound();
            }
            ViewData["TicketId"] = new SelectList(_context.Tickets, "Id", "Description", ticketAttachment.TicketId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", ticketAttachment.UserId);
            return View(ticketAttachment);
        }

        // POST: TicketAttachments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FilePath,FileData,Description,Created,TicketId,UserId")] TicketAttachment ticketAttachment)
        {
            if (id != ticketAttachment.Id)
            {
                return NotFound();
            }
            if (User.IsInRole("Demo"))
            {
                TempData["DemoLockout"] = "Demo users can't submit data.";
                return RedirectToAction("Details", "Tickets", new { id = ticketAttachment.Id });
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ticketAttachment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TicketAttachmentExists(ticketAttachment.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["TicketId"] = new SelectList(_context.Tickets, "Id", "Description", ticketAttachment.TicketId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", ticketAttachment.UserId);
            return View(ticketAttachment);
        }
        [Authorize(Roles = "Admin,ProjectManager")]
        // GET: TicketAttachments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticketAttachment = await _context.TicketAttachments
                .Include(t => t.Ticket)
                .Include(t => t.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ticketAttachment == null)
            {
                return NotFound();
            }

            return View(ticketAttachment);
        }

        // POST: TicketAttachments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (User.IsInRole("Demo"))
            {
                TempData["DemoLockout"] = "Demo users can't submit data.";
                return RedirectToAction(nameof(Index));
            }
            var ticketAttachment = await _context.TicketAttachments.FindAsync(id);
            _context.TicketAttachments.Remove(ticketAttachment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TicketAttachmentExists(int id)
        {
            return _context.TicketAttachments.Any(e => e.Id == id);
        }
    }
}
