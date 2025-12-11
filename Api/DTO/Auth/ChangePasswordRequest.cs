using System.ComponentModel.DataAnnotations;

namespace YssWebstoreApi.Api.DTO.Auth
{
    public class ChangePasswordRequest
    {
        public required string OldPassword { get; set; }
        
        [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[@$!%*?&])[A-Za-z\\d@$!%*?&]{8,}$",
                    ErrorMessage = "Your password must be at least 8 characters long and include at least one uppercase letter, one lowercase letter, one number, and one special character.")]
        public required string NewPassword { get; set; }
    }
}
