using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repos.Abstract
{
    public interface IGroupRepos : IRepos<Group>
    {
        List<Group> GetAllByGroupField(int id);
        List<Group> GetGroupsByStudentCount(int studentCount);
        Group GetByName(string name);
    }
}