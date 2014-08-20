using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Exernet.Models
{
    public class Comment {
        [Key]
        public int Id { get; set; }
        public ExernetTask Task { get; set; }
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
        public DateTime Date { get; set; }
        public string Text { get; set; }
    }
}