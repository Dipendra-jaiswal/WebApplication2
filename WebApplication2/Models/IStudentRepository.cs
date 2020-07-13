using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication2.Models
{
    public interface IStudentRepository
    {
        IEnumerable<Student> GetAllStudent();
        Student GetStudent(int id);
        Student Add(Student student);
        Student Update(Student student);
        Student Delete(int id);
    }
}
