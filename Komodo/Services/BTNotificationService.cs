using Komodo.Data;
using Komodo.Models;
using Microsoft.AspNetCore.Identity.UI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Komodo.Services
{
    public class BTNotificationService : IBTNotificationService
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmailSender _emailService;

        public BTNotificationService(ApplicationDbContext context, IEmailSender emailService)
        {
            _context = context;
            _emailService = emailService;
        }
        public async Task Notify(string userId, Ticket ticket, TicketHistory change)
        {
            var notification = new Notification
            {
                TicketId = ticket.Id,
                Description = $"The {change.Property} was updated from {change.OldValue} to {change.NewValue}.",
                Created = DateTime.Now,
                SenderId = userId,
                RecipientId = ticket.DeveloperUserId
            };
            await _context.Notifications.AddAsync(notification);
            await _context.SaveChangesAsync();
            var to = ticket.DeveloperUser.Email;
            var subject = $"For project: { ticket.Project.Name }, ticket: { ticket.Title }, priority: { ticket.TicketPriority.Name }";
            await _emailService.SendEmailAsync(to, subject, notification.Description);
        }
    }
}
