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
                    new Student{ Id = 1, Name ="Ramesh",Course="MCA",Subject="CS" },
                    new Student{ Id = 2, Name ="Suresh",Course="BTECH",Subject="IT" },
                    new Student{ Id = 3, Name ="Rajesh",Course="MCA",Subject="CS" }
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

        public IEnumerable<Student> GetAllStudent()
        {
            return _studentList;
        }

        public Student GetStudent(int id)
        {
            return _studentList.FirstOrDefault(p => p.Id == id);
        }
    }
}
