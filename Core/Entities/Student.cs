﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Student : BaseEntity
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime DOB { get; set; }
        public string Email { get; set; }
        public Group group { get; set; }
        public string groupId { get; set; }
    }
}