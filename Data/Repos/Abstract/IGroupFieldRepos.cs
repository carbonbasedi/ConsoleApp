using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repos.Abstract
{
    internal interface IGroupFieldRepos : IRepos<GroupField>
    {
        GroupField GetByName(string name);
    }
}
