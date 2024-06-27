using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DRS.Models;

public partial class UserLog
{
    public int LogsId { get; set; }

    public int? UserId { get; set; }
    [StringLength(50)]
    public string? LoginStatus { get; set; }
    [StringLength(50)]
    public string? UserIp { get; set; }

    [StringLength(256)]
    public string? UserBrowser { get; set; }

    public DateTime? LogInTime { get; set; }

    public DateTime? LogOutTime { get; set; }

    public User user { get; set; }
}
