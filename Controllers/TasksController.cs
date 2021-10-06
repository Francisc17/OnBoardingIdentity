﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using OnBoardingIdentity.Infrastructure.Data;
using OnBoardingIdentity.Models.TaskBindings;

namespace OnBoardingIdentity.Controllers
{
    public class TasksController : BaseApiController
    {
        //create task
        [Route("api/project/{projId:int}/tasks",Name = "CreateTask")]
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

                var project = await AppProjectManager.GetUserProjectAsync(User.Identity.GetUserId(), projId, true);
                if (project == null) return NotFound();

                ApplicationUser user = null;

                if(model.TaskResponsibleId != null)
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
    }
}