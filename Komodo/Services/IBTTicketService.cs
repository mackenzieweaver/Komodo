using Komodo.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;

namespace Komodo.Services
{
    public interface IBTTicketService
    {
        Task<SelectList> GetDevs(BTUser user);
        SelectList GetProjects();
        SelectList GetPriorities();
        SelectList GetStatuses();
        SelectList GetTypes();
    }
}
