using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Exernet.Models
{
    public class Like
    {
        [Key]
        public int Id { get; set; }
        public virtual ExernetTask Task { get; set; }
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
        public Boolean Type { get; set; }
    }
}