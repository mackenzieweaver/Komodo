using Komodo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Komodo.Services
{
    public interface IBTProjectService
    {
        //public bool IsUserOnProject(string userId, int projectId);
        public Task<bool> IsUserOnProject(string userId, int projectId);
        public Task<ICollection<Project>> ListUserProjects(string userId);
        public Task AddUserToProject(string userId, int projectId);
        public Task RemoveUserFromProject(string userId, int projectId);
        public Task<ICollection<BTUser>> UsersOnProject(int projectId);
        public Task<ICollection<BTUser>> UsersNotOnProject(int projectId);
        public List<BTUser> SortListOfDevsByTicketCountAsync(List<BTUser> users, List<Ticket> tickets);
    }
}
