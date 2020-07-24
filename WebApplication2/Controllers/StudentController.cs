using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
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
            //throw new Exception("Exception globally");
            var stdData = _studentRepository.GetStudent(id);
            if (stdData == null)
            {
                return View("NotFoundId", id);
            }
            return View(stdData);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(StudentViewModel studentViewModel)
        {
            if (ModelState.IsValid)
            {
                var student = GetStudentObject(studentViewModel);
                _studentRepository.Add(student);
                return RedirectToAction("Details", new { id = student.Id });
            }
            return View();
         }
        
        public IActionResult Edit(int id)
        {
            var stdData = _studentRepository.GetStudent(id);
            var student = new StudentViewModel
            {
                Id = stdData.Id,
                Name = stdData.Name,
                Course = stdData.Course,
                Subject = stdData.Subject,
                ExistingPath = stdData.Photo,
            };
            return View(student);
        }

        [HttpPost]
        public IActionResult Edit(StudentViewModel studentViewModel)
        {
            var student = GetStudentObject(studentViewModel);
            var stdData = _studentRepository.Update(student);
            return RedirectToAction("Details", new { id = student.Id });
        }

        public Student GetStudentObject(StudentViewModel studentViewModel)
        {
            string newFileName = "";
            if (studentViewModel.Photo != null && studentViewModel.Photo.Count > 0)
            {
                foreach (IFormFile photoPath in studentViewModel.Photo)
                {
                    string uploadFolder = Path.Combine(_hostEnvironment.ContentRootPath, "wwwroot\\images");
                    newFileName = Guid.NewGuid().ToString() + "_" + photoPath.FileName;
                    string filePath = Path.Combine(uploadFolder, newFileName);
                    photoPath.CopyTo(new FileStream(filePath, FileMode.Create));
                }
            }
            var student = new Student
            {
                Id = studentViewModel.Id,
                Name = studentViewModel.Name,
                Course = studentViewModel.Course,
                Subject = studentViewModel.Subject,
                Photo = string.IsNullOrEmpty(newFileName) ? studentViewModel.ExistingPath : newFileName,
            };

            return student;
        }

       
        public IActionResult Delete(int id)
        {
            var stdData = _studentRepository.Delete(id);
            return RedirectToAction("Index");
        }

    }
}