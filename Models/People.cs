using System;
using System.ComponentModel.DataAnnotations;

namespace apicore.Models
{
    public class People
    {
        [Key]
        public Guid personId { get; set;}

        [Required]
        [Display(Name = "Name")]
        public string Name { get; set;}

        [Required]
        [Display(Name = "Email")]
        public string Email { get; set;}

        [Display(Name = "Address")]
        public string Address { get; set;}
    }
}
