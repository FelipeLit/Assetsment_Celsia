using System.ComponentModel.DataAnnotations;

namespace assetsment_Celsia.Models
{
    public class Client
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Address is requiered.")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Phone number requiered.")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; }
        public string Status { get; set; }
        public int RoleId { get; set; }
    }
}