using System.ComponentModel.DataAnnotations;

namespace InnoSpend.Models
{
    public class CustomersDto
    {
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Firstname { get; set; } = "";

        [Required, MaxLength(100)]
        public string Lastname { get; set; } = "";

        [MaxLength(100)]
        [EmailAddress]
        public string Email { get; set; } = "";

        [Required]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; } = "";

        [Required]
        public string Address { get; set; } = "";

        [Required]
        public string Company { get; set; } = "";

        [Required]
        public string Notes { get; set; } = "";
    }
}
