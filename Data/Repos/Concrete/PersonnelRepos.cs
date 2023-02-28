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
    public class PersonnelRepos : IPersonnelRepos
    {
        static int id;
        public List<Personnel> GetAll()
        {
            return DbContext.Personnels;
        }
        public Personnel Get(int id)
        {
            return DbContext.Personnels.FirstOrDefault(q => q.Id == id);
        }
        public void Add(Personnel personnel)
        {
            id++;
            personnel.Id = id;
            personnel.CreatedAt = DateTime.Now;
            DbContext.Personnels.Add(personnel);
        }
        public void Update(Personnel personnel)
        {
            var dbPersonnel = DbContext.Personnels.FirstOrDefault(d => d.Id == personnel.Id);
            if (dbPersonnel != null)
            {
                dbPersonnel.Name = personnel.Name;
                dbPersonnel.Surname = personnel.Surname;
                dbPersonnel.DOB = personnel.DOB;
                dbPersonnel.Specialty = personnel.Specialty;
                dbPersonnel.ModifiedAt = DateTime.Now;
            }
        }
        public void Delete(Personnel personnel)
        {
            DbContext.Personnels.Remove(personnel);
        }
    }
}
