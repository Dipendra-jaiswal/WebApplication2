using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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


        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            objStudent = new StudentClass();
        }

       
        public IActionResult Index()
        {
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
            return View(); //"hello privacy " + id;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
