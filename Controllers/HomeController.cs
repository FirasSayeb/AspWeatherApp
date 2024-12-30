using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol;
using System.Diagnostics;
using WeaterApp.Data;
using WeaterApp.Models;

namespace WeaterApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly WeaterAppContext _context;

        public HomeController(ILogger<HomeController> logger, WeaterAppContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult History()
        {
            return View();
        }

        public IActionResult Predict()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [IgnoreAntiforgeryToken]
        public IActionResult Predict(string city,string time)
        {


            
             
                string t=time.Substring(11);            

            var sampleData = new PredictWeather.ModelInput()
            {

                City = city,   
                Time = t,
                WindSpeed = (float) _context.Weather.Where(w => w.City.Contains(city)).Average(w=>w.WindSpeed),
                Humidity= (float)_context.Weather.Where(w => w.City.Contains(city)).Average(w => w.Humidity)  
            };
             
            //Load model and predict output         
            var result = PredictWeather.Predict(sampleData);       
                            
               
                        
               
            return RedirectToAction("Result", new { temperateur = result.Score,city=city,time=time});  
        }

        public IActionResult Result(double temperateur, string city,string time)
        {
            Result res=new Result();
            res.city=city;
            res.time=time;    
            res.temperateur = temperateur;
            return View(res);
        }
        public IActionResult Save()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}