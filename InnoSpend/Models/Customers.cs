using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore; 

namespace InnoSpend.Models
{
    public class CustomerInfo
    {
        public int Id { get; set; }

        [MaxLength(100)]
        public string Firstname { get; set; } = "";

        [MaxLength(100)]
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

        [MaxLength(255)]
        public string Notes { get; set; } = "";
        public DateTime CreatedAt { get; set; }
    }
}
