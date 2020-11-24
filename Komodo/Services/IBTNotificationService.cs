using Komodo.Data;
using Komodo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Komodo.Services
{
    public interface IBTNotificationService
    {
        public Task Notify(string userId, Ticket ticket, string description);
        public Task SendNotificationEmail(Ticket ticket, Notification notification);
        public Task NotifyPM(Ticket ticket, string userId);
    }
}
