using System;
using System.Collections.Generic;
using System.Data.Entity;
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
        public void AddTask(ApplicationTask task, ApplicationProject project, ApplicationUser user = null)
        {
            _dbContext.Tasks.Add(task);

            //we need to attach and then equalize to get EF know
            //that we want an instance that already exists in database!
            //https://entityframework.net/knowledge-base/7884887/prevent-entity-framework-to-insert-values-for-navigational-properties

            if (user != null)
            {
                _dbContext.Users.Attach(user);
                task.TaskResponsible = user;
            }

            _dbContext.Project.Attach(project);
            task.Project = project;
        }

        public void DeleteTask(ApplicationTask task)
        {
            _dbContext.Tasks.Remove(task);
        }

        public async Task<ApplicationTask[]> GetProgrammerTasks(string id, bool includeRespInfo = false)
        {
            IQueryable<ApplicationTask> query = _dbContext.Tasks;

            if (includeRespInfo)
            {
                query = query.Include(p => p.TaskResponsible);
            }

            query = query.Where(p => p.TaskResponsible.Id == id);

            return await query.ToArrayAsync();
        }

        public async Task<ApplicationTask[]> GetProjectTasks(string id, int projectId, bool includeRespInfo = false)
        {
            IQueryable<ApplicationTask> query = _dbContext.Tasks;

            if (includeRespInfo)
            {
                query = query.Include(p => p.TaskResponsible);
            }

            query = query.Where(p => p.Project.ProjectManager.Id == id && p.Project.Id == projectId);

            return await query.ToArrayAsync();

        }

        public async Task<ApplicationTask> GetTaskDetailsAsync(string id, int taskId, bool includeRespInfo = false)
        {
            IQueryable<ApplicationTask> query = _dbContext.Tasks;

            if (includeRespInfo)
            {
                query = query.Include(p => p.TaskResponsible);
            }

            //We can return results if either the respective project manager or task responsible is calling the endpoint
            query = query.Where(p => (p.TaskResponsible.Id == id || p.Project.ProjectManager.Id == id) && p.Id == taskId);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _dbContext.SaveChangesAsync()) > 0;
        }
    }
}
