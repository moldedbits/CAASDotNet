using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace CAAS.Models.ViewModels.Account
{
    public class ExternalLoginConfirmationViewModel
    {
    [Required]
    [EmailAddress]
    public string Email { get; set; }
  }
}
