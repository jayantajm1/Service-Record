using Service_Record.DAL.Enums;
using System.ComponentModel.DataAnnotations;
using System.Dynamic;


namespace Service_Record.Models.DTOs
{
    public class BaseDTO
    {

        public ExpandoObject? DataSource { get; set; }
    }


    public class DateOnlyDTO
    {
        [DataType(DataType.Date)]
        public DateOnly DateOnly { get; set; }
    }

    public class CaptchaResultDTO
    {
        public string CaptchaCode { get; set; }
        public byte[] CaptchaByteData { get; set; }
        public string CaptchBase64Data => Convert.ToBase64String(CaptchaByteData);
        public DateTime Timestamp { get; set; }
    }
    public class CapcthaGetDTO
    {
        public string captchaImg { get; set; }
        public int captchaId { get; set; }
    }

    public class UserRegistrationDTO : BaseDTO
    {
      

        [Required]
        public string FullName { get; set; } = null!;
        [Required]
        public string Username { get; set; } = null!;
        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;
        [Required]
        public string Password { get; set; } = null!;
        // Assuming your 'ark.py' script created the UserModel
        // You might need to add the enum property manually to the UserModel if it wasn't there
         public UserRole Role { get; set; }
    }

    public class JwtSettings
    {
        public string Key { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int TokenExpiryInHours { get; set; }
    }



    public class LoginDTO
    {
        [Required]
        public string Username { get; set; } = null!;
        [Required]
        public string Password { get; set; } = null!;
    }

    public class AuthResponseDTO
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = null!;
        public string? Token { get; set; }
    }
}