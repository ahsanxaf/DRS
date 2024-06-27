using System;
using System.Collections.Generic;

namespace DRS.Models;

public partial class Version
{
    public int VersionId { get; set; }

    public int? DocId { get; set; }
    public virtual Document? Document { get; set; } // Add this navigation property

    public int? VersionNo { get; set; }

    public int? Vyear { get; set; }

    public string? Vnumber { get; set; }

    public int? VgazetteYear { get; set; }
    public string? VgazettePart { get; set; }
    public int? VgazettePage { get; set; }


    public string? DocumentName { get; set; }

    public string? DocUrl { get; set; }

    public DateTime? CommencementDate { get; set; }

    public DateTime? PublishingDate { get; set; }

    public DateTime? ValidityDate { get; set; }

    public string? VdocNature { get; set; }

    public string? Vstatus { get; set; }

    public string? Remarks { get; set; }

    public DateTime? UploadDate { get; set; }

}
