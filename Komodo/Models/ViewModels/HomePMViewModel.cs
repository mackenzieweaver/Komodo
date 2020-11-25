﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Komodo.Models.ViewModels
{
    public class HomePMViewModel
    {
        public HomePMViewModel()
        {
            Tickets = new List<Ticket>();
            Developers = new List<BTUser>();
            Count = new List<int>();
        }
        // data for everyone
        public int numTickets { get; set; }
        public int numCritical { get; set; }
        public int numAssigned { get; set; }
        public int numUnassigned { get; set; }
        public int numOpen { get; set; }
        public List<Ticket> Tickets { get; set; }

        // pm data
        public List<BTUser> Developers { get; set; }
        public List<BTUser> UsersOnProject { get; set; }
        public List<int> Count { get; set; }

        // Developer data
        public List<Ticket> TicketsAssignedToDev { get; set; }
        public List<Ticket> TicketsOnDevProjs { get; set; }

        // Submitter data
        public List<Ticket> TicketsCreatedByMe { get; set; }

        // notifications
        public List<Notification> Notifications { get; set; }
    }
}
