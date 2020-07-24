using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace WebApplication2.Models
{
    public class SqlStudentRepository : IStudentRepository
    {
        private readonly AppTestContext context;
        public SqlStudentRepository(AppTestContext context)
        {
            this.context = context;
        }

        public Student Add(Student student)
        {
            context.Students.Add(student);
            context.SaveChanges();
            return student;
        }

        public Student Delete(int id)
        {
            var student = context.Students.Find(id);
            if (student != null)
            {
                context.Students.Remove(student);
                context.SaveChanges();
            }
            return student;
        }

        public IEnumerable<Student> GetAllStudent()
        {
            //return context.Students.FromSqlRaw<Student>("Select * from students").ToList();
             return context.Students;
        }

        public Student GetStudent(int id)
        {
             return context.Students.Find(id);
            //var parameter = new SqlParameter("@id", id);
            // return context.Students.FromSqlRaw<Student>("GetStudentById {0}", id).ToList().FirstOrDefault();
            //return context.Students.FromSqlRaw<Student>("GetStudentById @id", parameter).ToList().FirstOrDefault();
            //return context.Students.FromSqlInterpolated<Student>($"GetStudentById {id}").ToList().FirstOrDefault();
        }
        

        public Student Update(Student student)
        {
            var studentUpdate = context.Students.Attach(student);
            studentUpdate.State = EntityState.Modified;
            context.SaveChanges();
            return student;
        }
    }
}
