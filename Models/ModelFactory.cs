using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Routing;
using Microsoft.AspNet.Identity.EntityFramework;
using OnBoardingIdentity.Infrastructure;
using OnBoardingIdentity.Infrastructure.Data;
using OnBoardingIdentity.Infrastructure.Data.Enums;

namespace OnBoardingIdentity.Models
{
    public class ModelFactory
    {
        private UrlHelper _UrlHelper;
        private ApplicationUserManager _AppUserManager;

        public ModelFactory(HttpRequestMessage request, ApplicationUserManager appUserManager)
        {
            _UrlHelper = new UrlHelper(request);
            _AppUserManager = appUserManager;
        }

        public UserReturnModel Create(ApplicationUser appUser)
        {
            return new UserReturnModel
            {
                Url = _UrlHelper.Link("GetUserById", new { id = appUser.Id }),
                Id = appUser.Id,
                UserName = appUser.UserName,
                FullName = string.Format("{0} {1}", appUser.FirstName, appUser.LastName),
                Email = appUser.Email,
                EmailConfirmed = appUser.EmailConfirmed,
                Level = appUser.Level,
                JoinDate = appUser.JoinDate,
                Roles = _AppUserManager.GetRolesAsync(appUser.Id).Result,
                Claims = _AppUserManager.GetClaimsAsync(appUser.Id).Result
            };
        }

        public ProjectReturnModel Create(ApplicationProject appProject)
        {
            var model = new ProjectReturnModel
            {
                //TODO: Implement URL like it is in users up there!
                Id = appProject.Id,
                ProjectName = appProject.ProjectName,
                Budget = appProject.Budget,
            };
            if (appProject.ProjectManager != null)
                model.ProjectManager = this.Create(appProject.ProjectManager);

            //result.Select(c => { return TheModelFactory.Create(c); }).ToList()

            if (appProject.Tasks != null)
                model.Tasks = appProject.Tasks.Select(c => { return this.Create(c); }).ToList();

            return model;
        }

        public TaskReturnModel Create(ApplicationTask appTask)
        {
            var model = new TaskReturnModel
            {
                //TODO: Implement URL like it is in users up there!
                Id = appTask.Id,
                TaskName = appTask.TaskName,
                Deadline = appTask.Deadline,
                State = appTask.State,
                Responsible = null
            };

            if (appTask.TaskResponsible != null)
                model.Responsible = this.Create(appTask.TaskResponsible);

            if (appTask.Project != null)
            {
                model.projectId = appTask.Project.Id;
                model.projectName = appTask.Project.ProjectName;
            }
                
            return model;
        }
    }

    public class UserReturnModel
    {
        public string Url { get; set; }
        public string Id { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public int Level { get; set; }
        public DateTime JoinDate { get; set; }
        public IList<string> Roles { get; set; }
        public IList<System.Security.Claims.Claim> Claims { get; set; }
    }

    public class ProjectReturnModel
    {
        public string Url { get; set; }
        public int Id { get; set; }
        public string ProjectName { get; set; }
        public int Budget { get; set; }
        public UserReturnModel ProjectManager { get; set; }
        public ICollection<TaskReturnModel> Tasks { get; set; }
    }

    public class TaskReturnModel
    {
        public string Url { get; set; }
        public int Id { get; set; }
        public string TaskName { get; set; }
        public DateTime Deadline { get; set; }
        public TaskState State { get; set; }
        public UserReturnModel Responsible { get; set; }

        //project info

        public int projectId { get; set; }
        public string projectName { get; set; }    }
}
