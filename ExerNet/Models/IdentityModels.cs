using Exernet.Code;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Data.Entity.Infrastructure.Interception;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace Exernet.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public int Rating { get; set; }
        [DefaultValue("http://res.cloudinary.com/goodcloud/image/upload/v1408205093/Exernet/ProfilePictures/j17mw98npc0qr5qrayhp.png")]
        public string ProfileFotoURL { get; set; }
        public bool IsAdmin { get; set; }
        public bool isBlocked { get; set; }
        public virtual ICollection<Solution> Solutions { get; set; }
        public virtual ICollection<ExernetTask> Tasks { get; set; }
        public virtual ICollection<Like> Likes { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {

        public ApplicationDbContext()
            : base("DefaultConnection")
        {
            
        }
        public DbSet<ExernetTask> Tasks { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Chart> Charts { get; set; }
        public DbSet<Formula> Formulas { get; set; }
        public DbSet<Video> Videos { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Solution> Solutions { get; set; }
        public DbSet<Answer> Answers { get; set; }

        
    }

    
   
}