using API.Repository;
using API.ViewModel;
using Azure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly AccountRepository repository;
        public AccountController(AccountRepository repository)
        {
            this.repository = repository;
        }

        [HttpGet("Email")]
        public async Task<IActionResult> Index()
        {
            var receiver = "yerol89830@hapincy.com";
            var subject = "Reset Password";
            var body = "Hello World";

            try
            {
                await repository.SendEmailAsync(receiver, subject, body);
                return Ok("Email sent successfully."); // Mengembalikan OK dengan pesan sukses.
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to send email: {ex.Message}"); // Mengembalikan BadRequest dengan pesan kesalahan jika terjadi masalah.
            }
        }
  

            [HttpGet("Login")]
            public ActionResult Login(string Email, string Password)
            {
                if (Email == null || Password == null)
                {
                    return BadRequest(new { status = HttpStatusCode.BadRequest, message = "Email atau password tidak boleh kosong." });
                }

                var result = repository.Login(Email, Password);

                if (result == false)
                {
                    return BadRequest(new { status = HttpStatusCode.BadRequest, message = "Email atau Password salah." });
                }

                return Ok(new { status = HttpStatusCode.OK, message = "Login berhasil." });
            }

        [HttpPost("ForgetPassword")]
        public ActionResult ForgetPassword(string Email)
        {
            bool success = repository.ForgetPassword(Email);
            if (success)
            {
                return Ok("Password reset email sent successfully.");
            }
            else
            {
                return NotFound("User not found.");
            }
        }
        [HttpPost("Change-Password")]
        public ActionResult ChangePassword(ChangePasswordVM ChangePassword)
        {
            var result = repository.ChangePassword(ChangePassword);
            switch (result)
            {
                case 1:
                    return Ok("Password Diperbaharui");
                case -1:
                    return BadRequest("Email tidak ditemukan");
                default:
                    return BadRequest("Error tidak diketahui");
            }
        }
    }
}

    

