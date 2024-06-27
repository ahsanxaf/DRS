using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DRS.Models;

public partial class DocumentCategory
{
    [Key] // This marks the property as the primary key
    public int CategoryId { get; set; }

    public string? CategoryName { get; set; }

    public string? CategoryDetails { get; set; }

    public DateTime? CreatedDate { get; set; }

    public virtual ICollection<Document>? Documents { get; set; } = new List<Document>();

    public virtual ICollection<Subcategory>? Subcategories { get; set; } = new List<Subcategory>();
}
