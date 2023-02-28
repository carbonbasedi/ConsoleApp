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
    internal class GroupFieldService
    {
        private readonly GroupFieldRepos _groupFieldRepos;

        public GroupFieldService()
        {
            _groupFieldRepos = new GroupFieldRepos();
        }
        public void Create(Adminstrator adminstrator)
        {
            Console.Clear();
        NameCheck:
            ConsoleHelper.WriteWithColor("Enter group field name", ConsoleColor.Blue);
            string name = Console.ReadLine();
            if (String.IsNullOrEmpty(name) == true)
            {
                ConsoleHelper.WriteWithColor("Please enter group field name", ConsoleColor.Yellow);
                goto NameCheck;
            }
            var Duplicate = _groupFieldRepos.GetByName(name);
            if (Duplicate != null)
            {
                ConsoleHelper.WriteWithColor($"{name} already exists in database!\n Please assign new group field name", ConsoleColor.Red);
                goto NameCheck;
            }

            var groupField = new GroupField
            {
                Name = name,
                CreatedBy = adminstrator.Username
            };

            Console.Clear();
            _groupFieldRepos.Add(groupField);
            ConsoleHelper.WriteWithColor($" New Group Field created successfully!\n Name : {groupField.Name}\n\n\n <<< PRESS ANY KEY TO CONTINUE >>>", ConsoleColor.Green);
            Console.ReadKey();
        }
        public void Update(Adminstrator adminstrator)
        {
            Console.Clear();
            var groupFieldsCount = _groupFieldRepos.GetAll();
            if (groupFieldsCount.Count == 0)
            {
                Console.Clear();
                ConsoleHelper.WriteWithColor("There is no groups to update\nPress any key to return to menu", ConsoleColor.Red);
                Console.ReadKey();
                return;
            }
            Console.Clear();
        IDCheck:
            foreach (var groupFields in groupFieldsCount)
            {
                ConsoleHelper.WriteWithColor($" ID : {groupFields.Id}\n Name : {groupFields.Name}\n ", ConsoleColor.Yellow);
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
            var groupField = _groupFieldRepos.Get(id);
            if (groupField is null)
            {
                Console.Clear();
                ConsoleHelper.WriteWithColor("There is no Group field with this ID", ConsoleColor.Red);
                goto IDCheck;
            }
            else
            {
            NameCheck:
                Console.Clear();
                ConsoleHelper.WriteWithColor("Enter New Group Field name", ConsoleColor.Blue);
                string name = Console.ReadLine();
                var Duplicate = _groupFieldRepos.GetByName(name);
                if (Duplicate != null)
                {
                    ConsoleHelper.WriteWithColor($"{name} already exists in database!\n Please assign new group field name", ConsoleColor.Red);
                    goto NameCheck;
                }

                groupField.Name = name;
                groupField.ModifiedBy = adminstrator.Username;

                _groupFieldRepos.Update(groupField);
                Console.Clear();
                ConsoleHelper.WriteWithColor($"{groupField.Name} Group field updated successfully\n Name : {groupField.Name}\n Created by : {groupField.CreatedBy}\n Created at : {groupField.CreatedAt}\n Modified by : {adminstrator.Username}\n Modified at : {groupField.ModifiedAt}\n\n\n <<< PRESS ANY KEY TO CONTINUE >>>", ConsoleColor.Green);
                Console.ReadKey();
            }
        }
        public void Remove()
        {
            Console.Clear();
            var groupFieldCount = _groupFieldRepos.GetAll();
            if (groupFieldCount.Count == 0)
            {
                Console.Clear();
                ConsoleHelper.WriteWithColor("There is no group field in database to remove! \nPress any key to return to menu", ConsoleColor.Red);
                Console.ReadKey();
                return;
            }
        IDCheck:
            foreach (var groupFields in groupFieldCount)
            {
                ConsoleHelper.WriteWithColor($" ID : {groupFields.Id}\n Name : {groupFields.Name}\n ", ConsoleColor.Yellow);
            }

            ConsoleHelper.WriteWithColor("Enter group field ID that you want to remove or press 0 to go back to Menu", ConsoleColor.Blue);
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
            var dbGroupField = _groupFieldRepos.Get(id);
            if (dbGroupField is null)
            {
                Console.Clear();
                ConsoleHelper.WriteWithColor("There is no group with this ID number\nPlease enter valid ID number\n", ConsoleColor.Red);
                goto IDCheck;
            }
            else
            {
                _groupFieldRepos.Delete(dbGroupField);
                ConsoleHelper.WriteWithColor($" {dbGroupField.Name} successfully deleted!\n <<<PRESS ANY KEY TO CONTINUE>>>", ConsoleColor.Green);
                Console.ReadLine();
            }
        }
        public void GetAll()
        {
            Console.Clear();
            var groupFields = _groupFieldRepos.GetAll();
            if (groupFields.Count == 0)
            {
                Console.Clear();
                ConsoleHelper.WriteWithColor("There is no group fields in database!\nPress any key to return to menu", ConsoleColor.Red);
                Console.ReadKey();
                return;
            }
            Console.Clear();
            foreach (var groupField in groupFields)
            {
                ConsoleHelper.WriteWithColor($" ID : {groupField.Id}\n Name : {groupField.Name}\n", ConsoleColor.Yellow);
            }
            ConsoleHelper.WriteWithColor("\n <<<PRESS ANY KEY TO CONTINUE>>>", ConsoleColor.Yellow);
            Console.ReadKey();
        }
        public void Get()
        {
            Console.Clear();
            var groupFieldCount = _groupFieldRepos.GetAll();
            if (groupFieldCount.Count == 0)
            {
                Console.Clear();
                ConsoleHelper.WriteWithColor("There is no group in database to show!\nPress any key to return to menu", ConsoleColor.Red);
                Console.ReadKey();
                return;
            }
        IDCheck:
            foreach (var groupField in groupFieldCount)
            {
                ConsoleHelper.WriteWithColor($" ID : {groupField.Id}\n Name : {groupField.Name}\n", ConsoleColor.Yellow);
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
            var dbGroupField = _groupFieldRepos.Get(id);
            if (dbGroupField is null)
            {
                Console.Clear();
                ConsoleHelper.WriteWithColor("There is no group with this ID number\nPlease enter valid ID number\n", ConsoleColor.Red);
                goto IDCheck;
            }
            else
            {
                ConsoleHelper.WriteWithColor($"ID : {dbGroupField.Id}\nName : {dbGroupField.Name}\n  <<<PRESS ANY KEY TO CONTINUE>>>", ConsoleColor.Green);
                Console.ReadLine();
            }
        }
    }
}