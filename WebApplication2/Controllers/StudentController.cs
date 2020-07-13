using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    public class StudentController : Controller
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IHostEnvironment _hostEnvironment;

        public StudentController(IStudentRepository studentRepository,IHostEnvironment hostEnvironment)
        {
            _studentRepository = studentRepository;
            _hostEnvironment = hostEnvironment;
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
        public IActionResult Create(StudentViewModel studentViewModel)
        {
            string newFileName = "";
            if(studentViewModel.Photo != null)
            {
                string uploadFolder = Path.Combine(_hostEnvironment.ContentRootPath, "wwwroot\\images");
                newFileName = Guid.NewGuid().ToString() + "_" + studentViewModel.Photo.FileName;
                string filePath = Path.Combine(uploadFolder,newFileName);
                studentViewModel.Photo.CopyTo(new FileStream(filePath,FileMode.Create));

            }
            var student = new Student
            {
                Name = studentViewModel.Name,
                Course = studentViewModel.Course,
                Subject = studentViewModel.Subject,
                Photo = newFileName
            };

            _studentRepository.Add(student);
            return RedirectToAction("Details", new { id = student.Id });
         }

        public IActionResult Edit(int id)
        {
            var stdData = _studentRepository.GetStudent(id);
            return View(stdData);
        }

        [HttpPost]
        public IActionResult Edit(Student student)
        {
            var stdData = _studentRepository.Update(student);
            return RedirectToAction("Details", new { id = student.Id });
        }

       
        public IActionResult Delete(int id)
        {
            var stdData = _studentRepository.Delete(id);
            return RedirectToAction("Index");
        }

    }
}