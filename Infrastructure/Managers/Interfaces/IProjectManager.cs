using System.Threading.Tasks;
using OnBoardingIdentity.Infrastructure.Data;

namespace OnBoardingIdentity.Infrastructure.Managers.Interfaces
{
    public interface IProjectManager
    {
        void AddProject(ApplicationProject project, ApplicationUser user);
        void DeleteProject(ApplicationProject project);
        Task<ApplicationProject[]> GetAllUserProjectsAsync(string id, bool includeTasks = false, bool includePMInfo = false);
        Task<ApplicationProject> GetUserProjectAsync(string id, int projectId, bool includeTasks = false, bool includePMInfo = false);
        Task<bool> SaveChangesAsync();

    }
}
