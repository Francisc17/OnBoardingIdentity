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
        void AddTask(ApplicationTask task);
        void DeleteTask(ApplicationTask task);
        Task<ApplicationTask[]> GetTaskDetailsAsync(int id, int projectId, int taskId);
        Task<bool> SaveChangesAsync();

    }
}
