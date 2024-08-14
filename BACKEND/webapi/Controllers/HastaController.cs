// Controllers/HastaController.cs

using webapi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HastaController : ControllerBase
    {
        private readonly SampleDBContext _context;

        public HastaController(SampleDBContext context)
        {
            _context = context;
        }

        [HttpGet("WithFilter")]
        public IActionResult GetHastas([FromQuery] string? firstname, [FromQuery] string? lastname)
        {
            IQueryable<Hasta> hastas = _context.Hasta;

            List<Hasta> tempList = new List<Hasta>();

            if (!string.IsNullOrEmpty(firstname))
            {
                tempList.AddRange(hastas.Where(h => h.FirstName.Contains(firstname)).ToList());
            }

            if (!string.IsNullOrEmpty(lastname))
            {
                tempList.AddRange(hastas.Where(h => h.LastName.Contains(lastname)).ToList());
            }

            if (string.IsNullOrEmpty(firstname) && string.IsNullOrEmpty(lastname))
            {
                tempList = hastas.ToList();
            }

            return Ok(tempList);
        }

        [HttpGet]
        public ActionResult<IEnumerable<Hasta>> GetHastaList()
        {
            return _context.Hasta.ToList();
        }

        [HttpGet("kullaniciAdi/{kullaniciAdi}")]
        public ActionResult<Hasta> GetHastaByUserName(string kullaniciAdi)
        {
            var hasta = _context.Hasta.FirstOrDefault(h => h.FirstName == kullaniciAdi);
            if (hasta == null)
            {
                return NotFound();
            }
            return hasta;
        }

        [HttpGet("sifre/{sifre}")]
        public ActionResult<Hasta> GetHastaByPassword(string sifre)
        {
            var hasta = _context.Hasta.FirstOrDefault(h => h.Password == sifre);
            if (hasta == null)
            {
                return NotFound();
            }
            return hasta;
        }

        [HttpGet("{id}")]
        public ActionResult<Hasta> GetHastaById(int id)
        {
            var hasta = _context.Hasta.Find(id);
            if (hasta == null)
            {
                return NotFound();
            }
            return hasta;
        }





        [HttpPost("login")]
        public IActionResult Login([FromBody] HastaRequest loginModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var hasta = _context.Hasta
                .FirstOrDefault(a => a.Email == loginModel.Email && a.Password == loginModel.Password);

            if (hasta == null)
            {
                return Ok(new { hastaInfo = (Hasta)null, success = false });
            }

            return Ok(new { hastaInfo = hasta, success = true });
        }

        [HttpPost]
        public ActionResult<Hasta> CreateHasta(Hasta hasta)
        {
            if (hasta == null)
            {
                return BadRequest();
            }
            _context.Hasta.Add(hasta);
            _context.SaveChanges();
            return Ok();
        }
    }

    public class HastaRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
