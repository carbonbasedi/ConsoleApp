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

namespace Presentation.Services
{
    internal class GroupService
    {
        private readonly GroupRepos _groupRepos;
        private readonly GroupFieldRepos _groupFieldRepos;

        public GroupService()
        {
            _groupRepos = new GroupRepos();
            _groupFieldRepos = new GroupFieldRepos();
        }
        public void Create()
        {
            //if(_groupFieldRepos.GetAll().Count == 0)
            //{
            //    Console.Clear();
            //    ConsoleHelper.WriteWithColor("There is no Fields to assign new group to\nPlease first create Group Field", ConsoleColor.Red);
            //}
            Console.Clear();
        NameCheck:
            ConsoleHelper.WriteWithColor("Enter group name", ConsoleColor.Blue);
            string name = Console.ReadLine();
            var Duplicate = _groupRepos.GetByName(name);
            if (Duplicate != null)
            {
                ConsoleHelper.WriteWithColor($"{name} already exists in database!\n Please assign new name", ConsoleColor.Red);
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

            var group = new Group
            {
                Name = name,
                MaxSize = maxSize,
                StartDate = startDate,
                EndDate = endDate,
            };

            Console.Clear();
            _groupRepos.Add(group);
            ConsoleHelper.WriteWithColor($" {group.Name} created successfully!\n Name : {group.Name}\n Max Capacity : {group.MaxSize}\n Start Date : {group.StartDate.ToShortDateString()}\n End Date : {group.EndDate.ToShortDateString()}\n\n\n <<< PRESS ANY KEY TO CONTINUE >>>", ConsoleColor.Green);
            Console.ReadKey();
        }
        public void Update()
        {
            Console.Clear();
            var groups = _groupRepos.GetAll();
            if (groups.Count == 0)
            {
                Console.Clear();
                ConsoleHelper.WriteWithColor("There is no groups to update\nPress any key to return to menu", ConsoleColor.Red);
                Console.ReadKey();
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
                ConsoleHelper.WriteWithColor("There is no group in database to remove! \n", ConsoleColor.Red);
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
                _groupRepos.Delete(dbGroup);
                ConsoleHelper.WriteWithColor($" {dbGroup.Name} successfully deleted!\n <<<PRESS ANY KEY TO CONTINUE>>>", ConsoleColor.Green);
                Console.ReadLine();
            }
        }
        public void GetAll()
        {
            Console.Clear();
            var groups = _groupRepos.GetAll();
            if (groups.Count == 0)
            {
                Console.Clear();
                ConsoleHelper.WriteWithColor("There is no groups in database!", ConsoleColor.Red);
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
                ConsoleHelper.WriteWithColor("There is no group in database to show! \n", ConsoleColor.Red);
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
                ConsoleHelper.WriteWithColor($"Name : {dbGroup.Name}\nSize : {dbGroup.MaxSize}\nStart Date : {dbGroup.StartDate}\nEnd Date : {dbGroup.EndDate}\nCreated at : {dbGroup.CreatedBy}\nCreated by : {dbGroup.CreatedBy}  <<<PRESS ANY KEY TO CONTINUE>>>", ConsoleColor.Green);
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
                ConsoleHelper.WriteWithColor("There is no group in database to show! \n", ConsoleColor.Red);
            }
        NameCheck:
            foreach (var group in groupCount)
            {
                ConsoleHelper.WriteWithColor($"Name : {group.Name}\nSize : {group.MaxSize}\nStart Date : {group.StartDate}\nEnd Date : {group.EndDate}\nCreated at : {group.CreatedBy}\nCreated by : {group.CreatedBy}", ConsoleColor.Green);
            }

            ConsoleHelper.WriteWithColor("Enter group name that you want to view or 0 to go back to Menu", ConsoleColor.Blue);
            string name = Console.ReadLine();

            var dbGroup = _groupRepos.GetByName(name);
            if (dbGroup is null)
            {
                Console.Clear();
                ConsoleHelper.WriteWithColor("There is no group with this name\n", ConsoleColor.Red);
                goto NameCheck;
            }
            else if (name == "0")
            {
                return;
            }
            else
            {
                ConsoleHelper.WriteWithColor($"Name : {dbGroup.Name}\nSize : {dbGroup.MaxSize}\nStart Date : {dbGroup.StartDate}\nEnd Date : {dbGroup.EndDate}\nCreated at : {dbGroup.CreatedBy}\nCreated by : {dbGroup.CreatedBy}  <<<PRESS ANY KEY TO CONTINUE>>>", ConsoleColor.Green);
                Console.ReadLine();
            }
        }
    }
}

