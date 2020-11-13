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
        public MultiSelectList UsersOnProject { get; set; }
        public MultiSelectList UsersOffProject { get; set; }
        // receives selected users
        public string[] SelectedUsers { get; set; } 
    }
}
