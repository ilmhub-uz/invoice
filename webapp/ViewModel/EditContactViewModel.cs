using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using webapp.Entity;

namespace webapp.ViewModel
{
 public class EditContactViewModel
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage ="Korxona Nomini Kiritish shart ! ")]
        [Display(Name = "Korxona Nomi")]
        public string Name { get; set; }

        [Required(ErrorMessage ="Manzilni To'liq Kiriting !")]
        [Display(Name = "Manzil")]
        public string Address { get; set; }

        [Required, Phone]
        [RegularExpression(
        @"^[\+]?(998[-\s\.]?)([0-9]{2}[-\s\.]?)([0-9]{3}[-\s\.]?)([0-9]{2}[-\s\.]?)([0-9]{2}[-\s\.]?)$",
        ErrorMessage = "Telefon raqam formati noto'g'ri.")]
        [Display(Name = "Telefon Raqami")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Email manzil kiritish shart!")]
        [EmailAddress(ErrorMessage = "Email manzil formati noto'g'ri.")]
        [DisplayName("Email manzil")]
        public string Email { get; set; }

        [Required(ErrorMessage ="Raxbarni Ismi Sharifini kiriting !")]
        [Display(Name = "Korxona Raxbari")]
        public virtual AppUser Owner { get; set; }
    }
}