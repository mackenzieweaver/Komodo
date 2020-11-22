using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Komodo.Models
{
    public class Project
    {
        public Project()
        {
            Tickets = new HashSet<Ticket>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Project Name")]
        public string Name { get; set; }

        [Display(Name = "Image Path")]
        public string ImagePath { get; set; }

        [Display(Name = "Image Data")]
        public byte[] ImageData { get; set; }

        public List<ProjectUser> ProjectUsers { get; set; }

        public virtual ICollection<Ticket> Tickets { get; set; }
    }
}
