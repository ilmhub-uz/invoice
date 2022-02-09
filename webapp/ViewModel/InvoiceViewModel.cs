using System.ComponentModel.DataAnnotations;

namespace webapp.ViewModel;

public class InvoiceViewModel
{
    [Key]
    public Guid Id { get; set; }
    
    [Required]
    public string Name { get; set; }

    public string 
}