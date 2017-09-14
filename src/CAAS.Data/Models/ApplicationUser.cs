using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace CAAS.Models
{
    public class ApplicationUser : IdentityUser
    {
        [NotMapped]
        public Author Author { get; set; }
    }
}
