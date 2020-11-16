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
        public Task Notify(string userId, Ticket ticket, string subject);
    }
}
