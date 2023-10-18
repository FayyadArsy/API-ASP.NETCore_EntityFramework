using API.Context;
using API.Models;
using API.Repository;
using API.ViewModel;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly EmployeeRepository repository;
        public EmployeesController(EmployeeRepository repository)
        {
            this.repository = repository;
        }
        /*[EnableCors]*/
        [HttpGet("TestCors")]
        public ActionResult TestCORS()
        {
            return Ok("Test CORS Berhasil");
        }

        [HttpGet]
        public ActionResult Get()
        {
            var allData = repository.Get();
            return Ok(new { status = HttpStatusCode.OK, response = "Menampilkan Seluruh Data", allData });
        }
        [HttpGet("v2")]
        public ActionResult Getv2()
        {
            var allData = repository.Getv2();
            return Ok(new { status = HttpStatusCode.OK, response = "Menampilkan Seluruh Datav2", allData });
        }
        [HttpPost]
        public virtual ActionResult Insert(Employee employee)
        {
            try
            {
                var emailExists = repository.CheckEmail(employee.Email);
                var phoneExists = repository.CheckPhoneNumber(employee.Phone);

                if (emailExists)
                {
                    return BadRequest(new {status=HttpStatusCode.BadRequest, response = "Email sudah ada dalam database." });
                }

                if (phoneExists)
                {
                    return BadRequest(new { status = HttpStatusCode.BadRequest, response = "Nomor HP sudah ada dalam database." });
                }

                repository.Insert(employee);

                return Ok(new { status = HttpStatusCode.OK, response = "Data Berhasil Ditambah" });
            }
            catch (Exception e)
            {
                return BadRequest(new { status = HttpStatusCode.BadRequest, response = "Gagal Menambah Data", ErrorMessage = e.Message });
            }
        }
        [HttpGet("{NIK}")]
        public virtual ActionResult Get(string NIK)
        {
            
            var get = repository.Get( NIK);
            if (get==null)
            {
                /*return StatusCode(StatusCodes.Status404NotFound, "Error message");*/
                return NotFound(new { response = "Data Tidak Ditemukan", get});
            }
            else
            return Ok(new{ status = HttpStatusCode.OK, response = "Data Ditemukan", get });
        }
        [HttpPut("update/{NIK}")]
        public virtual ActionResult Update(Employee employee)
        {

            try
            {
                var emailExists = repository.CheckEmail(employee.Email);
                var phoneExists = repository.CheckPhoneNumber(employee.Phone);

                if (emailExists)
                {
                    return BadRequest(new { status = HttpStatusCode.BadRequest, response = "Email sudah ada dalam database." });
                }

                if (phoneExists)
                {
                    return BadRequest(new { status = HttpStatusCode.BadRequest, response = "Nomor HP sudah ada dalam database." });
                }

                var updateResult = repository.Update(employee);

                if (updateResult == null)
                {
                    return NotFound(new { status = HttpStatusCode.NotFound, response = "Data Tidak Ditemukan" });
                }

                return Ok(new { status = HttpStatusCode.OK, response = "Data Berhasil Diubah" });
            }
            catch (Exception e)
            {
                return BadRequest(new { response = "Gagal", ErrorMessage = e.Message });
            }
        }

        [HttpPost("register")]
        public virtual ActionResult Register(RegisterVM register)
        {
            if (repository.CheckEmail(register.Email) == true)
            {
                return BadRequest(new { status = HttpStatusCode.BadRequest, message = "Email tidak boleh duplikat." });
            }
            else if (repository.CheckPhoneNumber(register.Phone) == true)
            {
                return BadRequest(new { status = HttpStatusCode.BadRequest, message = "Phone tidak boleh duplikat." });
            }
           

            repository.Register(register);
            return Ok(new { status = HttpStatusCode.OK, message = "Data berhasil ditambahkan." });
        }

        [HttpDelete("/delete/{NIK}")]
        public virtual ActionResult Delete(string NIK)
        {
         
            try
            {
                var get = repository.Delete(NIK);
                return Ok(new { status = HttpStatusCode.OK, response = "Data Berhasil Dihapus" });
            }
            catch (Exception e)
            {
                    return NotFound(new { status = HttpStatusCode.NotFound, response = "Data Tidak Ditemukan", e.Message });
                }
        }
    }
    
}
