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
        public void Create()
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

            ConsoleHelper.WriteWithColor("Enter student's email adress", ConsoleColor.Blue);

            var student = new Student
            {
                Name = name,
                Surname = surname,
                DOB = dob,
                Email = email,

            };

            Console.Clear();
            _studentRepos.Add(student);
            ConsoleHelper.WriteWithColor($"Personnel profile created successfully!\nId : {student.Id}\n Name : {student.Name}\n Surname : {student.Surname}\n Date of birth : {student.DOB.ToShortDateString()}\n Email : {student.Email}\n\n\n <<< PRESS ANY KEY TO CONTINUE >>>", ConsoleColor.Green);
            Console.ReadKey();
        }
        public void Update()
        {

        }
    }
}
