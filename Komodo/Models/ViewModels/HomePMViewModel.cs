using System;
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
        public List<Ticket> Tickets { get; set; }
        public List<BTUser> Developers { get; set; }
        public List<int> Count { get; set; }
    }
}
