using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnBoardingIdentity.Models.AccountBindings;

namespace OnBoardingIdentity.Models.ProjectBindings
{
    public class CreateProjectBindingModel
    {
        [Required]
        [StringLength(30)]
        public string ProjectName { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int Budget { get; set; }

        //TODO: Add CreateTaskBindingModel here!
        //TODO: Add error messages
    }
}