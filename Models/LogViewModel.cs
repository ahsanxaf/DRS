using System;

namespace DRS.Models
{
    public class LogViewModel
    {
        public int LogId { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Action { get; set; }
        public int DocumentId { get; set; }
        public string DocumentTitle { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
