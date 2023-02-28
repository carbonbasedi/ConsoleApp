using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entities;

namespace Data.Context
{
    public static class DbContext
    {
        static DbContext()
        {
            Admins = new List<Adminstrator>();
            Personnels = new List<Personnel>();
            GroupFields = new List<GroupField>();
            Groups = new List<Group>();
            Students = new List<Student>();
        }
        public static List<Adminstrator> Admins { get; set; }
        public static List<Personnel> Personnels { get; set; }
        public static List<GroupField> GroupFields { get; set; }
        public static List<Group> Groups { get; set; }
        public static List<Student> Students { get; set; }
    }
}