using System;
using System.ComponentModel.DataAnnotations;

namespace DRS.Models
{
    public partial class LawCategory
    {
        [Key]
        public int LawCategoryId { get; set; }

        public string? LawCategoryName { get; set; }
    }
}
