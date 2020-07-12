using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    public class StudentController : Controller
    {
        private readonly IStudentRepository _studentRepository;

        public StudentController(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }


        public IActionResult Index()
        {
            var stuList = _studentRepository.GetAllStudent();
            return View(stuList);
        }

        public IActionResult Details(int id)
        {
            var stdData = _studentRepository.GetStudent(id);
            return View(stdData);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Student student)
        {
            //var studentNew = new Student {
            //    Name = student.Name,
            //    Course = student.Course,
            //    Subject = student.Subject
            //};

            _studentRepository.Add(student);
            return RedirectToAction("Details", new { id = student.Id });
         }

    }
}