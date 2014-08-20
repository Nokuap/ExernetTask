using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Exernet.Models
{
    public class Tag
    {
        [Key]
        public int Id { get; set; }
        public string Text { get; set; }
        public int Rating { get; set; }
        public virtual ICollection<ExernetTask> Tasks { get; set; }
    }
}