using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Komodo.Data;
using Komodo.Models.ViewModels;
using Komodo.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Komodo.Controllers
{
    [Authorize(Roles="Admin")]
    public class ProjectUsersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IBTProjectService _projectService;

        public ProjectUsersController(ApplicationDbContext context, IBTProjectService projectService)
        {
            _context = context;
            _projectService = projectService;
        }

        public async Task<IActionResult> AddProjectUsers(int projectId)
        {
            ManageProjectUsersViewModel model = new ManageProjectUsersViewModel();
            model.Project = _context.Projects.FirstOrDefault(p => p.Id == projectId);
            model.UsersOnProject = (List<Models.BTUser>)await _projectService.UsersOnProject(projectId);
            model.UsersOffProject = (List<Models.BTUser>)await _projectService.UsersNotOnProject(projectId);
            return View(model);
        }
    }
}
