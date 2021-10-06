using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnBoardingIdentity.Infrastructure.Data;
using OnBoardingIdentity.Infrastructure.Data.Enums;

namespace OnBoardingIdentity.Models.TaskBindings
{
    public class CreateTaskBinding
    {
        //TODO: Add error messages
        //TODO: Add verifications

        [Required]
        public string TaskName { get; set; }

        [Required]
        public DateTime Deadline { get; set; }

        [Required]
        public TaskState State { get; set; } = TaskState.Starting;

        //it is not required because it can be a task without a responsible
        //The responsible can be added or associated after create the task
        public string TaskResponsibleId { get; set; } = null;
    }
}
