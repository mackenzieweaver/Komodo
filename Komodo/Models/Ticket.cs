using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Komodo.Models
{
    public class Ticket
    {
        public Ticket()
        {
            Comments = new HashSet<TicketComment>();
            Attachments = new HashSet<TicketAttachment>();
            Notifications = new HashSet<Notification>();
            Histories = new HashSet<TicketHistory>();
        }
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }

        [DataType(DataType.Date)]
        public DateTimeOffset Created { get; set; }
        [DataType(DataType.Date)]
        public DateTimeOffset? Updated { get; set; }

        [Display(Name = "Project")]
        public int ProjectId { get; set; }
        [Display(Name = "Type")]
        public int TicketTypeId { get; set; }
        [Display(Name = "Priority")]
        public int TicketPriorityId { get; set; }
        [Display(Name = "Status")]
        public int TicketStatusId { get; set; }
        [Display(Name = "Creator")]
        public string OwnerUserId { get; set; }
        [Display(Name = "Developer")]
        public string DeveloperUserId { get; set; }

        public Project Project { get; set; }
        public TicketType TicketType { get; set; }
        public TicketPriority TicketPriority { get; set; }
        public TicketStatus TicketStatus { get; set; }
        public BTUser OwnerUser { get; set; }
        public BTUser DeveloperUser { get; set; }

        public virtual ICollection<TicketComment> Comments { get; set; }
        public virtual ICollection<TicketAttachment> Attachments { get; set; }
        public virtual ICollection<Notification> Notifications { get; set; }
        public virtual ICollection<TicketHistory> Histories { get; set; }
    }
}
