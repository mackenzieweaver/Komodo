using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Komodo.Models.ViewModels
{
    public class ProjectUsersViewModel
    {
        public Project Project { get; set; }
        // populates list box
        public MultiSelectList Users { get; set; }
        // receives selected users
        public string[] SelectedUsers { get; set; } 
    }
}
