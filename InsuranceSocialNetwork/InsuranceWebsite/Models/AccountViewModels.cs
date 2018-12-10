using InsuranceWebsite.Validators;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace InsuranceWebsite.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "EmailRequired")]
        [Display(Name = "Email", ResourceType = typeof(Resources.Resources))]
        public string Email { get; set; }
    }

    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }

    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }

    public class VerifyCodeViewModel
    {
        [Required]
        public string Provider { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "CodeRequired")]
        [Display(Name = "Code", ResourceType = typeof(Resources.Resources))]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = "RememberThisBrowser", ResourceType = typeof(Resources.Resources))]
        public bool RememberBrowser { get; set; }

        public bool RememberMe { get; set; }
    }

    public class ForgotViewModel
    {
        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "EmailRequired")]
        [Display(Name = "Email", ResourceType = typeof(Resources.Resources))]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {
        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "EmailRequired")]
        [Display(Name = "Email", ResourceType = typeof(Resources.Resources))]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "PasswordRequired")]
        [DataType(DataType.Password)]
        [Display(Name = "Password", ResourceType = typeof(Resources.Resources))]
        public string Password { get; set; }

        [Display(Name = "RememberMe", ResourceType = typeof(Resources.Resources))]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "NameRequired")]
        [StringLength(32, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        [Display(Name = "Name", ResourceType = typeof(Resources.Resources))]
        public string Name { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "EmailRequired")]
        [EmailAddress(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "InvalidEmail")]
        [Display(Name = "Email", ResourceType = typeof(Resources.Resources))]
        public string Email { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "PasswordRequired")]
        [StringLength(100, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "FieldLength", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password", ResourceType = typeof(Resources.Resources))]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "ConfirmPassword", ResourceType = typeof(Resources.Resources))]
        [Compare("Password", ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "PasswordNotMatch")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "RoleRequired")]
        [Display(Name = "UserRole", ResourceType = typeof(Resources.Resources))]
        public string UserRole { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "ProfessionalNumberRequired")]
        [Display(Name = "ProfessionalNumber", ResourceType = typeof(Resources.Resources))]
        public string ProfessionalNumber { get; set; }

        //[Range(typeof(bool), "true", "true", ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "MustAcceptTermsAndConditions")]
        [MustBeTrueAttribute(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "MustAcceptTermsAndConditions")]
        [Display(Name = "TermsAndConditionsLabel", ResourceType = typeof(Resources.Resources))]
        public bool TermsAndConditions { get; set; }

        public List<System.Web.Mvc.SelectListItem> UserRoles { get; set; }
    }

    public class ResetPasswordViewModel
    {
        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "EmailRequired")]
        [EmailAddress]
        [Display(Name = "Email", ResourceType = typeof(Resources.Resources))]
        public string Email { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "PasswordRequired")]
        [StringLength(100, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "FieldLength", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password", ResourceType = typeof(Resources.Resources))]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "ConfirmPassword", ResourceType = typeof(Resources.Resources))]
        [Compare("Password", ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "PasswordNotMatch")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "EmailRequired")]
        [EmailAddress]
        [Display(Name = "Email", ResourceType = typeof(Resources.Resources))]
        public string Email { get; set; }
    }
}
