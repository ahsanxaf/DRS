using System.ComponentModel.DataAnnotations;

namespace DRS.Models
{
    public class DocumentType
    {
        [Key]
   
        public int DocumentTypeID { get; set; }
        public string? DocTitle { get; set; }

        // Foreign key properties
        public int? DocId { get; set; }
        public int? SubcategoryId { get; set; }
   

        // Additional properties for relating Acts, Rules, and Ordinance
        public int? Acts { get; set; }
        public int? Rules { get; set; }
        public int? Ordinance{ get; set; }



 
    }
}
