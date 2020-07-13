using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication2.Models
{
    public class MockStudentRepository : IStudentRepository
    {
        private static List<Student> _studentList;
        private static int count = 0;
        public MockStudentRepository()
        {
            if (count == 0)
            {
                _studentList = new List<Student> {
                    new Student{ Id = 1, Name ="Ramesh",Course="MCA",Subject="CS", Photo="Dan.jpg" },
                    new Student{ Id = 2, Name ="Suresh",Course="BTECH",Subject="IT", Photo="sam.jpg" },
                    new Student{ Id = 3, Name ="Rajesh",Course="MCA",Subject="CS", Photo="tan.jpg" }
                };
                count = 1;
            }
        }

        public Student Add(Student student)
        {
            student.Id = _studentList.Max(p => p.Id) + 1;
            _studentList.Add(student);
            return student;
        }

        public Student Delete(int id)
        {
            var studentDelete = _studentList.FirstOrDefault(p => p.Id == id);
            if (studentDelete != null)
            {
                _studentList.Remove(studentDelete);
            }
            return studentDelete;
        }

        public IEnumerable<Student> GetAllStudent()
        {
            return _studentList;
        }

        public Student GetStudent(int id)
        {
            return _studentList.FirstOrDefault(p => p.Id == id);
        }

        public Student Update(Student student)
        {
            var studentUpdate = _studentList.FirstOrDefault(p => p.Id == student.Id);
            if(studentUpdate != null)
            {
                studentUpdate.Name = student.Name;
                studentUpdate.Course = student.Course;
                studentUpdate.Subject = student.Subject;
                studentUpdate.Photo = student.Photo;
            }
            return studentUpdate;
        }
    }
}
