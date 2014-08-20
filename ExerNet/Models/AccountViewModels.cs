using Exernet.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Web;

namespace Exernet.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }
    }

    public class ManageUserViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class LoginViewModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
    public class ShowUserViewModel
    {
        public string UserName { get; set; }
        public int Rating { get; set; }
        public string Email { get; set; }
        public string ProfileFotoURL { get; set; }
        public int CommentsQuantity { get; set; }
        public int ResolvedTasksQuantity { get; set; }
        public int TasksQuantity { get; set; }
        public int SolutionsQuantity { get; set; }
        public IEnumerable<Solution> Solutions { get; set; }
        public IEnumerable<ExernetTask> Tasks { get; set; }
    }

    public class ExernetTaskViewModel
    {
        [Display(Name = "Id")]
        public int Id { get; set; }
        [Required]
        [Display(Name = "Category")]
        public string Category { get; set; }

        [Required(ErrorMessage = "Введите название.")]
        [Display(Name = "Title")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Опишите задачу.")]
        [Display(Name = "Text")]
        public string Text { get; set; }

        // String fields for ICollections 

        [Required(ErrorMessage = "Требуется хотя бы один ответ.")]
        [Display(Name = "Answers")]
        public string Answers { get; set; }

        [Required(ErrorMessage = "Требуется хотя бы один тэг.")]
        [Display(Name = "Tags")]
        public string Tags { get; set; }

        [Display(Name = "Videos")]
        public string Videos { get; set; }

        //[Display(Name = "Images")]
        public IEnumerable<Image> Images { get; set; }

        [Display(Name = "Formulas")]
        public string Formulas { get; set; }

        [Display(Name = "Charts")]
        public string Charts { get; set; }


    }
    public class JsonModel
    {
        public string HTMLString { get; set; }
        public bool NoMoreData { get; set; }
    }
    //public class UsersViewModel
    //{
    //    public IEnumerable<ApplicationUser> Users { get; set; }
    //}
}