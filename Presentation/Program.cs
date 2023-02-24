﻿using Core.Constants;
using Core.Entities;
using Core.Helpers;
using Data;
using Data.Context;
using Presentation.Services;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace Presentation
{
    public static class Program
    {
        private readonly static AdminService _adminService;
        private readonly static PersonnelService _personnelService;
        private readonly static GroupService _groupService;
        private readonly static StudentService _studentService;

        static Program()
        {
            DbInitialiizer.SeedAdmins();
            _adminService = new AdminService();
            _personnelService = new PersonnelService();
            _groupService = new GroupService();
            _studentService = new StudentService();
        }

        static void Main()
        {
        Authorization:
            var admin = _adminService.Authorize();
            while (true)
            {
                Console.Clear();
            MainMenuCheck:
                ConsoleHelper.WriteWithColor($"Logged in as :{admin.Username}\n",ConsoleColor.DarkGray);
                ConsoleHelper.WriteWithColor("Main Menu", ConsoleColor.Yellow);
                ConsoleHelper.WriteWithColor("[1] - Personnel", ConsoleColor.Blue);
                ConsoleHelper.WriteWithColor("[2] - Groups", ConsoleColor.Blue);
                ConsoleHelper.WriteWithColor("[3] - Students", ConsoleColor.Blue);
                ConsoleHelper.WriteWithColor("[4] - Log Out", ConsoleColor.Blue);
                ConsoleHelper.WriteWithColor("[0] - Terminate Session",ConsoleColor.Blue);

                int menu;
                bool isRightInput = int.TryParse(Console.ReadLine(), out menu);
                if (!isRightInput)
                {
                    Console.Clear();
                    ConsoleHelper.WriteWithColor("Incorrect input format!\n Please select from 0 to 4", ConsoleColor.Red);
                    goto MainMenuCheck;
                }
                else
                {
                    switch (menu)
                    {
                        case (int)MainOptions.Personnel:
                            {
                                while (true)
                                {
                                    Console.Clear();
                                PerMenuCheck:                                   
                                    ConsoleHelper.WriteWithColor($"Logged in as :{admin.Username}\n",ConsoleColor.DarkGray);
                                    ConsoleHelper.WriteWithColor("Personnel Menu", ConsoleColor.Yellow);
                                    ConsoleHelper.WriteWithColor("[1] - Add New Personnel", ConsoleColor.Blue);
                                    ConsoleHelper.WriteWithColor("[2] - Update Personnel Profile", ConsoleColor.Blue);
                                    ConsoleHelper.WriteWithColor("[3] - Remove Personnel Profile", ConsoleColor.Blue);
                                    ConsoleHelper.WriteWithColor("[4] - Get All Personnel Profiles", ConsoleColor.Blue);
                                    ConsoleHelper.WriteWithColor("[0] - Go Back to main menu", ConsoleColor.Blue);

                                    int perMenu;
                                    isRightInput = int.TryParse(Console.ReadLine(), out perMenu);
                                    if (!isRightInput)
                                    {
                                        Console.Clear();
                                        ConsoleHelper.WriteWithColor("Incorrect input format!\nPlease select from 0 to 4", ConsoleColor.Red);
                                        goto PerMenuCheck;
                                    }
                                    else
                                    {
                                        switch (perMenu)
                                        {
                                            case (int)PersonnelOptions.AddPersonnel:
                                                _personnelService.Create();
                                                break;
                                            case (int)PersonnelOptions.UpdatePersonnel:
                                                _personnelService.Update(); 
                                                break;
                                            case (int)PersonnelOptions.RemovePersonnel:
                                                _personnelService.Remove();
                                                break;
                                            case (int)PersonnelOptions.GetAllPersonnel:
                                                _personnelService.GetAll();
                                                break;
                                            case (int)PersonnelOptions.MainMenu:
                                                Console.Clear();
                                                goto MainMenuCheck;
                                            default:
                                                Console.Clear();
                                                ConsoleHelper.WriteWithColor("Please select valid option from 0 to 4", ConsoleColor.Red);
                                                goto PerMenuCheck;

                                        }
                                    }
                                }
                            }
                        case (int)MainOptions.Groups:
                            {
                                while (true)
                                {
                                    Console.Clear();
                                GroupMenuCheck:
                                    ConsoleHelper.WriteWithColor($"Logged in as :{admin.Username}\n", ConsoleColor.DarkGray);
                                    ConsoleHelper.WriteWithColor("Group Menu", ConsoleColor.Yellow);
                                    ConsoleHelper.WriteWithColor("[1] - Add New Group", ConsoleColor.Blue);
                                    ConsoleHelper.WriteWithColor("[2] - Update Group Details", ConsoleColor.Blue);
                                    ConsoleHelper.WriteWithColor("[3] - Remove Group ", ConsoleColor.Blue);
                                    ConsoleHelper.WriteWithColor("[4] - Get All Groups", ConsoleColor.Blue);
                                    ConsoleHelper.WriteWithColor("[5] - Find Group by Id", ConsoleColor.Blue);
                                    ConsoleHelper.WriteWithColor("[6] - Find Group by Name", ConsoleColor.Blue);
                                    ConsoleHelper.WriteWithColor("[0] - Go Back to main menu", ConsoleColor.Blue);


                                    int groupMenu;
                                    isRightInput = int.TryParse(Console.ReadLine(), out groupMenu);
                                    if (!isRightInput)
                                    {
                                        Console.Clear();
                                        ConsoleHelper.WriteWithColor("Incorrect input format!\n Please select from 0 to 6", ConsoleColor.Red);
                                        goto GroupMenuCheck;
                                    }
                                    else
                                    {
                                        switch (groupMenu)
                                        {
                                            case (int)GroupOptions.AddGroup:
                                                _groupService.Create();
                                                break;
                                            case (int)GroupOptions.UpdateGroup:
                                                _groupService.Update();
                                                break;
                                            case (int)GroupOptions.RemoveGroup:
                                                _groupService.Remove();
                                                break;
                                            case (int)GroupOptions.GetAllGroups:
                                                _groupService.GetAll();
                                                break;
                                            case (int)GroupOptions.FindGroupByName:
                                                _groupService.GetByName();
                                                break;
                                            case (int)GroupOptions.FindGroupById:
                                                _groupService.GetByID();
                                                break;
                                            case (int)GroupOptions.GetGroupsByStudentCount:
                                                break;
                                            case (int)GroupOptions.MainMenu:
                                                Console.Clear();
                                                goto MainMenuCheck;
                                            default:
                                                Console.Clear();
                                                ConsoleHelper.WriteWithColor("Please select valid option from 0 to 3", ConsoleColor.Red);
                                                goto GroupMenuCheck;
                                        }
                                    }
                                }
                            }
                        case (int)MainOptions.Students:
                            while (true)
                            {
                                Console.Clear();
                            StudentMenuCheck:
                                ConsoleHelper.WriteWithColor($"Logged in as :{admin.Username}\n", ConsoleColor.DarkGray);
                                ConsoleHelper.WriteWithColor("Student Menu", ConsoleColor.Yellow);
                                ConsoleHelper.WriteWithColor("[1] - Add New Student", ConsoleColor.Blue);
                                ConsoleHelper.WriteWithColor("[2] - Update Student details", ConsoleColor.Blue);
                                ConsoleHelper.WriteWithColor("[3] - Remove Student profile ", ConsoleColor.Blue);
                                ConsoleHelper.WriteWithColor("[4] - Get All Students", ConsoleColor.Blue);
                                ConsoleHelper.WriteWithColor("[5] - Get All Students By Group", ConsoleColor.Blue);
                                ConsoleHelper.WriteWithColor("[0] - Go Back to main menu", ConsoleColor.Blue);

                                int studentMenu;
                                isRightInput = int.TryParse(Console.ReadLine(), out studentMenu);
                                if (!isRightInput)
                                {
                                    Console.Clear();
                                    ConsoleHelper.WriteWithColor("Incorrect input format!\nPlease select from 0 to 5", ConsoleColor.Red);
                                    goto StudentMenuCheck;
                                }
                                else
                                {
                                    switch (studentMenu)
                                    {
                                        case (int)StudentOptions.AddStudent:
                                            _studentService.Create();
                                            break;
                                        case (int)StudentOptions.UpdateStudent:
                                            break;
                                        case (int)StudentOptions.RemoveStudent:
                                            break;
                                        case (int)StudentOptions.GetAllStudents:
                                            break;
                                        case (int)StudentOptions.GetAllStudentsByGroup:
                                            break;
                                        case (int)GroupOptions.MainMenu:
                                            Console.Clear();
                                            goto MainMenuCheck;
                                        default:
                                            Console.Clear();
                                            ConsoleHelper.WriteWithColor("Please select valid option from 0 to 3", ConsoleColor.Red);
                                            goto StudentMenuCheck;
                                    }
                                }
                            }
                        case (int)MainOptions.LogOut:
                            Console.Clear();
                            ConsoleHelper.WriteWithColor("Are you sure you want to log out of system? y/n", ConsoleColor.Red);
                            ConsoleKeyInfo cki = Console.ReadKey();
                            if (cki.Key == ConsoleKey.Y)
                            {
                                goto Authorization;
                            }
                            else if (cki.Key == ConsoleKey.N)
                            {
                                goto MainMenuCheck;
                            }
                            break;
                        case (int)MainOptions.Exit:
                            Console.Clear();
                            ConsoleHelper.WriteWithColor("Are you sure you want to terminate current session? y/n", ConsoleColor.Red);
                            ConsoleKeyInfo cki2 = Console.ReadKey();
                            if (cki2.Key == ConsoleKey.Y)
                            {
                                return;
                            }
                            else if (cki2.Key == ConsoleKey.N)
                            {
                                break;
                            }
                            break;
                        default:
                            Console.Clear();
                            ConsoleHelper.WriteWithColor("Please select valid option from 0 to 3", ConsoleColor.Red);
                            break;
                    }
                }
            }
        }
    }
}