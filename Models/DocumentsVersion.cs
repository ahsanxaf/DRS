using Microsoft.EntityFrameworkCore;

namespace DRS.Models
{
    [Keyless]
    public class DocumentVersion
    {
        public virtual Document? Document { get; set; }
        public virtual DocumentType? type { get; set; }

        public int DocId { get; set; }
        public string? DocTitle { get; set; }
        public string? DocumentName { get; set; }
        public int? SubcategoryId { get; set; }
        public int? VersionNo { get; set; }
        public int? VersionId { get; set; }
        public string? SubcategoryName { get; set; }
        public int? Vyear { get; set; }
        public string? Vnumber { get; set; }
        public string? DdlMinistries { get; set; }
        public string? DdlLawcategory { get; set; }
        public int DdlLawcategoryId { get; set; }
        public int DdlMinistriesId { get; set; }


    }
}