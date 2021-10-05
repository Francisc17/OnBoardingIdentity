using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnBoardingIdentity.Infrastructure.Data
{
    public class ApplicationProject
    {
        public int Id { get; set; }
        [Required]
        public string ProjectName { get; set; }
        [Required]
        public int Budget { get; set; }
        [Required]
        public ApplicationUser ProjectManager { get; set; }
        public ICollection<ApplicationTask> Tasks { get; set; }
    }
}
