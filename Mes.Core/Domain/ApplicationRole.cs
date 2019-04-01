using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Mes.Core.Domain
{
    public class ApplicationRole: IdentityRole
    {
        [Required]
        [StringLength(32)]
        public string DisplayName { get; set; }
    }
}
