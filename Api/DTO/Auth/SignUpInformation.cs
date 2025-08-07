using System.ComponentModel.DataAnnotations;

namespace YssWebstoreApi.Api.DTO.Auth
{
    public class SignUpInformation
    {
        [EmailAddress]
        public required string Email { get; set; }
        
        [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[@$!%*?&])[A-Za-z\\d@$!%*?&]{8,}$", 
            ErrorMessage = "Your password must be at least 8 characters long and include at least one uppercase letter, one lowercase letter, one number, and one special character.")]
        public required string Password { get; set; }
        
        [Length(3, 60)]
        public required string UniqueName { get; set; }
        
        [Length(1, 80)]
        public required string DisplayName { get; set; }
    }
}
