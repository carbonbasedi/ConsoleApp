using Core.Entities;
using Core.Helpers;
using Data.Repos.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Services
{
    public class AdminService
    {
        private readonly AdminRepos _adminRepos;
        public AdminService()
        {
             _adminRepos = new AdminRepos();
        }
        public Adminstrator Authorize()
        {
          LoginCheck: 
            Console.WriteLine("\n---- Login ----");

            Console.Write("Username :");
            string usernameInput = Console.ReadLine();

            Console.Write("Password :");
            var passwordInput = string.Empty;
            ConsoleKey key;
            do
            {
                var keyInfo = Console.ReadKey(intercept: true);
                key = keyInfo.Key;

                if (key == ConsoleKey.Backspace && passwordInput.Length > 0)
                {
                    Console.Write("\b \b");
                    passwordInput = passwordInput[0..^1];
                }
                else if (!char.IsControl(keyInfo.KeyChar))
                {
                    Console.Write("*");
                    passwordInput += keyInfo.KeyChar;
                }
            } while (key != ConsoleKey.Enter);

            var admin = _adminRepos.GetByUsernameAndPassword(usernameInput, passwordInput);
            if (admin is null)
            {
                Console.Clear();
                ConsoleHelper.WriteWithColor("Username and/or password is incorrect!", ConsoleColor.Red);
                goto LoginCheck;
            }
            return admin;
        }
    }
}