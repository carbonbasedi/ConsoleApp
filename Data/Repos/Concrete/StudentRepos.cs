using Core.Entities;
using Data.Context;
using Data.Repos.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repos.Concrete
{
    public class StudentRepos : IStudentRepos
    {
        static int id;
        public List<Student> GetAll()
        {
            return DbContext.Students;
        }
        public Student Get(int id)
        {
            return DbContext.Students.FirstOrDefault(q => q.Id == id);
        }
        public void Add(Student student)
        {
            id++;
            student.Id = id;
            student.CreatedAt = DateTime.Now;
            DbContext.Students.Add(student);
        }
        public void Update(Student student)
        {
            var dbStudent = DbContext.Students.FirstOrDefault(d => d.Id == student.Id);
            if (dbStudent != null)
            {
                dbStudent.Name = student.Name;
                dbStudent.Surname = student.Surname;
                dbStudent.DOB = student.DOB;
                dbStudent.Email = student.Email;
                dbStudent.ModifiedAt = DateTime.Now;
            }
        }
        public void Delete(Student student)
        {
            DbContext.Students.Remove(student);
        }
    }
}
