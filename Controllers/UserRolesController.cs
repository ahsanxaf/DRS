using DRS.Extensions;
using DRS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using BCrypt.Net;

namespace DRS.Controllers
{
    [Authorize]
    public class UsersRolesController : Controller
    {
        private readonly DRSdbContext dbContext;

        public UsersRolesController(DRSdbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        [CustomAuthorize("AddPermission", "UserModule")]
        public IActionResult AddUser()
        {
            ViewBag.getRoles = dbContext.Roles.ToList();

            return View();
        }

        [HttpPost]
        [CustomAuthorize("AddPermission", "UserModule")]
        public async Task<IActionResult> AddUser(User userModel)
{
    try
    {
        ViewBag.getRoles = dbContext.Roles.ToList();

        if (ModelState.IsValid)
        {
            if (dbContext.Users.Any(u => u.Email == userModel.Email))
            {
                return Json(new { success = false, message = "User with this Email already exists!" });
            }

            if (dbContext.Users.Any(u => u.MemberId == userModel.MemberId))
            {
                return Json(new { success = false, message = "User with this Member Id already exists!" });
            }

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(userModel.Password);

            var user = new User
            {
                RoleId = userModel.RoleId,
                Name = userModel.Name,
                Email = userModel.Email,
                Dob = userModel.Dob,
                ContactNo = userModel.ContactNo,
                Role = userModel.Role,
                City = userModel.City,
                State = userModel.State,
                PinCode = userModel.PinCode,
                MemberId = userModel.MemberId,
                Password = hashedPassword,
            };

            await dbContext.Users.AddAsync(user);
            await dbContext.SaveChangesAsync();

            return Json(new { success = true, message = "User Added successfully" });
        }
        else
        {
            return Json(new { success = false, message = "Validations Failed! Please Fill all the Mandatory Details" });
        }
    }
    catch (Exception ex)
    {
        TempData["Msg"] = "Failed to add, " + ex.Message;
        return Json(new { success = false, message = "An error occurred while adding the user" });
    }

}

        [HttpGet]
        [CustomAuthorize("UserModule", "ViewPermission")]
        public async Task<IActionResult> Users(int pg = 1)
        {
            var users = await dbContext.Users.Include(u => u.Role).ToListAsync();

            foreach (var user in users)
            {
                Console.WriteLine($"User: {user.Name}, RoleId: {user.RoleId}, Role: {user.Role?.RoleName}");
            }

            const int pageSize = 10;
            if (pg < 1)
            {
                pg = 1;
            }

            int recsCount = users.Count();

            var pager = new Pager(recsCount, pg, pageSize);

            int recskip = (pg - 1) * pageSize;

            var data = users.Skip(recskip).Take(pager.PageSize).ToList();

            ViewBag.Pager = pager;

            return View(data);
        }

        [HttpGet]
        [CustomAuthorize("AddPermission", "UserModule")]
        public async Task<IActionResult> EditUser(int userId)
        {
            ViewBag.UserId = userId;
            var user = await dbContext.Users.FindAsync(userId);
            ViewBag.Roles = await dbContext.Roles.ToListAsync();
            return View(user);
        }

        [HttpPost]
        [CustomAuthorize("AddPermission", "UserModule")]
        public async Task<IActionResult> EditUser(User viewModel)
        {
            var user = await dbContext.Users.FindAsync(viewModel.UserId);

            if (user is not null)
            {
                user.Name = viewModel.Name;
                user.Password = viewModel.Password;
                user.MemberId = viewModel.MemberId;
                user.Email = viewModel.Email;
                user.ContactNo = viewModel.ContactNo;
                user.RoleId = viewModel.RoleId;
                user.AccountStatus = viewModel.AccountStatus;

                await dbContext.SaveChangesAsync();
            }

            return RedirectToAction("Users", "UsersRoles");
        }

        [HttpPost]
        [CustomAuthorize("DeletePermission")]
        public async Task<IActionResult> Delete(User viewModel)
        {
            var user = await dbContext.Users.AsNoTracking().FirstOrDefaultAsync(x => x.UserId == viewModel.UserId);

            if (user != null)
            {
                dbContext.Users.Remove(user);
                await dbContext.SaveChangesAsync();
            }

            return RedirectToAction("Users", "UsersRoles");
        }

        [HttpGet]
        [CustomAuthorize("AddPermission", "UserModule")]
        public async Task<IActionResult> Roles()
        {
            var roles = await dbContext.Roles
             .Include(r => r.Permissions)
             .ToListAsync();

            foreach (var role in roles)
            {
                if (role.Permissions == null)
                {
                    role.Permissions = new List<Permission>();
                }
            }

            var commonModel = new CommonModel
            {
                roleslist = roles,
            };
            return View(commonModel);
        }

        [HttpPost]
        [CustomAuthorize("AddPermission", "UserModule")]
        public async Task<IActionResult> Roles(CommonModel model, string userModule, string documentCategoryModule, string loginAuditModule,
            string documentModule, string searchModule, string backupModule, string addPermissions,
            string viewPermissions, string deletePermissions)
        {
            try
            {
                var existingRole = await dbContext.Roles.FirstOrDefaultAsync(r => r.RoleName == model.Roles.RoleName);

                if (existingRole != null)
                {
                    /*ModelState.AddModelError("Roles.RoleName", "Role with this name already exists.");
                    TempData["RoleExistsError"] = "Role with this name already exists.";
                    ModelState.Clear();
                    model.Roles.RoleName = "";*/
                    /*return View();*/
                    return Json(new { success = false, message = "Role with this name already exists." });
                }

                var role = new Role
                {

                    RoleName = model.Roles.RoleName,
                };

                await dbContext.Roles.AddAsync(role);
                await dbContext.SaveChangesAsync();

                var permissions = new Permission
                {
                    RoleId = role.RoleId,
                    UserModule = null,
                    DocumentCategoryModule = null,
                    LoginAuditModule = null,
                    DocumentModule = null,
                    SearchModule = null,
                    BackupModule = null,
                    DeletePermission = null,
                    ViewPermission = null,
                    AddPermission = null
                };

                permissions.UserModule = (userModule == "true") ? true : false;
                permissions.DocumentCategoryModule = (documentCategoryModule == "true") ? true : false;
                permissions.LoginAuditModule = (loginAuditModule == "true") ? true : false;
                permissions.DocumentModule = (documentModule == "true") ? true : false;
                permissions.SearchModule = (searchModule == "true") ? true : false;
                permissions.BackupModule = (backupModule == "true") ? true : false;
                permissions.DeletePermission = (deletePermissions == "true") ? true : false;
                permissions.ViewPermission = (viewPermissions == "true") ? true : false;
                permissions.AddPermission = (addPermissions == "true") ? true : false;

                await dbContext.Permissions.AddAsync(permissions);

                await dbContext.SaveChangesAsync();
                ModelState.Clear();
                TempData["Msg"] = "Role Added Successfully";

                return Json(new { success = true, message = "Role and Permissions added successfully" });
            }
            catch (Exception ex)
            {
                TempData["Msg"] = "Failed to add, " + ex.Message;
                /*Console.WriteLine($"Error: {ex.Message}");
                ModelState.AddModelError(string.Empty, "An error occurred while adding the role.");*/
                /*return View(model);*/
                return Json(new { success = false, message = "An error occurred while adding the role." });
            }

            
        }

        [HttpGet]
        [CustomAuthorize("UserModule")]
        public async Task<IActionResult> GetRolesTable(int id)
        {
            var roles = await dbContext.Roles
             .Include(r => r.Permissions)
             .ToListAsync();

            var getRoles = new CommonModel
            {
                roleslist = roles,
            };

            return PartialView("_RolesTable", getRoles);
        }

        [HttpPost]
        public async Task<IActionResult> UpdatePermissions(int roleId, bool userModule, bool documentCategoryModule, bool loginAuditModule, bool documentModule, bool searchModule, bool backupModule, bool addPermissions, bool viewPermissions, bool deletePermissions)
        {
            var role = await dbContext.Roles
                .Include(r => r.Permissions)
                .FirstOrDefaultAsync(r => r.RoleId == roleId);

            if (role == null)
            {
                return NotFound();
            }

            var permissions = role.Permissions.FirstOrDefault();
            if (permissions == null)
            {
                permissions = new Permission
                {
                    RoleId = roleId,
                    UserModule = userModule,
                    DocumentCategoryModule = documentCategoryModule,
                    LoginAuditModule = loginAuditModule,
                    DocumentModule = documentModule,
                    SearchModule = searchModule,
                    BackupModule = backupModule,
                    AddPermission = addPermissions,
                    ViewPermission = viewPermissions,
                    DeletePermission = deletePermissions
                };
                dbContext.Permissions.Add(permissions);
            }
            else
            {
                permissions.UserModule = userModule;
                permissions.DocumentCategoryModule = documentCategoryModule;
                permissions.LoginAuditModule = loginAuditModule;
                permissions.DocumentModule = documentModule;
                permissions.SearchModule = searchModule;
                permissions.BackupModule = backupModule;
                permissions.AddPermission = addPermissions;
                permissions.ViewPermission = viewPermissions;
                permissions.DeletePermission = deletePermissions;
            }

            await dbContext.SaveChangesAsync();

            var usersWithRole = await dbContext.Users
                                               .Include(u => u.Role)
                                               .ThenInclude(r => r.Permissions)
                                               .Where(u => u.RoleId == roleId)
                                               .ToListAsync();

            foreach (var user in usersWithRole)
            {
                var userPermissions = user?.Role?.Permissions?.ToList();
                HttpContext.Session.Set($"Permissions_{user?.Email}", userPermissions);
            }

            var currentLoggedInEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

            foreach (var user in usersWithRole)
            {
                if (user.Email != currentLoggedInEmail)
                {
                    var userPermissions = user?.Role?.Permissions.ToList();
                    HttpContext.Session.Set($"Permissions_{user?.Email}", userPermissions);
                }
                   
            }

            var currentUser = await dbContext.Users
                                    .Include(u => u.Role)
                                    .ThenInclude(r => r.Permissions)
                                    .FirstOrDefaultAsync(u => u.Email == currentLoggedInEmail);

            if (currentUser != null && currentUser.RoleId == roleId)
            {
                var currentUserPermissions = currentUser?.Role?.Permissions.ToList();
                HttpContext.Session.Set("LoggedInUserPermissions", currentUserPermissions);
            }

            return RedirectToAction("Roles");
        }


        [CustomAuthorize("ViewPermission", "UserModule")]
public async Task<IActionResult> ViewUser(int userId)
{
    try
    {
        var user = await dbContext.Users.FindAsync(userId);
        if (user == null)
        {
            return NotFound();
        }
        return View(user);
    }
    catch (Exception ex)
    {
        return StatusCode(500, "Internal server error: " + ex.Message);
    }
}
    
    }

}
