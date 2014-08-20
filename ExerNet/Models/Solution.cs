using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Exernet.Models
{
    public class Solution
    {
        [Key]
        public int Id { get; set; }
        public virtual ExernetTask Task { get; set; }
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
        public DateTime UploadDate { get; set; }
        public bool Correct { get; set; }
    }
}