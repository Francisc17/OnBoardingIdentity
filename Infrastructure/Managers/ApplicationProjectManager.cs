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
    public class ApplicationProjectManager :IProjectManager
    {
        private readonly ApplicationDbContext _dbContext;

        public ApplicationProjectManager(ApplicationDbContext dbContext)
        {
           _dbContext = dbContext;
        }

        public void AddProject(ApplicationProject project, ApplicationUser user)
        {
            _dbContext.Project.Add(project);

            //we need to attach and then equalize to get EF know
            //that we want an instance that already exists in database!
            //https://entityframework.net/knowledge-base/7884887/prevent-entity-framework-to-insert-values-for-navigational-properties
            _dbContext.Users.Attach(user);
            project.ProjectManager = user;
        }

        public void DeleteProject(ApplicationProject project)
        {
            _dbContext.Project.Remove(project);
        }

        public async Task<ApplicationProject[]> GetAllUserProjectsAsync(string id, bool includeTasks = false,
            bool includePMInfo = false)
        {
            IQueryable<ApplicationProject> query = _dbContext.Project;

            if (includeTasks)
            {
                query = query.Include(p => p.Tasks.Select(r => r.TaskResponsible));
            }

            if (includePMInfo)
            {
                query = query.Include(p => p.ProjectManager);
            }

            query = query.Where(p => p.ProjectManager.Id == id);

            return await query.ToArrayAsync();
        }

        public async Task<ApplicationProject> GetUserProjectAsync(string id, int projectId,
            bool includeTasks = false, bool includePMInfo = false)
        {
            IQueryable<ApplicationProject> query = _dbContext.Project;

            if (includeTasks)
            {
                query = query.Include(p => p.Tasks.Select(r => r.TaskResponsible));
            }

            if (includePMInfo)
            {
                query = query.Include(p => p.ProjectManager);
            }

            //this way we can only obtain project if the user is the manager of it.
            query = query.Where(p => p.ProjectManager.Id == id && p.Id == projectId);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _dbContext.SaveChangesAsync()) > 0;
        }
    }
}