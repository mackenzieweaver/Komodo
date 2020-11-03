using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Komodo.Data;
using Komodo.Models;
using Komodo.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Komodo.Controllers
{
    public class UserRolesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IBTRolesService _rolesService;
        private readonly UserManager<BTUser> _userManager;

        public UserRolesController(ApplicationDbContext context, IBTRolesService rolesService, UserManager<BTUser> userManager)
        {
            _context = context;
            _rolesService = rolesService;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }        
    }
}
