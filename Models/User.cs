using System;
using System.Collections.Generic;


namespace DRS.Models;

public partial class User
{
    public int UserId { get; set; }

    public int? RoleId { get; set; }

    public string Name { get; set; } = null!;

    public DateOnly? Dob { get; set; }

    public string? ContactNo { get; set; }

    public string Email { get; set; } = null!;

    public string? State { get; set; }

    public string? City { get; set; }

    public string? PinCode { get; set; }

    public string? UserAddress { get; set; }

    public string MemberId { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? AccountStatus { get; set; }

    public virtual Role? Role { get; set; }

    public virtual ICollection<Log> Logs { get; set; }
}
