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
    public class MasterProjectRepository : Repository<MASTER_PROJECT>, IMasterProjectRepository
    {
        private readonly ApplicationDbContext _db;
        public MasterProjectRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }        
    }
}
