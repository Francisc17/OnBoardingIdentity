using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using Microsoft.AspNet.Identity;
using OnBoardingIdentity.Infrastructure.Data;
using OnBoardingIdentity.Models.ProjectBindings;

namespace OnBoardingIdentity.Controllers
{
    [RoutePrefix("api/projects")]
    public class ProjectsController : BaseApiController
    {

        //Create Project
        [Authorize(Roles = "Gestor Projeto")]
        [Route()]
        [HttpPost]
        public async Task<IHttpActionResult> Post(CreateProjectBindingModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest();

                var project = new ApplicationProject()
                {
                    ProjectName = model.ProjectName,
                    Budget = model.Budget,
                    ProjectManager = null
                };

                AppProjectManager.AddProject(project, AppUserManager.FindById(User.Identity.GetUserId()));


                try
                {
                    if (await AppProjectManager.SaveChangesAsync())
                    {
                        //TODO: Change this to be like AccountsController return on POST
                        return Ok("User created");
                    }
                }
                catch (DbEntityValidationException ex)
                {
                    foreach (var entityValidationErrors in ex.EntityValidationErrors)
                    {
                        foreach (var validationError in entityValidationErrors.ValidationErrors)
                        {
                            return BadRequest("Property: " + validationError.PropertyName + " Error: " + validationError.ErrorMessage);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

            return BadRequest();
        }

        //Get all Project manager associated projects
        [Route(Name = "GetUserProjects")]
        [Authorize(Roles = "Gestor Projeto")]
        [HttpGet]
        public async Task<IHttpActionResult> GetUserProjects(bool includeTasks = false,
            bool includePMInfo = false)
        {
            try
            {
                var result = await AppProjectManager.GetAllUserProjectsAsync(User.Identity.GetUserId(),
                    includeTasks, includePMInfo);
                if (result == null) return NotFound();

                //for each to create and return a model for each Project
                return Ok(result.Select(c => { return TheModelFactory.Create(c); }).ToList());
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [Route("{ProjectId:int}")]
        [Authorize(Roles = "Gestor Projeto")] //maybe also programador?
        [HttpGet]
        public async Task<IHttpActionResult> GetProjectDetails(int projectId, bool includeTasks = false,
            bool includePMInfo = false)
        {
            try
            {
                var result = await AppProjectManager.GetUserProjectAsync(
                    User.Identity.GetUserId(), projectId, includeTasks, includePMInfo);
                if (result == null) return NotFound();

                return Ok(TheModelFactory.Create(result));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [Route("{ProjectId:int}")]
        [Authorize(Roles = "Gestor Projeto")]
        [HttpDelete]
        public async Task<IHttpActionResult> deleteProject(int projectId)
        {
            try
            {
                var result = await AppProjectManager.GetUserProjectAsync(User.Identity.GetUserId(), projectId);
                if (result == null) return NotFound();

                AppProjectManager.DeleteProject(result);

                if (await AppProjectManager.SaveChangesAsync())
                {
                    return Ok();
                }

                return InternalServerError();

            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [Route("{ProjectId:int}")]
        [Authorize(Roles = "Gestor Projeto")]
        [HttpPut]
        public async Task<IHttpActionResult> ChangeProject(int projectId, CreateProjectBindingModel model)
        {
            try
            {
                var result = await AppProjectManager.GetUserProjectAsync(User.Identity.GetUserId(), projectId,true,true);
                if (result == null) return NotFound();


                //these are the only 2 camps that we can change
                result.ProjectName = model.ProjectName;
                result.Budget = model.Budget;

                //AppProjectManager.UpdateProject(User.Identity.GetUserId(), projectId, result);

                await AppProjectManager.SaveChangesAsync();
                    
                return Ok(TheModelFactory.Create(result));

            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}