using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lms.Core.Entities
{
    public class Course
    {
        [Required]
        [StringLength(20)]
        public int Id { get; set; }

        public string Title { get; set; }

        public DateTime StartDate { get; set; }

        public ICollection<Module> Modules { get; set; }
    }
}
