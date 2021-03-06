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
using OnBoardingIdentity.Models.TaskBindings;

namespace OnBoardingIdentity.Controllers
{
    public class TasksController : BaseApiController
    {
        //create task
        [Route("api/project/{projId:int}/tasks", Name = "CreateTask")]
        [Authorize(Roles = "Gestor Projeto")]
        [HttpPost]
        public async Task<IHttpActionResult> CreateTask(int projId, CreateTaskBinding model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest();

                var task = new ApplicationTask()
                {
                    TaskName = model.TaskName,
                    Deadline = model.Deadline,
                    State = model.State
                };

                var project = await AppProjectManager.GetUserProjectAsync(User.Identity.GetUserId(), projId, false);
                if (project == null) return NotFound();

                Console.WriteLine(project);

                ApplicationUser user = null;

                if (model.TaskResponsibleId != null)
                {
                    user = AppUserManager.FindById(model.TaskResponsibleId);
                    if (user == null) return NotFound();
                }

                AppTaskManager.AddTask(task, project, user);

                if (await AppTaskManager.SaveChangesAsync())
                {
                    //TODO: Change this to be like AccountsController return on POST
                    return Ok("Task created");
                }
            }
            catch (Exception ex)
            {

                return InternalServerError(ex);
            }

            return BadRequest();
        }

        [Route("api/user/tasks")]
        [Authorize(Roles = "Programador")]
        [HttpGet]
        public async Task<IHttpActionResult> GetAllUserTasksResponsible(bool includeRespInfo = false)
        {
            try
            {
                var result = await AppTaskManager.GetProgrammerTasks(User.Identity.GetUserId(), includeRespInfo);
                if (result == null) return NotFound();

                return Ok(result.Select(c => { return TheModelFactory.Create(c); }).ToList());
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [Route("api/user/tasks/{taskId:int}")]
        [Authorize(Roles = "Programador, Gestor Projeto")]
        [HttpGet]
        public async Task<IHttpActionResult> GetTaskDetails(int taskId, bool includeRespInfo = false, bool includeProjInfo = false)
        {
            try
            {
                var result = await AppTaskManager.GetTaskDetailsAsync(User.Identity.GetUserId(), taskId, includeRespInfo, includeProjInfo);
                if (result == null) return NotFound();

                return Ok(TheModelFactory.Create(result));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }


        [Route("api/user/project/{projId:int}/tasks")]
        [Authorize(Roles = "Gestor Projeto")]
        [HttpGet]
        public async Task<IHttpActionResult> GetProjectTasks(int projId, bool includeRespInfo = false)
        {
            try
            {
                var result = await AppTaskManager.GetProjectTasks(User.Identity.GetUserId(), projId, includeRespInfo);
                if (result == null) return NotFound();

                return Ok(result.Select(c => { return TheModelFactory.Create(c); }).ToList());
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [Route("api/user/tasks/{taskId:int}")]
        [Authorize(Roles = "Gestor Projeto")]
        [HttpDelete]
        public async Task<IHttpActionResult> deleteTask(int taskId)
        {
            try
            {
                var result = await AppTaskManager.GetTaskDetailsAsync(User.Identity.GetUserId(), taskId);
                if (result == null) return NotFound();

                AppTaskManager.DeleteTask(result);

                if (await AppTaskManager.SaveChangesAsync())
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

        [Route("api/user/tasks/{taskId:int}")]
        [Authorize(Roles = "Gestor Projeto, Programador ")]
        [HttpPut]
        public async Task<IHttpActionResult> updateTask(int taskId, CreateTaskBinding model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest();

                var result = await AppTaskManager.GetTaskDetailsAsync(User.Identity.GetUserId(), taskId, true, true);
                if (result == null) return NotFound();

                result.TaskName = model.TaskName;
                result.State = model.State;

                try
                {
                    if (await AppTaskManager.SaveChangesAsync())
                    {
                        return Ok(TheModelFactory.Create(result));
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

                return InternalServerError();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
 }
