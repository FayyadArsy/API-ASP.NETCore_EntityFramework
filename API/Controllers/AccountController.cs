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
                return Ok(new { status = HttpStatusCode.OK, message = "Kode OTP telah dikirim, silahkan cek Email, valid selama 3 menit" });
            }
            else
            {
                return NotFound(new { status = HttpStatusCode.NotFound, message = "User tidak ditemukan" });
            }
        }
        [HttpPost("Change-Password")]
        public ActionResult ChangePassword(ChangePasswordVM ChangePassword)
        {
            var result = repository.ChangePassword(ChangePassword);
            switch (result)
            {
                case 1:
                    return Ok(new { status = HttpStatusCode.OK, message = "Password Diperbaharui" });
                case -1:
                    return NotFound(new { status = HttpStatusCode.NotFound, message = "Email tidak ditemukan" });
                case -2:
                    return BadRequest(new { status = HttpStatusCode.BadRequest, message = "Password harus sama" });
                case -3:
                    return BadRequest(new { status = HttpStatusCode.BadRequest, message = "OTP Salah" });
                case -4:
                    return BadRequest(new { status = HttpStatusCode.BadRequest, message = "Kode OTP Expired" });
                case -5:
                    return Conflict(new { status = HttpStatusCode.Conflict, message = "Kode OTP sudah digunakan" });
                case -6:
                    return BadRequest(new { status = HttpStatusCode.BadRequest, message = "Password baru tidak boleh sama" });
                case -7:
                    return BadRequest(new { status = HttpStatusCode.BadRequest, message = "Password baru Harus mengandung minimal 8 karakter, minimal 1 Huruf kapital dan angka" });
                default:
                    return StatusCode((int)HttpStatusCode.InternalServerError, new { status = HttpStatusCode.InternalServerError, message = "Error tidak diketahui" });
            }
        }
    }
}

    

