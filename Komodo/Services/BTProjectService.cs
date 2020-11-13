using Komodo.Data;
using Komodo.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Komodo.Services
{
    public class BTProjectService : IBTProjectService
    {
        private readonly ApplicationDbContext _context;

        public BTProjectService(ApplicationDbContext context)
        {
            _context = context;
        }

        //public async Task AddUserToProject(string userId, int projectId)
        //{
        //    var user = new ProjectUser
        //    {
        //        UserId = userId,
        //        ProjectId = projectId
        //    };
        //    await _context.ProjectUsers.AddAsync(user);
        //    await _context.SaveChangesAsync();
        //}

        public async Task AddUserToProject(string userId, int projectId)
        {
            if(!await IsUserOnProject(userId, projectId))
            {
                try
                {
                    await _context.ProjectUsers.AddAsync(new ProjectUser { ProjectId = projectId, UserId = userId });
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"*** ERROR *** - Error Adding user to project. --> {ex.Message}");
                    throw;
                }
            }
        }

        //public async Task<bool> IsUserOnProject(string userId, int projectId)
        //{
        //    var user = await _context.ProjectUsers.FirstOrDefaultAsync(pu => pu.UserId == userId && pu.ProjectId == projectId);
        //    if (user == null)
        //    {
        //        return false;
        //    }
        //    else
        //    {
        //        return true;
        //    }
        //}

        public async Task<bool> IsUserOnProject(string userId, int projectId)
        {
            Project project = await _context.Projects
                //.Include(u => u.ProjectUsers.Where(u => u.UserId == userId)).ThenInclude(u => u.User)
                .Include(u => u.ProjectUsers).ThenInclude(u => u.User)
                .FirstOrDefaultAsync(u => u.Id == projectId);
            bool result = project.ProjectUsers.Any(u => u.UserId == userId);
            return result;
        }

        //public async Task<bool> IsUserOnProject(string userId, int projectId)
        //{
        //    return _context.ProjectUsers.Where(pu => pu.UserId == userId && pu.ProjectId == projectId).Any();
        //}

        public async Task<ICollection<Project>> ListUserProjects(string userId)
        {
            BTUser user = await _context.Users
                .Include(p => p.ProjectUsers)
                .ThenInclude(p => p.Project)
                .FirstOrDefaultAsync(p => p.Id == userId);

            //List<Project> projects = user.ProjectUsers.SelectMany(p => (IEnumerable<Project>)p.Project).ToList();
            List<Project> projects = user.ProjectUsers.Select(p => p.Project).ToList();
            return projects;
        }

        //public async Task<ICollection<Project>> ListUserProjects(string userId)
        //{
        //    var ids = await _context.ProjectUsers.Where(pu => pu.UserId == userId).Select(p => p.ProjectId).ToListAsync();
        //    ICollection<Project> Projects = new HashSet<Project>();
        //    foreach(var id in ids)
        //    {
        //        var project = await _context.Projects.FirstOrDefaultAsync(p => p.Id == id);
        //        Projects.Add(project);
        //    }
        //    return Projects;
        //}

        public async Task RemoveUserFromProject(string userId, int projectId)
        {
            if (await IsUserOnProject(userId, projectId))
            {
                try
                {
                    //ProjectUser user = new ProjectUser
                    //{
                    //    ProjectId = projectId,
                    //    UserId = userId
                    //};
                    var user = await _context.ProjectUsers.FirstOrDefaultAsync(pu => pu.UserId == userId);
                    _context.ProjectUsers.Remove(user);
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"*** ERROR *** - Error Adding user to project. --> {ex.Message}");
                    throw;
                }
            }
        }

        //public async Task RemoveUserFromProject(string userId, int projectId)
        //{
        //    var projectUser = await _context.ProjectUsers.FirstOrDefaultAsync(pu => pu.UserId == userId && pu.ProjectId == projectId);
        //    _context.ProjectUsers.Remove(projectUser);
        //    await _context.SaveChangesAsync();
        //}

        public async Task<ICollection<BTUser>> UsersNotOnProject(int projectId)
        {
            var Users = await _context.Users.Where(u => IsUserOnProject(u.Id, projectId).Result == false).ToListAsync();

            //var users = await _context.Users.ToListAsync();
            //ICollection<BTUser> Users = new List<BTUser>();
            //foreach (var user in users)
            //{
            //    var result = IsUserOnProject(user.Id, projectId).Result;
            //    if (result == false) 
            //    {
            //        Users.Add(user);
            //    }
            //}

            return Users;
        }

        //public ICollection<BTUser> UsersNotOnProject(int projectId)
        //{
        //    var users = _context.Users.ToList();
        //    ICollection<BTUser> Users = new HashSet<BTUser>();
        //    foreach (var user in users)
        //    {
        //        if(!IsUserOnProject(user.Id, projectId))
        //        {
        //            Users.Add(user);
        //        }
        //    }
        //    return Users;
        //}

        public async Task<ICollection<BTUser>> UsersOnProject(int projectId)
        {
            Project project = await _context.Projects
                .Include(u => u.ProjectUsers).ThenInclude(u => u.User)
                .FirstOrDefaultAsync(u => u.Id == projectId);
            //List<BTUser> projectusers = new List<BTUser>();
            //List<BTUser> projectusers = project.ProjectUsers.SelectMany(p => (IEnumerable<BTUser>)p.User).ToList();
            List<BTUser> projectusers = project.ProjectUsers.Select(p => p.User).ToList();
            return projectusers;
        }

        //public async Task<ICollection<BTUser>> UsersOnProject(int projectId)
        //{
        //    return await _context.Users.Where(u => IsUserOnProject(u.Id, projectId)).ToListAsync();
        //}

        //public async Task<ICollection<BTUser>> UsersOnProject(int projectId)
        //{
        //    var users = await _context.Users.ToListAsync();
        //    ICollection<BTUser> Users = new HashSet<BTUser>();
        //    foreach (var user in users)
        //    {
        //        if (IsUserOnProject(user.Id, projectId))
        //        {
        //            Users.Add(user);
        //        }
        //    }
        //    return Users;
        //}
    }
}
