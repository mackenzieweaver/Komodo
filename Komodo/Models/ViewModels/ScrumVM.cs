using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Komodo.Models.ViewModels
{
    public class ScrumVM
    {
        public ICollection<Ticket> Opened { get; set; } = new HashSet<Ticket>();
        public ICollection<Ticket> Development { get; set; } = new HashSet<Ticket>();
        public ICollection<Ticket> QualityAssurance { get; set; } = new HashSet<Ticket>();
        public ICollection<Ticket> Closed { get; set; } = new HashSet<Ticket>();
    }
}
