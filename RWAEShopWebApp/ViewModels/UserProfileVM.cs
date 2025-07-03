using System.ComponentModel.DataAnnotations;

namespace RWAEShopWebApp.ViewModels
{
    public class UserProfileVM
    {
        [Display(Name = "Username")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [Display(Name = "Name")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Last name is required.")]
        [Display(Name = "Last name")]
        public string? LastName { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        [Display(Name = "Phone number")]
        public string? Phone { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string? Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        [Display(Name = "Confirm password")]
        public string? ConfirmPassword { get; set; }
    }
}
