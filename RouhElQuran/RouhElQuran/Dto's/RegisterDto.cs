using System.ComponentModel.DataAnnotations;

namespace RouhElQuran.Dto_s
{
    public class RegisterDto
    {
        [Required]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "FirstName must be at least 2 characters.")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "LastName must be at least 2 characters.")]
        public string LastName { get; set; }

        [Required]
        //[StringLength(20, MinimumLength = 2, ErrorMessage = "Country must be exactly 2 characters.")]
        public string Country { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        //[StringLength(20, MinimumLength = 6, ErrorMessage = "Password must be between 6 and 20 characters.")]
        //[RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{6,20}$",
        //    ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, one digit, and one special character.")]
        public string Password { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        public string? UserRole { get; set; }

        [Required]
        public string Language { get; set; }

        //[RegularExpression(@"^.*\.(jpg|JPG|png|PNG)$", ErrorMessage = "Please select a valid JPG or PNG file.")]
        public IFormFile? PersonalImage { get; set; }
    }
}