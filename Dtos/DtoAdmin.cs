using System.ComponentModel.DataAnnotations;

namespace assetsment_Celsia.Dtos
{
    public class DtoAdmin
    {
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
        public int RoleId { get; set; }
    }
}