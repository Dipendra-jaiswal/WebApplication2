using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IList<Student> students = new List<Student>();
        private readonly StudentClass objStudent;

        private readonly string Name = "Name";
        private readonly string age = "Age";


        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            objStudent = new StudentClass();
        }

       
        public IActionResult Index()
        {
            HttpContext.Session.SetString(Name, "Ram");
            HttpContext.Session.SetInt32(age,32);
           
            HttpContext.Session.Set("Complex", Encoding.ASCII.GetBytes("hello"));

            var stuList = students;
           // var ddd = students.Where(p => p.Id == 2).First();
           // ddd.Name = "Naresh";
           // students[2].Name = "Nirjesh";
           // foreach (var item in students)
           // {
            //    if(item.Id == 2)
            //    {
            //        item.Name = "Naresh";
            //    }
            //}            
            
            ViewData["Hello"] = "Hello World";
            ViewBag.Hello1 = "use view bag";
            return View(stuList);
        }

        public IActionResult Details(int id)
        {
            var stdData = students.Where(p => p.Id == id).First();
            return View(stdData);
        }



        public IActionResult Privacy(int id)
        {
            ViewBag.Name = HttpContext.Session.GetString(Name);
            ViewBag.Age = HttpContext.Session.GetInt32(age);
            ViewBag.Complex = ASCIIEncoding.ASCII.GetString(HttpContext.Session.Get("Complex"));
            return View(); //"hello privacy " + id;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
