using Core.Entities;
using Data.Context;
using Data.Repos.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Data.Repos.Concrete
{
    public class GroupFieldRepos : IGroupFieldRepos
    {
        static int id;
        public List<GroupField> GetAll()
        {
            return DbContext.GroupFields;
        }
        public GroupField Get(int id)
        {
            return DbContext.GroupFields.FirstOrDefault(q => q.Id == id);
        }
        public void Add(GroupField groupField)
        {
            id++;
            groupField.Id = id;
            groupField.CreatedAt = DateTime.Now;
            DbContext.GroupFields.Add(groupField);
        }
        public void Delete(GroupField groupField)
        {
            DbContext.GroupFields.Remove(groupField);
        }
        public void Update(GroupField groupField)
        {
            var dbGroupField = DbContext.GroupFields.FirstOrDefault(d => d.Id == groupField.Id);
            if (dbGroupField != null)
            {
                dbGroupField.Name = groupField.Name;                
                dbGroupField.ModifiedAt = DateTime.Now;
            }
        }
        public void GetByFieldId(int id)
        {
            DbContext.GroupFields.FirstOrDefault(q => q.Id == id);
        }
    }
}
