using INVENTORYWeb.DataAccess.Data;
using INVENTORYWeb.DataAccess.Repository.IRepository;
using INVENTORYWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INVENTORYWeb.DataAccess.Repository
{
    public class ApplicationUserRepository : Repository<ApplicationUser>, IApplicationUserRepository
    {
        private readonly ApplicationDbContext _db;

        public ApplicationUserRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(ApplicationUser obj)
        {
            _db.Update(obj);
            //try
            //{
            //    var objFromDb = _db.ApplicationUsers.FirstOrDefault(s => s.Id == obj.Id);
            //    if (objFromDb != null)
            //    {
            //        objFromDb.FullName = obj.FullName;
            //        objFromDb.Role = obj.Role;
            //        objFromDb.RolesName = obj.RolesName;
            //        objFromDb.LockoutEnabled = obj.LockoutEnabled;
            //        _db.SaveChanges();
            //    }
            //}
            //catch (Exception ex)
            //{
            //    string err = ex.Message;
            //}

        }
    }
}
