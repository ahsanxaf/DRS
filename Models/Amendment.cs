using System;
using System.Collections.Generic;

namespace DRS.Models;

public partial class Amendment
{

    public int? AmendId { get; set; }

    public int? DocId { get; set; }

    public int? VersionNo { get; set; }

    public string? Sections { get; set; }
    public string? Clause { get; set; }
    public string? SubSection { get; set; }
    public string? SubClause { get; set; }

    public int? Snumber { get; set; }

    public string? AmendmentType { get; set; }

    public int? BookNo { get; set; }

    public string? Entry { get; set; }

    public string? AmendedContent { get; set; }

    public int? UpdatedBy { get; set; }

    public byte[]? UpdatedTime { get; set; }
}