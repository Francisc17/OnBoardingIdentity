using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnBoardingIdentity.Infrastructure.Data;

namespace OnBoardingIdentity.Infrastructure.Managers.Interfaces
{
    public interface ITaskManager
    {
        void AddTask(ApplicationTask task, ApplicationProject project, ApplicationUser user = null);
        void DeleteTask(ApplicationTask task);
        Task<ApplicationTask[]> GetProgrammerTasks(string id, bool includeRespInfo = false);
        Task<ApplicationTask> GetTaskDetailsAsync(string id, int taskId, bool includeRespInfo = false);
        Task<ApplicationTask[]> GetProjectTasks(string id, int projectId, bool includeRespInfo = false);
        Task<bool> SaveChangesAsync();
    }
}
