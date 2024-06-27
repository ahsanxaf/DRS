using System;
using System.Collections.Generic;

namespace DRS.Models;

public partial class Subcategory
{
    public int SubcategoryId { get; set; }

    public int CategoryId { get; set; }

    public string? SubcategoryName { get; set; }

    public string? SubcategoryDetails { get; set; }

    public DateTime? CreatedDate { get; set; }

    public virtual DocumentCategory? Category { get; set; } = null!;

    public virtual ICollection<Document>? Documents { get; set; } = new List<Document>();
}
