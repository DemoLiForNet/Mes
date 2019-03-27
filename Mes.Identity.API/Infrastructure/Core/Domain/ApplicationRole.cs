using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Mes.Identity.API.Infrastructure.Core.Domain
{
    public class ApplicationRole: IdentityRole
    {
        [Required]
        [StringLength(50)]
        public string DisplayName { get; set; }
    }
}
