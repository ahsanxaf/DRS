using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DRS.Models;

public partial class Document
{
    [Key] // This marks the property as the primary key
    public int DocId { get; set; }

    public int? CategoryId { get; set; }

    public int? SubcategoryId { get; set; }
    [Required]
    public string? DocTitle { get; set; }

    public string? DdlMinistries { get; set; }

    public string? DdlLawcategory { get; set; }

    //public virtual string? City { get; set; }

    public string? Volume { get; set; }

    public int? PageNo { get; set; }

    public int? Year { get; set; }

    public string? RepealedBy { get; set; }

    public string? AmdtPrinciple { get; set; }

    public string? BookNoNew { get; set; }

    public string? BookNoOld { get; set; }

    public string? BookPageNo { get; set; }

    public int? GazetteYear { get; set; }

    public string? GazettePart { get; set; }

    public int? GazettePageNo { get; set; }

    public string? Srono { get; set; }

    public string? SubDepartment { get; set; }

    public string? Status { get; set; }

    public string? Remarks { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? PublishingDate { get; set; }

    public DateTime? CommencementDate { get; set; }
    [NotMapped]
    public virtual DateTime? Validity { get; set; }

    public string? SecurityLevel { get; set; }

    public string? TargetSubCategory { get; set; }

    public string? TargetSearchDocument { get; set; }

    public string? TitleUrl { get; set; }

    public string? LatestUrl { get; set; }
    [NotMapped]
    public virtual string? DocUrl { get; set; }
    public int? Rules { get; set; }

    public int? Acts { get; set; }
    public int? Ordinance { get; set; }



    public virtual DocumentCategory? Category { get; set; }

    public virtual ICollection<Version>? Versions { get; set; }

    public virtual Subcategory? Subcategory { get; set; }

    public virtual ICollection<Log> Logs { get; set; }

}
