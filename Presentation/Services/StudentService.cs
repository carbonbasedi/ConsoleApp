using Core.Entities;
using Core.Helpers;
using Core.Extensions;
using Data.Repos.Abstract;
using Data.Repos.Concrete;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Services
{
    internal class StudentService
    {
        private readonly StudentRepos _studentRepos;
        private readonly GroupRepos _groupRepos;
        public StudentService()
        {
            _studentRepos = new StudentRepos();
            _groupRepos = new GroupRepos();
        }
        public void Create(Adminstrator adminstrator)
        {
            if (_groupRepos.GetAll().Count == 0)
            {
                ConsoleHelper.WriteWithColor("There is no Groups to assign new student to\nPlease create group first\n Press any key to continue", ConsoleColor.Red);
                Console.ReadLine();
                return;
            }
            Console.Clear();
            ConsoleHelper.WriteWithColor("Enter student name", ConsoleColor.Blue);
            string name = Console.ReadLine();

            Console.Clear();
            ConsoleHelper.WriteWithColor("Enter student surname", ConsoleColor.Blue);
            string surname = Console.ReadLine();

            Console.Clear();
        dobCheck:
            DateTime dob;
            ConsoleHelper.WriteWithColor("Enter date of birth", ConsoleColor.Blue);
            bool isRightInput = DateTime.TryParseExact(Console.ReadLine(), "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dob);
            if (!isRightInput)
            {
                Console.Clear();
                ConsoleHelper.WriteWithColor("Wrong input format!\nEnter date in [dd.MM.yyyy] format\n", ConsoleColor.Red);
                goto dobCheck;
            }
            Console.Clear();
            DateTime boundary = new DateTime(1950, 1, 1);
            if (dob < boundary)
            {
                Console.Clear();
                ConsoleHelper.WriteWithColor("Date of birth cannot be set higher than year 1950 !\nPlease enter valid date\n", ConsoleColor.Red);
                goto dobCheck;
            }

            Console.Clear();
            ConsoleHelper.WriteWithColor("Enter student's email adress", ConsoleColor.Blue);
            string email = Console.ReadLine();
            if (email.IsEmail() == false)
            {
                ConsoleHelper.WriteWithColor("Wrong email input format!", ConsoleColor.Red);
            }

            Console.Clear();
            var groups = _groupRepos.GetAll();
            foreach (var group in groups)
            {
                ConsoleHelper.WriteWithColor($" ID : {group.Id}\n Name : {group.Name}\n Group Size : {group.MaxSize}\n Start Date : {group.StartDate.ToShortDateString()}\n End Date : {group.EndDate.ToShortDateString()}", ConsoleColor.Yellow);
            }
            string groupId;


            ConsoleHelper.WriteWithColor("Enter student's email adress", ConsoleColor.Blue);

            var student = new Student
            {
                Name = name,
                Surname = surname,
                DOB = dob,
                Email = email,
                CreatedBy = adminstrator.Username

            };

            Console.Clear();
            _studentRepos.Add(student);
            ConsoleHelper.WriteWithColor($"Student profile created successfully!\nId : {student.Id}\n Name : {student.Name}\n Surname : {student.Surname}\n Date of birth : {student.DOB.ToShortDateString()}\n Email : {student.Email}\n Created by : {student.CreatedBy}\n\n\n <<< PRESS ANY KEY TO CONTINUE >>>", ConsoleColor.Green);
            Console.ReadKey();
        }
        public void Update(Adminstrator adminstrator)
        {
            Console.Clear();
            var studentprof = _studentRepos.GetAll();
            if (studentprof.Count == 0)
            {
                Console.Clear();
                ConsoleHelper.WriteWithColor("There is no student profiles to update\nPress any key to return to menu", ConsoleColor.Red);
                Console.ReadKey();
                return;
            }
            Console.Clear();
        IDCheck:
            foreach (var student in studentprof)
            {
                ConsoleHelper.WriteWithColor($"\n Id : {student.Id}\n Name : {student.Name}\n Surname : {student.Surname}\n Date of birth : {student.DOB.ToShortDateString()}\n Email adress : {student.Email}", ConsoleColor.Yellow);
            }
            ConsoleHelper.WriteWithColor("Enter Student ID to update profile or 0 to return back to menu", ConsoleColor.Blue);
            int id;
            bool isRightInput = int.TryParse(Console.ReadLine(), out id);
            if (!isRightInput)
            {
                Console.Clear();
                ConsoleHelper.WriteWithColor("Wrong Input!\nEnter Personnel ID to uptade it's profile details", ConsoleColor.Red);
                goto IDCheck;
            }
            else if (id == 0)
            {
                return;
            }
            var _student = _studentRepos.Get(id);
            if (_student is null)
            {
                Console.Clear();
                ConsoleHelper.WriteWithColor("There is no Student profile with this ID", ConsoleColor.Red);
                goto IDCheck;
            }
            else
            {
                Console.Clear();
                ConsoleHelper.WriteWithColor("Enter student name", ConsoleColor.Blue);
                string name = Console.ReadLine();

                Console.Clear();
                ConsoleHelper.WriteWithColor("Enter student surname", ConsoleColor.Blue);
                string surname = Console.ReadLine();

                Console.Clear();
            dobCheck:
                DateTime dob;
                ConsoleHelper.WriteWithColor("Enter date of birth", ConsoleColor.Blue);
                isRightInput = DateTime.TryParseExact(Console.ReadLine(), "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dob);
                if (!isRightInput)
                {
                    Console.Clear();
                    ConsoleHelper.WriteWithColor("Wrong input format!\nEnter date in [dd.MM.yyyy] format\n", ConsoleColor.Red);
                    goto dobCheck;
                }
                Console.Clear();
                DateTime boundary = new DateTime(1950, 1, 1);
                if (dob < boundary)
                {
                    Console.Clear();
                    ConsoleHelper.WriteWithColor("Date of birth cannot be set higher than year 1950 !\nPlease enter valid date\n", ConsoleColor.Red);
                    goto dobCheck;
                }

                Console.Clear();
                string email = Console.ReadLine();
                if (email.IsEmail() == false)
                {
                    ConsoleHelper.WriteWithColor("Wrong email input format!", ConsoleColor.Red);
                }

                _student.Name = name;
                _student.Surname = surname;
                _student.DOB = dob;
                _student.ModifiedBy = adminstrator.Username;

                _studentRepos.Update(_student);
                Console.Clear();
                ConsoleHelper.WriteWithColor($"{_student.Name} Student profile updated successfully\n Name : {_student.Name}\n Surname : {_student.Surname}\n Date of birth: {_student.DOB.ToShortDateString()}\n\n\n <<< PRESS ANY KEY TO CONTINUE >>>", ConsoleColor.Green);
                Console.ReadKey();
            }
        }
        public void Remove()
        {
            Console.Clear();
            var studentCount = _studentRepos.GetAll();
            if (studentCount.Count == 0)
            {
                Console.Clear();
                ConsoleHelper.WriteWithColor("There is no group in database to remove! \n Press any key to continue", ConsoleColor.Red);
                Console.ReadKey();
                return;
            }
        IDCheck:
            foreach (var students in studentCount)
            {
                ConsoleHelper.WriteWithColor($"\nId : {students.Id}\n Name : {students.Name}\n Surname : {students.Surname}\n Date of birth : {students.DOB.ToShortDateString()}\n Email : {students.Email}\n Created by : {students.CreatedBy}\n\n\n <<< PRESS ANY KEY TO CONTINUE >>>", ConsoleColor.Green);
            }

            ConsoleHelper.WriteWithColor("Enter student ID that you want to remove or press 0 to go back to Menu", ConsoleColor.Blue);
            int id;
            bool isRightInput = int.TryParse(Console.ReadLine(), out id);
            if (!isRightInput)
            {
                Console.Clear();
                ConsoleHelper.WriteWithColor("Wrong input format!\nPlease enter ID again\n", ConsoleColor.Red);
                goto IDCheck;
            }
            else if (id == 0)
            {
                return;
            }
            var dbStudent = _studentRepos.Get(id);
            if (dbStudent is null)
            {
                Console.Clear();
                ConsoleHelper.WriteWithColor("There is no student with this ID number\nPlease enter valid ID number\n", ConsoleColor.Red);
                goto IDCheck;
            }
            else
            {
                _studentRepos.Delete(dbStudent);
                ConsoleHelper.WriteWithColor($" {dbStudent.Name} Student profile successfully deleted!\n <<<PRESS ANY KEY TO CONTINUE>>>", ConsoleColor.Green);
                Console.ReadLine();
            }
        }
        public void GetAll()
        {
            var students = _studentRepos.GetAll();
            if (students.Count == 0)
            {
                Console.Clear();
                ConsoleHelper.WriteWithColor("There is no student profiles in database\nPress any key to return to menu", ConsoleColor.Red);
                Console.ReadKey();
                return;
            }
        IDCheck:
            foreach (var student in students)
            {
                ConsoleHelper.WriteWithColor($" ID : {student.Id}\n Name : {student.Name}\n Surname: {student.Surname}\n Date of birth : {student.DOB.ToShortDateString()}\n Email : {student.Email}\n Created by : {student.CreatedBy}\n Created at : {student.CreatedAt}\n Modified by : {student.ModifiedBy}\n Modified at :{student.ModifiedAt}", ConsoleColor.Yellow);
            }

            ConsoleHelper.WriteWithColor("Enter student ID that you want to remove or press 0 to go back to Menu", ConsoleColor.Blue);
            int id;
            bool isRightInput = int.TryParse(Console.ReadLine(), out id);
            if (!isRightInput)
            {
                Console.Clear();
                ConsoleHelper.WriteWithColor("Wrong input format!\nPlease enter ID again\n", ConsoleColor.Red);
                goto IDCheck;
            }
            else if (id == 0)
            {
                return;
            }
            var dbStudent = _studentRepos.Get(id);
            if (dbStudent is null)
            {
                Console.Clear();
                ConsoleHelper.WriteWithColor("There is no student profile with this ID number\nPlease enter valid ID number\n", ConsoleColor.Red);
                goto IDCheck;
            }
            else
            {
                _studentRepos.Delete(dbStudent);
                ConsoleHelper.WriteWithColor($"{dbStudent.Name} student profile successfully deleted!\n <<<PRESS ANY KEY TO CONTINUE>>>", ConsoleColor.Green);
                Console.ReadLine();
            }
        }
    }
}
