namespace DRS.Models
{
    public class UserLogViewModel
    {
        public int LogsId { get; set; }
        public string UserName { get; set; }
        public string LoginStatus { get; set; }
        public string UserIp { get; set; }
        public string UserBrowser { get; set; }
        public DateTime? LogInTime { get; set; }
        public DateTime? LogOutTime { get; set; }
    }
}
