using System.ComponentModel.DataAnnotations;
using webapp.Entity;

namespace webapp.ViewModel
{
    public class EditContactViewModel
    {
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Address { get; set; }

        [Required,Phone]
        [RegularExpression(
        @"^[\+]?(998[-\s\.]?)([0-9]{2}[-\s\.]?)([0-9]{3}[-\s\.]?)([0-9]{2}[-\s\.]?)([0-9]{2}[-\s\.]?)$", 
        ErrorMessage = "Telefon raqam formati noto'g'ri.")]
        public string Phone { get; set; }

        [Required,EmailAddress]
        public string Email { get; set; }
        public virtual AppUser Owner { get; set; }
        public virtual Organization Organization { get; set; }
    }
}