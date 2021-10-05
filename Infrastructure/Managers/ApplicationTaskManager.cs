using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnBoardingIdentity.Infrastructure.Data;
using OnBoardingIdentity.Infrastructure.Managers.Interfaces;

namespace OnBoardingIdentity.Infrastructure.Managers
{
    public class ApplicationTaskManager :  ITaskManager
    {
        private readonly ApplicationDbContext _dbContext;

        public ApplicationTaskManager(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public void AddTask(ApplicationTask task)
        {
            throw new NotImplementedException();
        }

        public void DeleteTask(ApplicationTask task)
        {
            throw new NotImplementedException();
        }

        public Task<ApplicationTask[]> GetTaskDetailsAsync(int id, int projectId, int taskId)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _dbContext.SaveChangesAsync()) > 0;
        }
    }
}
