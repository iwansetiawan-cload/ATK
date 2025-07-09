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
    public class CategoryRepository : Repository<CATEGORY>, ICategoryRepository
    {
        private readonly ApplicationDbContext _db;
        public CategoryRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(CATEGORY category)
        {
            var objFromDb = _db.CATEGORY.FirstOrDefault(s => s.ID == category.ID);
            if (objFromDb != null)
            {
                objFromDb.NAME = category.NAME;
                objFromDb.DESCRIPTION = category.DESCRIPTION;
                _db.SaveChanges();
            }
        }
    }
}
