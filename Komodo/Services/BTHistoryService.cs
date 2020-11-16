using Komodo.Data;
using Komodo.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Komodo.Services
{
    public class BTHistoryService : IBTHistoryService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<BTUser> _userManager;
        private readonly IEmailSender _emailService;
        private readonly IBTNotificationService _notificationService;

        public BTHistoryService(
            ApplicationDbContext context, 
            UserManager<BTUser> userManager,
            IEmailSender emailService,
            IBTNotificationService notificationService)
        {
            _context = context;
            _userManager = userManager;
            _emailService = emailService;
            _notificationService = notificationService;
        }
        public async Task AddHistory(Ticket oldTicket, Ticket newTicket, string userId)
        {
            //Title
            if (oldTicket.Title != newTicket.Title)
            {
                TicketHistory history = new TicketHistory
                {
                    TicketId = newTicket.Id,
                    Property = "Title",
                    OldValue = oldTicket.Title,
                    NewValue = newTicket.Title,
                    Created = DateTime.Now,
                    UserId = userId
                };
                _context.TicketHistories.Add(history);
                // someone other than the developer made a change
                if(oldTicket.DeveloperUserId != userId)
                {
                    await _notificationService.Notify(userId, newTicket, history);
                }
            }
            //Description
            if (oldTicket.Description != newTicket.Description)
            {
                TicketHistory history = new TicketHistory
                {
                    TicketId = newTicket.Id,
                    Property = "Description",
                    OldValue = oldTicket.Description,
                    NewValue = newTicket.Description,
                    Created = DateTime.Now,
                    UserId = userId
                };
                _context.TicketHistories.Add(history);
            }
            // Ticket Type
            if (oldTicket.TicketTypeId != newTicket.TicketTypeId)
            {
                TicketHistory history = new TicketHistory
                {
                    TicketId = newTicket.Id,
                    Property = "Type",
                    OldValue = oldTicket.TicketType.Name,
                    NewValue = newTicket.TicketType.Name,
                    Created = DateTime.Now,
                    UserId = userId
                };
                _context.TicketHistories.Add(history);
            }
            // Ticket Priority
            if (oldTicket.TicketPriorityId != newTicket.TicketPriorityId)
            {
                TicketHistory history = new TicketHistory
                {
                    TicketId = newTicket.Id,
                    Property = "Priority",
                    OldValue = oldTicket.TicketPriority.Name,
                    NewValue = newTicket.TicketPriority.Name,
                    Created = DateTime.Now,
                    UserId = userId
                };
                _context.TicketHistories.Add(history);
            }
            // Ticket Status
            if (oldTicket.TicketStatusId != newTicket.TicketStatusId)
            {
                TicketHistory history = new TicketHistory
                {
                    TicketId = newTicket.Id,
                    Property = "Status",
                    OldValue = oldTicket.TicketStatus.Name,
                    NewValue = newTicket.TicketStatus.Name,
                    Created = DateTime.Now,
                    UserId = userId
                };
                _context.TicketHistories.Add(history);
            }
            // New Developer
            if (oldTicket.DeveloperUserId != newTicket.DeveloperUserId)
            {
                var oldval = oldTicket.DeveloperUserId == null ? "Unassigned" : oldTicket.DeveloperUser.FullName;
                TicketHistory history = new TicketHistory
                {
                    TicketId = newTicket.Id,
                    Property = "Developer",
                    OldValue = oldval,
                    NewValue = newTicket.DeveloperUser.FullName,
                    Created = DateTime.Now,
                    UserId = userId
                };
                _context.TicketHistories.Add(history);
                await _notificationService.Notify(userId, newTicket, history);
            }
            await _context.SaveChangesAsync();
        }
    }
}
