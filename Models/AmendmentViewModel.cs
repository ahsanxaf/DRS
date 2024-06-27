namespace DRS.Models
{
    public class AmendmentViewModel
    {
        public Amendment NewAmendment { get; set; }
        public IEnumerable<Amendment> AmendmentsHistory { get; set; }
    }

}
