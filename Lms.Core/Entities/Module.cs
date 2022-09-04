using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lms.Core.Entities
{
    public class Module
    {
        [Required]
        [StringLength(20)]
        public int Id { get; set; }

        public string Title { get; set; }

        public DateTime StartDate { get; set; }

        public int CourseId { get; set; }

        //public Course Course { get; set; }
    }
}
