using Core.Entities;
using Core.Helpers;
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
    internal class PersonnelService
    {
        private readonly PersonnelRepos _personnelRepos;
        public PersonnelService()
        {
            _personnelRepos = new PersonnelRepos();
        }
        public void Create(Adminstrator adminstrator)
        {
        NameCheck:
            Console.Clear();
            ConsoleHelper.WriteWithColor("Enter teacher name", ConsoleColor.Blue);
            string name = Console.ReadLine();
            if (String.IsNullOrEmpty(name) == true)
            {
                ConsoleHelper.WriteWithColor("Please enter personnel name", ConsoleColor.Yellow);
                goto NameCheck;
            }

            Console.Clear();
            ConsoleHelper.WriteWithColor("Enter teacher surname", ConsoleColor.Blue);
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
            if (dob < boundary && dob > DateTime.Now)
            {
                Console.Clear();
                ConsoleHelper.WriteWithColor("Date of birth cannot be set befor year 1950 or after current date !\nPlease enter valid date\n", ConsoleColor.Red);
                goto dobCheck;
            }

            Console.Clear();
            ConsoleHelper.WriteWithColor("Enter Specialty", ConsoleColor.Blue);
            string spec = Console.ReadLine();

            var personnel = new Personnel
            {
                Name = name,
                Surname = surname,
                DOB = dob,
                Specialty = spec,
                CreatedBy = adminstrator.Username
            };

            Console.Clear();
            _personnelRepos.Add(personnel);
            ConsoleHelper.WriteWithColor($"Personnel profile created successfully!\n Name : {personnel.Name}\n Surname : {personnel.Surname}\n Date of birth : {personnel.DOB.ToShortDateString()}\n Specialty : {personnel.Specialty}\n\n\n <<< PRESS ANY KEY TO CONTINUE >>>", ConsoleColor.Green);
            Console.ReadKey();
        }
        public void Update(Adminstrator adminstrator)
        {
            Console.Clear();
            var personnelprof = _personnelRepos.GetAll();
            if (personnelprof.Count == 0)
            {
                Console.Clear();
                ConsoleHelper.WriteWithColor("There is no personnel profiles to update\nPress any key to return to menu", ConsoleColor.Red);
                Console.ReadKey();
                return;
            }
            Console.Clear();
        IDCheck:
            foreach (var _personnel in personnelprof)
            {
                ConsoleHelper.WriteWithColor($"\n Id : {_personnel.Id}\n Name : {_personnel.Name}\n Surname : {_personnel.Surname}\n Date of birth : {_personnel.DOB.ToShortDateString()}\n Specialty : {_personnel.Specialty}", ConsoleColor.Yellow);
            }
            ConsoleHelper.WriteWithColor("Enter Personnel ID to update profile or 0 to return back to menu", ConsoleColor.Blue);
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
            var personnel = _personnelRepos.Get(id);
            if (personnel is null)
            {
                Console.Clear();
                ConsoleHelper.WriteWithColor("There is no Personnel profile with this ID", ConsoleColor.Red);
                goto IDCheck;
            }
            else
            {
                Console.Clear();
                ConsoleHelper.WriteWithColor("Enter teacher name", ConsoleColor.Blue);
                string name = Console.ReadLine();

                Console.Clear();
                ConsoleHelper.WriteWithColor("Enter teacher surname", ConsoleColor.Blue);
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
                if (dob < boundary && dob > DateTime.Now)
                {
                    Console.Clear();
                    ConsoleHelper.WriteWithColor("Date of birth cannot be set before year 1950 or after current date !\nPlease enter valid date\n", ConsoleColor.Red);
                    goto dobCheck;
                }

                Console.Clear();
                ConsoleHelper.WriteWithColor("Enter Specialty", ConsoleColor.Blue);
                string spec = Console.ReadLine();

                personnel.Name = name;
                personnel.Surname = surname;
                personnel.DOB = dob;
                personnel.Specialty = spec;
                personnel.ModifiedBy = adminstrator.Username;

                _personnelRepos.Update(personnel);
                Console.Clear();
                ConsoleHelper.WriteWithColor($"{personnel.Name} Personnel profile updated successfully\n Name : {personnel.Name}\n Surname : {personnel.Surname}\n Date of birth: {personnel.DOB.ToShortDateString()}\n Speciality : {personnel.Specialty}\n\n\n <<< PRESS ANY KEY TO CONTINUE >>>", ConsoleColor.Green);
                Console.ReadKey();
            }

        }
        public void Remove()
        {
            Console.Clear();
            var personnelCount = _personnelRepos.GetAll();
            if (personnelCount.Count == 0)
            {
                Console.Clear();
                ConsoleHelper.WriteWithColor("There is no personnel profile in database to remove! \n press any key to continue ", ConsoleColor.Red);
                Console.ReadKey();
                return;
            }
        IDCheck:
            foreach (var personnel in personnelCount)
            {
                ConsoleHelper.WriteWithColor($" ID : {personnel.Id}\n Name : {personnel.Name}\n Surname : {personnel.Surname}\n Date of birth : {personnel.DOB.ToShortDateString()}\n Speciality : {personnel.Specialty}\n", ConsoleColor.Yellow);
            }

            ConsoleHelper.WriteWithColor("Enter personnel ID that you want to remove or press 0 to go back to Menu", ConsoleColor.Blue);
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
            var dbPersonnel = _personnelRepos.Get(id);
            if (dbPersonnel is null)
            {
                Console.Clear();
                ConsoleHelper.WriteWithColor("There is no personnel profile with this ID number\nPlease enter valid ID number\n", ConsoleColor.Red);
                goto IDCheck;
            }
            else
            {
            yesNoCheck:
                ConsoleHelper.WriteWithColor("Are you sure you want to remove this account y/n", ConsoleColor.Red);
                ConsoleKeyInfo cki2 = Console.ReadKey();
                if (cki2.Key == ConsoleKey.Y)
                {
                    Console.Clear();
                    _personnelRepos.Delete(dbPersonnel);
                    ConsoleHelper.WriteWithColor($"\n{dbPersonnel.Name} {dbPersonnel.Surname} personnel profile successfully deleted!\n <<<PRESS ANY KEY TO CONTINUE>>>", ConsoleColor.Green);
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
            var personnels = _personnelRepos.GetAll();
            if (personnels.Count == 0)
            {
                Console.Clear();
                ConsoleHelper.WriteWithColor("There is no personnel profiles in database\n press any key to continue", ConsoleColor.Red);
                Console.ReadKey();
                return;
            }
            Console.Clear();
            foreach (var personnel in personnels)
            {
                ConsoleHelper.WriteWithColor($" ID : {personnel.Id}\n Name : {personnel.Name}\n Surname: {personnel.Surname}\n Date of birth : {personnel.DOB.ToShortDateString()}\n Speciality : {personnel.Specialty}\n", ConsoleColor.Yellow);

                if (personnel.Groups.Count == 0)
                {
                    ConsoleHelper.WriteWithColor("No groups assigned to this Personnel member\n", ConsoleColor.Red);
                }
                foreach (var group in personnel.Groups)
                {
                    ConsoleHelper.WriteWithColor($"Group Id : {group.Id}\n Name : {group.Name}", ConsoleColor.Green);
                }
            }
            ConsoleHelper.WriteWithColor("Press any key to return to menu\n", ConsoleColor.Green);
            Console.ReadKey();
        }
    }
}
