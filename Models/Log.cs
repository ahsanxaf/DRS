using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DRS.Models;

public partial class Log
{
    public int LogId { get; set; }
    public int UserId { get; set; }
    public string? Action { get; set; }
    public int DocumentId { get; set; }
    public DateTime Timestamp { get; set; }

    [ForeignKey("UserId")]
    public virtual User User { get; set; }

    public virtual Document Document { get; set; }
}
