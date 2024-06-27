using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using DRS.Models;
using DRS.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DRS.Controllers
{
    public class AccountController : Controller
    {
        private readonly DRSdbContext _context;
        private readonly EmailService _emailService;

        public AccountController(DRSdbContext context, EmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
                if (user == null) return View("ForgotPasswordConfirmation");

                var token = GenerateToken();
                var expiryDate = DateTime.Now.AddHours(1);

                var passwordResetToken = new PasswordResetToken
                {
                    Token = token,
                    UserId = user.UserId.ToString(),
                    ExpiryDate = expiryDate
                };

                await _context.PasswordResetTokens.AddAsync(passwordResetToken);
                await _context.SaveChangesAsync();

                var resetLink = Url.Action("ResetPassword", "Account", new { token, email = model.Email }, Request.Scheme);
                var emailBody = $"Please reset your password by clicking <a href='{resetLink}'>here</a> " +
                    $"or copy and paste the below link in the browser <br> {resetLink} <br> " +
                    $"Please ignore this email if you had not requested to create a new password.<br>Best of luck!<br>Regards";
                await _emailService.SendEmailAsync(model.Email, "DRS - Password Reset", emailBody);

                return View("ForgotPasswordConfirmation");
            }
            catch(Exception ex)
            {
                Console.WriteLine($"An error occurred while processing the forgot password request: {ex.Message}");
                ViewData["ValidateMessage"] = "An error occurred while processing the forgot password request: " + ex.Message;
                return View(model);
            }
            
        }

        [HttpGet]
        public IActionResult ResetPassword(string token, string email)
        {
            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(email))
            {
                return BadRequest("Token and email are required.");
            }

            var model = new ResetPasswordViewModel { Token = token, Email = email };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            Console.WriteLine($"Received Token: {model.Token}, Email: {model.Email}");

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
            if (user == null)
            {
                ModelState.AddModelError("", "No user associated with this email.");
                return View(model);
            }

            var passwordResetToken = await _context.PasswordResetTokens
                .FirstOrDefaultAsync(t => t.Token == model.Token && t.UserId == user.UserId.ToString());

            if (passwordResetToken == null)
            {
                ModelState.AddModelError("", "Invalid token.");
                return View(model);
            }

            if (passwordResetToken.ExpiryDate < DateTime.Now)
            {
                ModelState.AddModelError("", "Token has expired.");
                return View(model);
            }

            if (user == null)
            {
                return View("ResetPasswordConfirmation");
            }

            user.Password = HashPassword(model.Password);
            _context.PasswordResetTokens.Remove(passwordResetToken);
            await _context.SaveChangesAsync();

            return View("ResetPasswordConfirmation");
        }


        private string GenerateToken()
        {
            var byteArray = new byte[40];
            RandomNumberGenerator.Fill(byteArray);
            return Convert.ToBase64String(byteArray);
        }

        private string HashPassword(string password)
        {
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
            return hashedPassword;
        }
    }
}
