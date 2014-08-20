using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Exernet.Models
{
    public class ExernetTask
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
        public string Category { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public bool Block { get; set; }
        public DateTime UploadDate { get; set; }
        public virtual ICollection<Comment> Comments{ get; set; }
        public virtual ICollection<Like> Likes { get; set; }
        public virtual ICollection<Video> Videos { get; set; }
        public virtual ICollection<Image> Images { get; set; }
        public virtual ICollection<Answer> Answers { get; set; }
        public virtual ICollection<Formula> Formulas { get; set; }
        public virtual ICollection<Chart> Charts { get; set; }
        public virtual ICollection<Solution> Solutions { get; set; }
        public virtual ICollection<Tag> Tags { get; set; }
    }
}