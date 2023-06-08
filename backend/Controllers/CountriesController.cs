using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CountryApi.Models;

namespace CountryApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountriesController : ControllerBase
    {
        private readonly CountryAndStateContext _context;

        public CountriesController(CountryAndStateContext context)
        {
            _context = context;
        }

        // GET: api/Country
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Country>>> GetCountries()
        {
          if (_context.Countries == null)
          {
              return NotFound();
          }
            return await _context.Countries.ToListAsync();
        }

        // GET: api/Country/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Country>> GetCountry(long id)
        {
          if (_context.Countries == null)
          {
              return NotFound();
          }
            var country = await _context.Countries.FindAsync(id);

            if (country == null)
            {
                return NotFound();
            }

            return country;
        }

        // GET: api/Country/AU/states
        [HttpGet("{countryCode}/States")]
        public async Task<ActionResult<IEnumerable<State>>> GetCountryStates(string countryCode)
        {
            if (_context.States == null)
            {
                return NotFound();
            }
            // var countries = await _context.Countries.ToListAsync();
            // long countryId = -1;
            var countryId = await
                    (from country in _context.Countries
                    where country.Code == countryCode
                    select country.Id).SingleOrDefaultAsync();

            var countryStates = 
                    from state in _context.States
                    where state.CountryId == countryId
                    select state;

            return await countryStates.ToListAsync();

            // var states = new List<State>();
            // states.Add(new State {Id=1,code="VA",Name="Virginia",countryId=1});

            // return Ok(Task.FromResult(states));
        }

        // PUT: api/Country/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCountry(long id, Country country)
        {
            if (id != country.Id)
            {
                return BadRequest();
            }

            _context.Entry(country).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CountryExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Country
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Country>> PostCountry(CreateCountryDTO country)
        {
          if (_context.Countries == null)
          {
              return Problem("Entity set 'CountryContext.Countries'  is null.");
          }

          var newCountry = new Country
          {
            Code = country.Code,
            Name = country.Name
          };
            _context.Countries.Add(newCountry);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCountry), new { id = newCountry.Id }, newCountry);
        }

        // DELETE: api/Country/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCountry(long id)
        {
            if (_context.Countries == null)
            {
                return NotFound();
            }
            var country = await _context.Countries.FindAsync(id);
            if (country == null)
            {
                return NotFound();
            }

            _context.Countries.Remove(country);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CountryExists(long id)
        {
            return (_context.Countries?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
