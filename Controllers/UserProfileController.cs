using DRS.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace DRS.Controllers
{
    [Authorize]
    public class UserProfileController : Controller
    {
        private readonly DRSdbContext dbContext;

        public UserProfileController(DRSdbContext dbContext)
        {
            this.dbContext = dbContext;
        }


        [HttpGet]
        public IActionResult EditProfile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = dbContext.Users.FirstOrDefault(u => u.UserId.ToString() == userId);

            if (user == null)
            {
                return NotFound();
            }

            ViewBag.getRoles = dbContext.Roles.ToList();
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> EditProfile(User userModel)
        {
            try
            {
                ViewBag.getRoles = dbContext.Roles.ToList();

                ModelState.Remove("Password");
                ModelState.Remove("RoleId");
                ModelState.Remove("AccountStatus");

                if (ModelState.IsValid)
                {
                    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    var user = dbContext.Users.FirstOrDefault(u => u.UserId.ToString() == userId);

                    if (user == null)
                    {
                        return Json(new { success = false, message = "User not Found!" });
                    }

                    if (dbContext.Users.Any(u => u.Email == userModel.Email && u.UserId != user.UserId))
                    {
                        return Json(new { success = false, message = "User with this Email already exists!" });
                    }

                    if (dbContext.Users.Any(u => u.MemberId == userModel.MemberId && u.UserId != user.UserId))
                    {
                        return Json(new { success = false, message = "MemberId is already taken!" });
                    }

                    user.Name = userModel.Name;
                    user.Email = userModel.Email;
                    user.Dob = userModel.Dob;
                    user.ContactNo = userModel.ContactNo;
                    user.City = userModel.City;
                    user.State = userModel.State;
                    user.PinCode = userModel.PinCode;

                    dbContext.Users.Update(user);
                    await dbContext.SaveChangesAsync();

                    await UpdateUserClaims(user);

                    return Json(new { success = true, message = "Profile Updated successfully" });
                }
                else
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors)
                                          .Select(e => e.ErrorMessage)
                                          .ToList();
                    return Json(new { success = false, message = "Validation failed!", errors });
                }
            }
            catch (Exception ex)
            {
                TempData["Msg"] = "Failed to update, " + ex.Message;
                return Json(new { success = false, message = "An error occurred while updating the profile" });
            }
        }

        public async Task UpdateUserClaims(User user)
        {
            var claimsIdentity = new ClaimsIdentity(User.Identity);

            var nameClaim = claimsIdentity.FindFirst(ClaimTypes.Name);
            if (nameClaim != null) claimsIdentity.RemoveClaim(nameClaim);

            var emailClaim = claimsIdentity.FindFirst(ClaimTypes.Email);
            if (emailClaim != null) claimsIdentity.RemoveClaim(emailClaim);

            claimsIdentity.AddClaim(new Claim(ClaimTypes.Name, user.Name));
            claimsIdentity.AddClaim(new Claim(ClaimTypes.Email, user.Email));

            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            await HttpContext.SignInAsync(claimsPrincipal);
        }

    }
}