using Komodo.Data;
using Komodo.Models;
using Microsoft.AspNetCore.Identity;
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

        public BTHistoryService(ApplicationDbContext context, UserManager<BTUser> userManager)
        {
            _context = context;
            _userManager = userManager;
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
                    Property = "TicketTypeId",
                    OldValue = _context.TicketTypes.Find(oldTicket.TicketTypeId).Name,
                    NewValue = _context.TicketTypes.Find(newTicket.TicketTypeId).Name,
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
                    Property = "TicketPriorityId",
                    OldValue = _context.TicketPriorities.Find(oldTicket.TicketPriorityId).Name,
                    NewValue = _context.TicketPriorities.Find(newTicket.TicketPriorityId).Name,
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
                    Property = "TicketStatusId",
                    OldValue = _context.TicketStatuses.Find(oldTicket.TicketStatusId).Name,
                    NewValue = _context.TicketStatuses.Find(newTicket.TicketStatusId).Name,
                    Created = DateTime.Now,
                    UserId = userId
                };
                _context.TicketHistories.Add(history);
            }
            // Developer User
            if (oldTicket.DeveloperUserId != newTicket.DeveloperUserId)
            {
                var oldval = oldTicket.DeveloperUserId == null ? "Unassigned" : _context.Users.Find(oldTicket.DeveloperUserId).FullName;
                TicketHistory history = new TicketHistory
                {
                    TicketId = newTicket.Id,
                    Property = "DeveloperUserId",
                    OldValue = oldval,
                    NewValue = _context.Users.Find(newTicket.DeveloperUserId).FullName,
                    Created = DateTime.Now,
                    UserId = userId
                };
                _context.TicketHistories.Add(history);
            }
            await _context.SaveChangesAsync();
        }
    }
}
