using API.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        [HttpGet]
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
    }
}
