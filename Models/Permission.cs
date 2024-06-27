using System;
using System.Collections.Generic;

namespace DRS.Models;

public partial class Permission
{
    public int PermissionId { get; set; }

    public int RoleId { get; set; }

    public bool? UserModule { get; set; }

    public bool? DocumentCategoryModule { get; set; }

    public bool? LoginAuditModule { get; set; }

    public bool? DocumentModule { get; set; }

    public bool? SearchModule { get; set; }

    public bool? BackupModule { get; set; }

    public bool? AddPermission { get; set; }

    public bool? DeletePermission { get; set; }

    public bool? ViewPermission { get; set; }

    public virtual Role Role { get; set; } = null!;
}
