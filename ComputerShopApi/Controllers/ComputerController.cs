using ComputerShopApi.Models;
using ComputerShopApi.Models.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Diagnostics.Eventing.Reader;

namespace ComputerShopApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComputerController : ControllerBase
    {
        public ComputerShopDbContext _context = new();
        [HttpPost("addComputer")]

        public object AddComputer([FromBody]AddComputerDto addComputerDto)
        {
            var computer = new Computer
            {
                Id = Guid.NewGuid(),
                Brand = addComputerDto.Brand,
                Type = addComputerDto.Type,
                Display = addComputerDto.Display,
                Memory = addComputerDto.Memory,
                CreatedTime = DateTime.Now,
                OsId = addComputerDto.OsId,
            };
            if (computer != null)
            {
                _context.Computers.Add(computer);
                _context.SaveChanges();

                return StatusCode(201, new { message = "Sikeres felvétel", result = computer });
            }
            return StatusCode(400, new { message = "Sikertelen felvétel", result = computer });
        }
        [HttpGet("allComputer")]
        public object GetAllComputers()
        {
            var computers = _context.Computers.ToList();
            Exception e = new();

            if (computers.Count > 0)
            {
                return StatusCode(200, new { message = "Sikeres lekérdezés", result = computers });
            }
            else if(computers.Count < 1)
            {
                return StatusCode(200, new { message = "Üres a tábla", result = computers });
            }
            else
            {
                return StatusCode(400, new { message = "Sikeres felvétel", result = e.Message });
            }
        }
        [HttpPut("updateComputer")]

        public object UpdateComputer([FromQuery] Guid id, [FromBody] UpdateComputerDto updateComputerDto)
        {
            var computer = _context.Computers.FirstOrDefault(os => os.Id == id);
            Exception e = new();

            if (computer != null)
            {
                computer.Brand = updateComputerDto.Brand;
                computer.Type = updateComputerDto.Type;
                computer.Display = updateComputerDto.Display;
                computer.Memory = updateComputerDto.Memory;
                computer.OsId = updateComputerDto.OsId;

                _context.Computers.Update(computer);
                _context.SaveChanges();

                return StatusCode(200, new { message = "Sikeres frissítés", result = computer });
            }
            else if (computer == null)
            {
                return StatusCode(400, new { message = "Nincs ilyen ID", result = computer });
            }
            else
            {
                return StatusCode(200, new { message = "Hiba a lekérdezésben", result = e.Message });
            }
        }

        [HttpDelete("deleteComputer")]
        public object DeleteComputer([FromQuery] Guid id)
        {
            var computer = _context.Computers.SingleOrDefault(os => os.Id == id);
            Exception e = new();

            if (computer != null)
            {

                _context.Computers.Remove(computer);
                _context.SaveChanges();

                return StatusCode(200, new { message = "Sikeres törlés", result = computer });
            }
            else if (computer == null)
            {
                return StatusCode(400, new { message = "Nincs ilyen ID", result = computer });
            }
            else
            {
                return StatusCode(200, new { message = "Hiba a lekérdezésben", result = e.Message });
            }
        }
        [HttpGet("allComputerWithOsystem")]
        public object AllComputerWithOsystem()
        {
            var computers = _context.Computers
                .Join(_context.Osystems,
                c => c.OsId,
                o => o.Id,
                (c,o) => new {Márka = c.Brand, Típus = c.Type, OsNév = o.Name, OsVerzió = o.Version}
                );



            return StatusCode(200, new { message = "Sikeres lekérdezés", result = computers });
        }

        [HttpGet("oneSystemWithComputers")]

        public object OneSystemWithComputers([FromQuery] Guid id)
        {
            var osystem = _context.Osystems.SingleOrDefault(o => o.Id == id);
            var os = _context.Osystems
               .Join(_context.Computers,
               o => o.Id,
               c => c.OsId,
               (o, c) => new { o.Name,o.Version,c.Brand,c.Type}
               );
 
            if (osystem != null)
            {
                var computers = _context.Computers.Where(c => c.OsId == osystem.Id);
                return StatusCode(200, new { message = "Sikeres lekérdezés", result = new { osystem.Name, computers} });
            }
            return StatusCode(404, new { message = "Nincs ilyen Id-vel osystem", result = computers } });
        }
    }
}
