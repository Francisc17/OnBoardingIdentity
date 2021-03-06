using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnBoardingIdentity.Infrastructure.Data.Enums;

namespace OnBoardingIdentity.Infrastructure.Data
{
    public class ApplicationTask
    {
        //TODO: Add verifications

        public int Id { get; set; }

        [Required]
        public string TaskName { get; set; }

        [Required]
        public DateTime Deadline { get; set; }

        public TaskState State { get; set; } = TaskState.Starting;

        //it can be a task without a responsible yet
        public ApplicationUser TaskResponsible { get; set; } = null;

        [Required]
        public ApplicationProject Project { get; set; }
    }
}
