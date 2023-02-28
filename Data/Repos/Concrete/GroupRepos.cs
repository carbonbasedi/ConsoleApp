using Core.Entities;
using Data.Context;
using Data.Repos.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repos.Concrete
{
    public class GroupRepos : IGroupRepos
    {
        static int id;
        public List<Group> GetAll()
        {
            return DbContext.Groups;
        }
        public List<Group> GetAllByGroupField(int id)
        {
            return DbContext.Groups.FindAll(f => f.Id == id);
        }
        public List<Group> GetGroupsByStudentCount(int studentCount)
        {
            return DbContext.Groups.Where(g => g.Students.Count >= studentCount).ToList();
        }
        public Group Get(int id)
        {
            return DbContext.Groups.FirstOrDefault(q => q.Id == id);
        }
        public Group GetByName(string name)
        {
            return DbContext.Groups.FirstOrDefault(q => q.Name == name);
        }
        public void Add(Group group)
        {
            id++;
            group.Id = id;
            group.CreatedAt = DateTime.Now;
            DbContext.Groups.Add(group);
        }
        public void Update(Group group)
        {
            var dbGroup = DbContext.Groups.FirstOrDefault(d => d.Id == group.Id);
            if (dbGroup != null)
            {
                dbGroup.Name = group.Name;
                dbGroup.MaxSize = group.MaxSize;
                dbGroup.StartDate = group.StartDate;
                dbGroup.EndDate = group.EndDate;
                dbGroup.ModifiedAt = DateTime.Now;
            }
        }
        public void Delete(Group group)
        {
            DbContext.Groups.Remove(group);
        }
    }
}
