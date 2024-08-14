// Controllers/CustomerController.cs

using webapi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly SampleDBContext _context;
        public AdminController(SampleDBContext context)
        {
            _context = context;
        }

        // GET: api/Customer
        [HttpGet]
        public ActionResult<IEnumerable<Admin>> GetList()
        {
            return _context.Admin.ToList();
        }

        

        // GET: api/Customer/1
        [HttpGet("kullaniciAdi/{kullaniciAdi}")]
        public ActionResult<Admin> Getkullaniciadi(string kullaniciAdi)
        {
            var admin = _context.Admin.Where((hast) => hast.FirstName == kullaniciAdi).FirstOrDefault();
            if (admin == null)
            {
                return NotFound();
            }
            return admin;
        }

        [HttpGet("sifre/{sifre}")]
        public ActionResult<Admin> Getsifre(string sifre)
        {
            var admin = _context.Admin.Where((hast) => hast.Password == sifre).FirstOrDefault();
            if (admin == null)
            {
                return NotFound();
            }
            return admin;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] AdminRequest loginModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var admin = _context.Admin
                .FirstOrDefault(a => a.Email == loginModel.Email && a.Password == loginModel.Password);

            if (admin == null)
            {
                return Ok(new { adminInfo = (Admin)null, success = false });
            }

            return Ok(new { adminInfo = admin, success = true });
        }


        // POST: api/Customer
        [HttpPost]
        public ActionResult<Admin> CreateAdmin(Admin admin)
        {
            if (admin == null)
            {
                return BadRequest();
            }

            _context.Admin.Add(admin);  
            _context.SaveChanges();
            return Ok();
        }
    }

    public class AdminRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}