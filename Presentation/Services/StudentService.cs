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
using System.Text.RegularExpressions;

namespace Presentation.Services
{
    internal class StudentService
    {
        private readonly StudentRepos _studentRepos;
        private readonly GroupRepos _groupRepos;
        private readonly GroupService _groupService;
        public StudentService()
        {
            _studentRepos = new StudentRepos();
            _groupRepos = new GroupRepos();
            _groupService = new GroupService();
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
        EmailCheck:
            ConsoleHelper.WriteWithColor("Enter student's email address", ConsoleColor.Blue);
            string email = Console.ReadLine();
            if (email.IsEmail() == false)
            {
                Console.Clear();
                ConsoleHelper.WriteWithColor("Wrong email input format!\n", ConsoleColor.Red);
                goto EmailCheck;
            }
            if (String.IsNullOrEmpty(email) == true)
            {
                ConsoleHelper.WriteWithColor("Please enter email address", ConsoleColor.Yellow);
                goto EmailCheck;
            }

        groupIdCheck: Console.Clear();
            var groups = _groupRepos.GetAll();
            foreach (var group in groups)
            {
                ConsoleHelper.WriteWithColor($" ID : {group.Id}\n Name : {group.Name}\n Group Size : {group.MaxSize}\n Start Date : {group.StartDate.ToShortDateString()}\n End Date : {group.EndDate.ToShortDateString()}", ConsoleColor.Yellow);
            }

            ConsoleHelper.WriteWithColor("Enter student's group id", ConsoleColor.Blue);
            int groupId;
            isRightInput = int.TryParse(Console.ReadLine(), out groupId);
            if (!isRightInput)
            {
                Console.Clear();
                ConsoleHelper.WriteWithColor("Wrong id input!\nChoose id from list");
                goto groupIdCheck;
            }

            var dbGroup = _groupRepos.Get(groupId);
            if (dbGroup.MaxSize <= dbGroup.Students.Count)
            {
                Console.Clear();
                ConsoleHelper.WriteWithColor("This group is full\nChoose different group", ConsoleColor.Red);
                goto groupIdCheck;
            }

            var student = new Student
            {
                Name = name,
                Surname = surname,
                DOB = dob,
                Email = email,
                Group = dbGroup,
                GroupId = groupId,
                CreatedBy = adminstrator.Username
            };

            dbGroup.Students.Add(student);

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
                ConsoleHelper.WriteWithColor($"\nId : {students.Id}\n Name : {students.Name}\n Surname : {students.Surname}\n Date of birth : {students.DOB.ToShortDateString()}\n Email : {students.Email}\n Created by : {students.CreatedBy}\n\n\n ", ConsoleColor.Yellow);
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
            yesNoCheck:
                ConsoleHelper.WriteWithColor("Are you sure you want to remove this student profile  y/n", ConsoleColor.Red);
                ConsoleKeyInfo cki2 = Console.ReadKey();
                if (cki2.Key == ConsoleKey.Y)
                {
                    Console.Clear();
                    _studentRepos.Delete(dbStudent);
                    ConsoleHelper.WriteWithColor($" {dbStudent.Name} {dbStudent.Surname} Student profile successfully deleted!\n <<<PRESS ANY KEY TO CONTINUE>>>", ConsoleColor.Green);
                    Console.ReadLine();
                }
                else if (cki2.Key == ConsoleKey.N)
                {
                    Console.Clear();
                    goto IDCheck;
                }
                else
                {
                    Console.Clear();
                    ConsoleHelper.WriteWithColor("Please select y/n", ConsoleColor.Red);
                    goto yesNoCheck;
                }

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

            Console.Clear();
            foreach (var student in students)
            {
                ConsoleHelper.WriteWithColor($" ID : {student.Id}\n Name : {student.Name}\n Surname: {student.Surname}\n Date of birth : {student.DOB.ToShortDateString()}\n Email : {student.Email}\n Student Group Id : {student.GroupId}\n Student Group : {student.Group.Name}\n Created by : {student.CreatedBy}\n Created at : {student.CreatedAt}\n Modified by : {student.ModifiedBy}\n Modified at :{student.ModifiedAt}\n", ConsoleColor.Yellow);
            }
            ConsoleHelper.WriteWithColor("Press any key to continue", ConsoleColor.Green);
            Console.ReadKey();
        }
        public void GetAllByGroup()
        {
            var students = _studentRepos.GetAll();
            if (students.Count == 0)
            {
                Console.Clear();
                ConsoleHelper.WriteWithColor("There is no student profiles in database\nPress any key to return to menu", ConsoleColor.Red);
                Console.ReadKey();
                return;
            }

            var groups = _groupRepos.GetAll();
            if (groups.Count == 0)
            {
                Console.Clear();
                ConsoleHelper.WriteWithColor("There is no groups in database!\nPress any key to return to menu", ConsoleColor.Red);
                Console.ReadKey();
                return;
            }
        IdCheck:
            foreach (var group in groups)
            {
                ConsoleHelper.WriteWithColor($" ID : {group.Id}\n Name : {group.Name}\n Group Size : {group.MaxSize}\n Start Date : {group.StartDate.ToShortDateString()}\n End Date : {group.EndDate.ToShortDateString()}", ConsoleColor.Yellow);
            }
            int id;
            bool isRightInput = int.TryParse(Console.ReadLine(), out id);
            if (!isRightInput)
            {
                Console.Clear();
                ConsoleHelper.WriteWithColor("Wrong group id input\n Please selec from list");
                goto IdCheck;
            }

            var dbGroup = _groupRepos.Get(id);
            if (dbGroup == null)
            {
                Console.Clear();
                ConsoleHelper.WriteWithColor("There is no group with this id\n Please choose from the list\nPress any key to continue", ConsoleColor.Red);
                Console.ReadKey();
            }
            Console.Clear();
            foreach (var student in dbGroup.Students)
            {
                ConsoleHelper.WriteWithColor($"Id : {student.Id}\nName : {student.Name}\nSurname : {student.Surname}\n", ConsoleColor.Green);
            }
            ConsoleHelper.WriteWithColor("Press any key to continue", ConsoleColor.Green);
            Console.ReadKey();
        }
    }
}
