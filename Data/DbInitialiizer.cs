using Core.Entities;
using Core.Helpers;
using Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public static class DbInitialiizer
    {
        static int id;

        public static void SeedAdmins()
        {
            var admins = new List<Adminstrator>
            {
                new Adminstrator
                {
                    Id = ++id,
                    Username= "admin",
                    Password= PasswordHasher.Encrypt("pentagon"),
                    CreatedBy ="System"
                },
                new Adminstrator
                {
                    Id = ++id,
                    Username= "adminka",
                    Password= PasswordHasher.Encrypt("whatever"),
                    CreatedBy ="System"
                }
            };
            DbContext.Admins.AddRange(admins);
        }
    }
}
