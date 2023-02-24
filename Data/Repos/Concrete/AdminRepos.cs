using Core.Entities;
using Core.Helpers;
using Data.Context;
using Data.Repos.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repos.Concrete
{
    public class AdminRepos : IAdminRepos
    {
        public Adminstrator GetByUsernameAndPassword(string username, string password)
        {
          return DbContext.Admins.FirstOrDefault(a => a.Username.ToLower() == username.ToLower() && PasswordHasher.Decrypt(a.Password) == password);
        }
    }
}
