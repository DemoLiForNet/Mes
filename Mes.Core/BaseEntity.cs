using System;
using System.ComponentModel.DataAnnotations;

namespace Mes.Core
{
    public class BaseEntity
    {
        public long Id { get; set; }
        [Required]
        [StringLength(128)]
        public string CreatedByUser { get; set; }
        [Required]
        public DateTime CreatedOn { get; set; }
        [StringLength(128)]
        public string ModifyByUser { get; set; }
        [DataType("datetime2")]
        public DateTime ModifedOn { get; set; }
        public bool IsDelete { get; set; }
    }
}
