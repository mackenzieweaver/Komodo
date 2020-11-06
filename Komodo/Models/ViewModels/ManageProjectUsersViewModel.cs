using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Komodo.Models.ViewModels
{
    public class ManageProjectUsersViewModel
    {
        public Project Project { get; set; }

        public MultiSelectList MultiSelectUsersOnProject { get; set; }
        public MultiSelectList MultiSelectUsersOffProject { get; set; }
        public string[] SelectedUsersOnProject { get; set; }
        public string[] SelectedUsersOffProject { get; set; }

        public List<BTUser> UsersOnProject { get; set; }
        public List<BTUser> UsersOffProject { get; set; }
    }
}
