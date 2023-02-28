using Core.Helpers;
using Data.Repos.Abstract;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Repos.Concrete;
using Core.Entities;
using System.Drawing;
using System.Security.AccessControl;

namespace Presentation.Services
{
    internal class GroupService
    {
        private readonly GroupRepos _groupRepos;
        private readonly GroupFieldRepos _groupFieldRepos;
        private readonly PersonnelRepos _personnelRepos;
        private readonly StudentRepos _studentRepos;

        public GroupService()
        {
            _groupRepos = new GroupRepos();
            _groupFieldRepos = new GroupFieldRepos();
            _personnelRepos = new PersonnelRepos();
            _studentRepos = new StudentRepos();
        }
        public void Create(Adminstrator adminstrator)
        {
            if (_personnelRepos.GetAll().Count == 0)
            {
                Console.Clear();
                ConsoleHelper.WriteWithColor("There is no Teachers to assign new group to\nPlease first create Teacher\n\nPress any key to continue\n", ConsoleColor.Yellow);
                Console.ReadKey();
                return;
            }
            if (_groupFieldRepos.GetAll().Count == 0)
            {
                Console.Clear();
                ConsoleHelper.WriteWithColor("There is no Fields to assign new group to\nPlease first create Group Field\n\nPress any key to continue\n", ConsoleColor.Yellow);
                Console.ReadKey();
                return;
            }
            Console.Clear();
        NameCheck:
            ConsoleHelper.WriteWithColor("Enter group name", ConsoleColor.Blue);
            string name = Console.ReadLine();

            if (String.IsNullOrEmpty(name) == true)
            {
                ConsoleHelper.WriteWithColor("Please enter group name", ConsoleColor.Yellow);
                goto NameCheck;
            }

            var Duplicate = _groupRepos.GetByName(name);
            if (Duplicate != null)
            {
                ConsoleHelper.WriteWithColor($"{name} already exists in database!\nPlease assign new name", ConsoleColor.Red);
                goto NameCheck;
            }

            Console.Clear();
        MaxSizeCheck:
            int maxSize;
            ConsoleHelper.WriteWithColor("Enter size of group", ConsoleColor.Blue);
            bool isRightInput = int.TryParse(Console.ReadLine(), out maxSize);
            if (!isRightInput)
            {
                Console.Clear();
                ConsoleHelper.WriteWithColor("Wrong input detected!\nPlease try again\n", ConsoleColor.Red);
                goto MaxSizeCheck;
            }
            if (1 > maxSize || maxSize > 18)
            {
                Console.Clear();
                ConsoleHelper.WriteWithColor("Group Size Shouldn't exceed 18 or set lower than 1!\nEnter valid group size\n", ConsoleColor.Red);
                goto MaxSizeCheck;
            }

            Console.Clear();
        StartDateCheck:
            DateTime startDate;
            ConsoleHelper.WriteWithColor("Enter Start Date of Group", ConsoleColor.Blue);
            isRightInput = DateTime.TryParseExact(Console.ReadLine(), "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out startDate);
            if (!isRightInput)
            {
                Console.Clear();
                ConsoleHelper.WriteWithColor("Wrong input format!\nEnter date in [dd.MM.yyyy] format\n", ConsoleColor.Red);
                goto StartDateCheck;
            }
            Console.Clear();
            DateTime originDate = new DateTime(1861, 4, 1);
            if (startDate < originDate)
            {
                Console.Clear();
                ConsoleHelper.WriteWithColor("Start date of group cannot be set before establishment date of college!\nPlease enter valid date\n", ConsoleColor.Red);
                goto StartDateCheck;
            }

            Console.Clear();
        EndDateCheck:
            DateTime endDate;
            ConsoleHelper.WriteWithColor("Enter End Date of Group", ConsoleColor.Blue);
            isRightInput = DateTime.TryParseExact(Console.ReadLine(), "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out endDate);
            if (!isRightInput)
            {
                Console.Clear();
                ConsoleHelper.WriteWithColor("Wrong input format\nEnter date in [dd.MM.yyyy] format", ConsoleColor.Red);
                goto EndDateCheck;
            }
            if (startDate > endDate)
            {
                Console.Clear();
                ConsoleHelper.WriteWithColor("End date cannot be set before start date!\nPlease enter valid date\n", ConsoleColor.Red);
                goto EndDateCheck;
            }

        groupFieldCheck: Console.Clear();
            var groupFields = _groupFieldRepos.GetAll();
            foreach (var groupField in groupFields)
            {
                ConsoleHelper.WriteWithColor($"Id : {groupField.Id}\nName : {groupField.Name}", ConsoleColor.Yellow);
            }
            int num;
            ConsoleHelper.WriteWithColor("Choose field of group", ConsoleColor.Blue);
            isRightInput = int.TryParse(Console.ReadLine(), out num);
            if (!isRightInput)
            {
                Console.Clear();
                ConsoleHelper.WriteWithColor("Wrong input detected!\nPlease try again\n", ConsoleColor.Red);
                goto groupFieldCheck;
            }
            var dbGroupField = _groupFieldRepos.Get(num);
            if (dbGroupField == null)
            {
                Console.Clear();
                ConsoleHelper.WriteWithColor("Wrong Group Field Id\n please choose from list", ConsoleColor.Red);
                goto groupFieldCheck;
            }

        personnelCheck: Console.Clear();
            var personnels = _personnelRepos.GetAll();
            foreach (var personnel in personnels)
            {
                ConsoleHelper.WriteWithColor($" ID : {personnel.Id}\n Fullname : {personnel.Name} {personnel.Surname}\n Specialty : {personnel.Specialty}", ConsoleColor.Yellow);
            }

            ConsoleHelper.WriteWithColor("Enter group's Teacher id", ConsoleColor.Blue);
            int personnelId;
            isRightInput = int.TryParse(Console.ReadLine(), out personnelId);
            if (!isRightInput)
            {
                Console.Clear();
                ConsoleHelper.WriteWithColor("Wrong id input!\nChoose id from list", ConsoleColor.Red);
                goto personnelCheck;
            }
            var dbPersonnel = _personnelRepos.Get(personnelId);
            if (dbPersonnel is null)
            {
                ConsoleHelper.WriteWithColor("There is no personnel with this id", ConsoleColor.Red);
                goto personnelCheck;
            }

            var group = new Group
            {
                Name = name,
                MaxSize = maxSize,
                StartDate = startDate,
                EndDate = endDate,
                Personnel = dbPersonnel,
                GroupField = dbGroupField,
                CreatedBy = adminstrator.Username
            };

            dbPersonnel.Groups.Add(group);
            dbGroupField.Groups.Add(group);

            Console.Clear();
            _groupRepos.Add(group);
            ConsoleHelper.WriteWithColor($" {group.Name} created successfully!\n Name : {group.Name}\n Max Capacity : {group.MaxSize}\n Start Date : {group.StartDate.ToShortDateString()}\n End Date : {group.EndDate.ToShortDateString()}\n Teacher : {group.Personnel.Name} {group.Personnel.Surname}\n Group Field : {group.GroupField.Name}\n\n\n <<< PRESS ANY KEY TO CONTINUE >>>", ConsoleColor.Green);
            Console.ReadKey();
        }
        public void Update(Adminstrator adminstrator)
        {
            Console.Clear();
            var groups = _groupRepos.GetAll();
            if (groups.Count == 0)
            {
                Console.Clear();
                ConsoleHelper.WriteWithColor("There is no groups to update\nPress any key to return to menu", ConsoleColor.Red);
                Console.ReadKey();
                return;
            }
            Console.Clear();
        IDCheck:
            foreach (var _group in groups)
            {
                ConsoleHelper.WriteWithColor($" ID : {_group.Id}\n Name : {_group.Name}\n Group Size : {_group.MaxSize}\n Start Date : {_group.StartDate.ToShortDateString()}\n End Date : {_group.EndDate.ToShortDateString()}", ConsoleColor.Yellow);
            }
            ConsoleHelper.WriteWithColor("Enter Group ID or 0 to return back to menu", ConsoleColor.Blue);
            int id;
            bool isRightInput = int.TryParse(Console.ReadLine(), out id);
            if (!isRightInput)
            {
                Console.Clear();
                ConsoleHelper.WriteWithColor("Wrong Input!\nEnter Group ID to uptade it's details", ConsoleColor.Red);
                goto IDCheck;
            }
            else if (id == 0)
            {
                return;
            }
            var group = _groupRepos.Get(id);
            if (group is null)
            {
                Console.Clear();
                ConsoleHelper.WriteWithColor("There is no Group with this ID", ConsoleColor.Red);
                goto IDCheck;
            }
            else
            {
                Console.Clear();
                ConsoleHelper.WriteWithColor("Enter New Group name", ConsoleColor.Blue);
                string name = Console.ReadLine();

                Console.Clear();
            MaxSizeCheck:
                int maxSize;
                ConsoleHelper.WriteWithColor("Enter size of group", ConsoleColor.Blue);
                isRightInput = int.TryParse(Console.ReadLine(), out maxSize);
                if (!isRightInput)
                {
                    Console.Clear();
                    ConsoleHelper.WriteWithColor("Wrong input detected!\nPlease try again\n", ConsoleColor.Red);
                    goto MaxSizeCheck;
                }
                if (1 > maxSize || maxSize > 18)
                {
                    Console.Clear();
                    ConsoleHelper.WriteWithColor("Group Size Shouldn't exceed 18 or be set lower than 1!\nEnter valid group size\n", ConsoleColor.Red);
                    goto MaxSizeCheck;
                }

                Console.Clear();
            StartDateCheck:
                DateTime startDate;
                ConsoleHelper.WriteWithColor("Enter Start Date of Group", ConsoleColor.Blue);
                isRightInput = DateTime.TryParseExact(Console.ReadLine(), "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out startDate);
                if (!isRightInput)
                {
                    Console.Clear();
                    ConsoleHelper.WriteWithColor("Wrong input format!\nEnter date in [dd.MM.yyyy] format\n", ConsoleColor.Red);
                    goto StartDateCheck;
                }

                DateTime originDate = new DateTime(1861, 4, 1);
                if (startDate < originDate)
                {
                    Console.Clear();
                    ConsoleHelper.WriteWithColor("Start date of group cannot be set before establishment date of college [01.04.1861]!\nPlease enter valid date\n", ConsoleColor.Red);
                    goto StartDateCheck;
                }

                Console.Clear();
            EndDateCheck:
                DateTime endDate;
                ConsoleHelper.WriteWithColor("Enter End Date of Group", ConsoleColor.Blue);
                isRightInput = DateTime.TryParseExact(Console.ReadLine(), "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out endDate);
                if (!isRightInput)
                {
                    Console.Clear();
                    ConsoleHelper.WriteWithColor("Wrong input format\nEnter date in [dd.MM.yyyy] format", ConsoleColor.Red);
                    goto EndDateCheck;
                }
                if (startDate > endDate)
                {
                    Console.Clear();
                    ConsoleHelper.WriteWithColor("End date cannot be set before start date!\nPlease enter valid date\n", ConsoleColor.Red);
                    goto EndDateCheck;
                }

                group.Name = name;
                group.MaxSize = maxSize;
                group.StartDate = startDate;
                group.EndDate = endDate;
                group.ModifiedBy = adminstrator.Username;

                _groupRepos.Update(group);
                Console.Clear();
                ConsoleHelper.WriteWithColor($"{group.Name} Group updated successfully\n Name : {group.Name}\n Max Capacity : {group.MaxSize}\n Start Date : {group.StartDate.ToShortDateString()}\n End Date : {group.EndDate.ToShortDateString()}\n\n\n <<< PRESS ANY KEY TO CONTINUE >>>", ConsoleColor.Green);
                Console.ReadKey();
            }
        }
        public void Remove()
        {
            Console.Clear();
            var groupCount = _groupRepos.GetAll();
            if (groupCount.Count == 0)
            {
                Console.Clear();
                ConsoleHelper.WriteWithColor("There is no group in database to remove! \nPress any key to return to menu", ConsoleColor.Red);
                Console.ReadKey();
                return;
            }
        IDCheck:
            foreach (var group in groupCount)
            {
                ConsoleHelper.WriteWithColor($" ID : {group.Id}\n Name : {group.Name}\n Max Capacity : {group.MaxSize}\n Start Date : {group.StartDate.ToShortDateString()}\n End Date : {group.EndDate.ToShortDateString()}\n", ConsoleColor.Yellow);
            }

            ConsoleHelper.WriteWithColor("Enter group ID that you want to remove or press 0 to go back to Menu", ConsoleColor.Blue);
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
            var dbGroup = _groupRepos.Get(id);
            if (dbGroup is null)
            {
                Console.Clear();
                ConsoleHelper.WriteWithColor("There is no group with this ID number\nPlease enter valid ID number\n", ConsoleColor.Red);
                goto IDCheck;
            }
            else
            {
            yesNoCheck:
                ConsoleHelper.WriteWithColor("Are you sure you want to remove this group y/n", ConsoleColor.Red);
                ConsoleKeyInfo cki2 = Console.ReadKey();
                if (cki2.Key == ConsoleKey.Y)
                {
                    Console.Clear();
                    _groupRepos.Delete(dbGroup);
                    ConsoleHelper.WriteWithColor($" {dbGroup.Name} successfully deleted!\n <<<PRESS ANY KEY TO CONTINUE>>>", ConsoleColor.Green);
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
            Console.Clear();
            var groups = _groupRepos.GetAll();
            if (groups.Count == 0)
            {
                Console.Clear();
                ConsoleHelper.WriteWithColor("There is no groups in database!\nPress any key to return to menu", ConsoleColor.Red);
                Console.ReadKey();
                return;
            }
            foreach (var group in groups)
            {
                ConsoleHelper.WriteWithColor($" ID : {group.Id}\n Name : {group.Name}\n Group Size : {group.MaxSize}\n Start Date : {group.StartDate.ToShortDateString()}\n End Date : {group.EndDate.ToShortDateString()}", ConsoleColor.Yellow);
            }
            ConsoleHelper.WriteWithColor("\n <<<PRESS ANY KEY TO CONTINUE>>>", ConsoleColor.Yellow);
            Console.ReadKey();
        }
        public void GetByID()
        {
            Console.Clear();
            var groupCount = _groupRepos.GetAll();
            if (groupCount.Count == 0)
            {
                Console.Clear();
                ConsoleHelper.WriteWithColor("There is no group in database to show!\nPress any key to return to menu", ConsoleColor.Red);
                Console.ReadKey();
                return;
            }
        IDCheck:
            foreach (var group in groupCount)
            {
                ConsoleHelper.WriteWithColor($" ID : {group.Id}\n Name : {group.Name}\n Max Capacity : {group.MaxSize}\n Start Date : {group.StartDate.ToShortDateString()}\n End Date : {group.EndDate.ToShortDateString()}\n", ConsoleColor.Yellow);
            }

            ConsoleHelper.WriteWithColor("Enter group ID that you want to view or press 0 to go back to Menu", ConsoleColor.Blue);
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
            var dbGroup = _groupRepos.Get(id);
            if (dbGroup is null)
            {
                Console.Clear();
                ConsoleHelper.WriteWithColor("There is no group with this ID number\nPlease enter valid ID number\n", ConsoleColor.Red);
                goto IDCheck;
            }
            else
            {
                ConsoleHelper.WriteWithColor($"Name : {dbGroup.Name}\nSize : {dbGroup.MaxSize}\nStart Date : {dbGroup.StartDate}\nEnd Date : {dbGroup.EndDate}\nCreated at : {dbGroup.CreatedAt}\nCreated by : {dbGroup.CreatedBy}\n  <<<PRESS ANY KEY TO CONTINUE>>>", ConsoleColor.Green);
                Console.ReadLine();
            }
        }
        public void GetByName()
        {
            Console.Clear();
            var groupCount = _groupRepos.GetAll();
            if (groupCount.Count == 0)
            {
                Console.Clear();
                ConsoleHelper.WriteWithColor("There is no group in database to show!\nPress any key to return to menu", ConsoleColor.Red);
                Console.ReadKey();
                return;
            }
        NameCheck:
            foreach (var group in groupCount)
            {
                ConsoleHelper.WriteWithColor($"Name : {group.Name}\nSize : {group.MaxSize}\nStart Date : {group.StartDate}\nEnd Date : {group.EndDate}\nCreated at : {group.CreatedAt}\nCreated by : {group.CreatedBy}", ConsoleColor.Green);
            }

            ConsoleHelper.WriteWithColor("Enter group name that you want to view", ConsoleColor.Blue);
            string name = Console.ReadLine();

            var dbGroup = _groupRepos.GetByName(name);
            if (dbGroup is null)
            {
                Console.Clear();
                ConsoleHelper.WriteWithColor("There is no group with this name\n", ConsoleColor.Red);
                goto NameCheck;
            }
            else
            {
                Console.Clear();
                ConsoleHelper.WriteWithColor($"Name : {dbGroup.Name}\nSize : {dbGroup.MaxSize}\nStart Date : {dbGroup.StartDate}\nEnd Date : {dbGroup.EndDate}\nCreated at : {dbGroup.CreatedAt}\nCreated by : {dbGroup.CreatedBy}\nModified by : {dbGroup.ModifiedBy}\nModified at : {dbGroup.ModifiedAt}  <<<PRESS ANY KEY TO CONTINUE>>>", ConsoleColor.Green);
                Console.ReadKey();
            }
        }
        public void GetByStudentCount()
        {
            Console.Clear();
            var students = _studentRepos.GetAll();
            if (students.Count == 0)
            {
                Console.Clear();
                ConsoleHelper.WriteWithColor("There is no students to show groups by\nPlease first create students\nPress any key to continue\n", ConsoleColor.Red);
                Console.ReadKey();
                return;
            }
        countCheck:
            ConsoleHelper.WriteWithColor("Enter minimum student count to", ConsoleColor.Red);
            int count;
            bool isRightInput = int.TryParse(Console.ReadLine(), out count);
            if (!isRightInput)
            {
                Console.Clear();
                ConsoleHelper.WriteWithColor("Wrong input format!\nPlease enter student count again\n", ConsoleColor.Red);
                goto countCheck;

            }
            var groups = _groupRepos.GetGroupsByStudentCount(count);
            if (groups.Count == 0)
            {
                ConsoleHelper.WriteWithColor("There is no groups with this many students\npress any key to continue", ConsoleColor.Red);
                Console.ReadKey();
                goto countCheck;
            }
            foreach (var group in groups)
            {
                ConsoleHelper.WriteWithColor($"Group Id : {group.Id}\nGroup Name : {group.Name}\nGroup Teacher : {group.Personnel.Name} {group.Personnel.Surname}\nGroup Field : {group.GroupField.Name}\n", ConsoleColor.Green);
            }
            ConsoleHelper.WriteWithColor("press any key to return to main menu", ConsoleColor.Yellow);
            Console.ReadKey();
        }
        public void GetByGroupField()
        {
            Console.Clear();
            var groupFields = _groupFieldRepos.GetAll();
            if (groupFields.Count == 0)
            {
                Console.Clear();
                ConsoleHelper.WriteWithColor("There is no Fields to show Groups by\nPlease first create Group Field\n Press any key to continue\n", ConsoleColor.Red);
                Console.ReadKey();
                return;
            }

            var groups = _groupRepos.GetAll();
            if (groups.Count == 0)
            {
                Console.Clear();
                ConsoleHelper.WriteWithColor("There is no Groups to show\nPlease first create Groups\n Press any key to continue\n", ConsoleColor.Red);
                Console.ReadKey();
                return;
            }

        inputCheck: Console.Clear();
            foreach (var groupField in groupFields)
            {
                ConsoleHelper.WriteWithColor($" ID : {groupField.Id}\n Name : {groupField.Name}\n", ConsoleColor.Yellow);
            }
            ConsoleHelper.WriteWithColor("Select Group Field to show all of Groups assinged to it or press 0 to return to menu", ConsoleColor.Yellow);
            int num;
            bool isRightInput = int.TryParse(Console.ReadLine(), out num);
            if (!isRightInput)
            {
                Console.Clear();
                ConsoleHelper.WriteWithColor("Wrong input format!\nPlease enter ID again\n", ConsoleColor.Red);
                goto inputCheck;
            }
            else if (num == 0)
            {
                return;
            }
            var dbGroupField = _groupFieldRepos.Get(num);
            if (dbGroupField is null)
            {
                Console.Clear();
                ConsoleHelper.WriteWithColor("There is no group field with this ID number\nPlease enter valid ID number\n", ConsoleColor.Red);
                goto inputCheck;
            }
            foreach (var group in dbGroupField.Groups)
            {
                ConsoleHelper.WriteWithColor($"Id : {group.Id}\nName : {group.Name}\n", ConsoleColor.Green);
            }
            ConsoleHelper.WriteWithColor(" <<<PRESS ANY KEY TO CONTINUE>>>", ConsoleColor.Green);
            Console.ReadLine();

        }
        public void GetGroupsByTeacher()
        {

            var personnels = _personnelRepos.GetAll();
            if (personnels.Count == 0)
            {
                Console.Clear();
                ConsoleHelper.WriteWithColor("There is no Groups to show in database\n Press any key to continue", ConsoleColor.Red);
                Console.ReadKey();
            }
        personnelIdCheck:
            foreach (var personnel in personnels)
            {
                ConsoleHelper.WriteWithColor($"Id : {personnel.Id} \nFullname : {personnel.Name} {personnel.Surname}");
            }

            ConsoleHelper.WriteWithColor("Enter Teacher Id", ConsoleColor.Blue);
            int id;
            bool isRightInput = int.TryParse(Console.ReadLine(), out id);
            if (!isRightInput)
            {
                Console.Clear();
                ConsoleHelper.WriteWithColor("Wrong Id input format\n Please choose id from the list\nPress any key to continue ", ConsoleColor.Red);
                Console.ReadKey();
                goto personnelIdCheck;
            }

            var dbPersonnel = _personnelRepos.Get(id);
            if (dbPersonnel == null)
            {
                Console.Clear();
                ConsoleHelper.WriteWithColor("There is no personnel with this id\n Please choose from the list\nPress any key to continue", ConsoleColor.Red);
                Console.ReadKey();
            }
            Console.Clear();
            foreach (var group in dbPersonnel.Groups)
            {
                ConsoleHelper.WriteWithColor($"Id : {group.Id}\nName : {group.Name}\n", ConsoleColor.Green);
            }
            ConsoleHelper.WriteWithColor("Press any key to continue", ConsoleColor.Green);
            Console.ReadKey();
        }
    }
}

