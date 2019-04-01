using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Mes.Core.Domain
{
    public class ApplicationUser: IdentityUser
    {
        [StringLength(64)]
        public string NickName { get; set; }
    }
}
