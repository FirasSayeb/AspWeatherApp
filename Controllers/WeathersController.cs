using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WeaterApp.Data;
using WeaterApp.Models;

namespace WeaterApp.Controllers
{
    public class WeathersController : Controller
    {
        private readonly WeaterAppContext _context;

        public WeathersController(WeaterAppContext context)
        {
            _context = context;
        }

        // GET: Weathers
        public async Task<IActionResult> Index()
        {
              return _context.Weather != null ? 
                          View(await _context.Weather.ToListAsync()) :
                          Problem("Entity set 'WeaterAppContext.Weather'  is null.");
        }

        // GET: Weathers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Weather == null)
            {
                return NotFound();
            }

            var weather = await _context.Weather
                .FirstOrDefaultAsync(m => m.Id == id);
            if (weather == null)
            {
                return NotFound();
            }

            return View(weather);
        }

        // GET: Weathers/Create
        /* public IActionResult Create()
         {
             return View();
         }*/

        // POST: Weathers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        /* [HttpPost]
         [ValidateAntiForgeryToken]
         public async Task<IActionResult> Create([Bind("Id,City,Temperature,WindSpeed,Humidity")] Weather weather)
         {
             if (ModelState.IsValid)
             {
                 _context.Add(weather);
                 await _context.SaveChangesAsync();
                 return RedirectToAction(nameof(Index));
             }
             return View(weather);
         }
        */
        [AllowAnonymous]
        [IgnoreAntiforgeryToken]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] WeatherData weatherData)
        {
            if (weatherData == null || weatherData.temperature == null)
            {
                return BadRequest("Invalid weather data.");
            }

            Console.WriteLine($"City: {weatherData.city}");
            Console.WriteLine($"Temperature: {string.Join(", ", weatherData.temperature)}");
            Console.WriteLine($"WindSpeed: {string.Join(", ", weatherData.windSpeed)}");
            Console.WriteLine($"Humidity: {string.Join(", ", weatherData.humidity)}");

            var weatherEntries = new List<Weather>();

            for (int i = 0; i < weatherData.temperature.Length; i++)
            {
                weatherEntries.Add(new Weather
                {
                    City = weatherData.city,
                    Temperature = weatherData.temperature[i],
                    WindSpeed = weatherData.windSpeed[i],
                    Humidity = weatherData.humidity[i],  
                });
            }

            _context.Weather.AddRange(weatherEntries);
            await _context.SaveChangesAsync();

            return Ok("Weather data saved successfully.");     
        }
            

        // GET: Weathers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Weather == null)
            {
                return NotFound();
            }

            var weather = await _context.Weather.FindAsync(id);
            if (weather == null)
            {
                return NotFound();
            }
            return View(weather);
        }

        // POST: Weathers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,City,Temperature,WindSpeed,Humidity")] Weather weather)
        {
            if (id != weather.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(weather);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WeatherExists(weather.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(weather);
        }

        // GET: Weathers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Weather == null)
            {
                return NotFound();
            }

            var weather = await _context.Weather
                .FirstOrDefaultAsync(m => m.Id == id);
            if (weather == null)
            {
                return NotFound();
            }

            return View(weather);
        }

        // POST: Weathers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Weather == null)
            {
                return Problem("Entity set 'WeaterAppContext.Weather'  is null.");
            }
            var weather = await _context.Weather.FindAsync(id);
            if (weather != null) 
            {
                _context.Weather.Remove(weather);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WeatherExists(int id)
        {
          return (_context.Weather?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
