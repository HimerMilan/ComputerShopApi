using ComputerShopApi.Models;
using ComputerShopApi.Models.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;

namespace ComputerShopApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OsystemController : ControllerBase
    {
        public ComputerShopDbContext _context = new();

        [HttpPost("addOsystem")]

        public object AddNewOsystem([FromBody] AddNewOsystemDto addNewOsystemDto)
        {
            var osystem = new Osystem
            {
                Id = Guid.NewGuid(),
                Name = addNewOsystemDto.Name,
                Version = addNewOsystemDto.Version,
                RegisterTime = DateTime.Now,
                UpdateTime = DateTime.Now,
            };
            if (osystem != null)
            {
                _context.Osystem.Add(osystem);
                _context.SaveChanges();

                return StatusCode(201, new { message = "Sikeres felvétel", result = osystem });
            }

            return StatusCode(400, new { message = "Sikertelen felvétel", result = osystem });
        }

        [HttpGet("allOsystem")]
        public object GetAllOsystem()
        {
            var osystems = _context.Osystem.ToList();
            Exception e = new();

            if (osystems.Count > 0)
            {
                return StatusCode(200, new { message = "Sikeres lekérdezés", result = _context.Osystem.ToList() });
            }
            else if (osystems.Count < 1)
            {
                return StatusCode(200, new { message = "Üres tábla", result = osystems });
            }
            else
            {
                return StatusCode(400, new { message = "Hiba a lekérdezésben", result = e.Message });
            }

        }
        [HttpPut("updateOsystem")]
        
        public object UpdateOsystem([FromQuery]Guid id, [FromBody]UpdateOsystemDto updateOsystemDto)
        {
            var osystem = _context.Osystem.FirstOrDefault(os => os.Id == id);
            Exception e = new();

            if (osystem != null)
            {
                osystem.Name = updateOsystemDto.Name;
                osystem.Version = updateOsystemDto.Version;
                osystem.UpdateTime = DateTime.Now;

                _context.Osystem.Update(osystem);
                _context.SaveChanges();

                return StatusCode(200, new { message = "Sikeres frissítés", result = osystem });
            }
            else if(osystem == null)
            {
                return StatusCode(400, new { message = "Nincs ilyen ID", result = osystem});
            }
            else
            {
                return StatusCode(200, new { message = "Hiba a lekérdezésben", result = e.Message});
            }
        }

        [HttpDelete]
        public object DeleteOsystem([FromQuery] Guid id)
        {
            var osystem = _context.Osystem.SingleOrDefault(os => os.Id == id);
            Exception e = new();

            if (osystem != null)
            {

                _context.Osystem.Remove(osystem);
                _context.SaveChanges();

                return StatusCode(200, new { message = "Sikeres törlés", result = osystem });
            }
            else if (osystem == null)
            {
                return StatusCode(400, new { message = "Nincs ilyen ID", result = osystem });
            }
            else
            {
                return StatusCode(200, new { message = "Hiba a lekérdezésben", result = e.Message });
            }
        }
    }
}
