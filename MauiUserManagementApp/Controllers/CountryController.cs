using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using MauiUserManagementApp.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MauiUserManagementApp.DatabaseCommunication;

namespace MauiUserManagementApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CountryController : Controller
    {
        private readonly UserManagementContext _context;

        public CountryController(UserManagementContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Country>>> GetCountries()
        {
            return await _context.Countries.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Country>> GetCountry(int id)
        {
            var country = await _context.Countries.FindAsync(id);

            if (country == null)
            {
                return NotFound();
            }

            return country;
        }

        [HttpPost]
        public async Task<ActionResult<Country>> PostCountry(Country country)
        {
            if (ModelState.IsValid)
            {
                _context.Countries.Add(country);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetCountries), new { id = country.Id }, country);
            }

            return BadRequest(ModelState);
        }
    }
}
