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
    public class MSUDCRepository : Repository<MSUDC>, IMSUDCRepository
    {
        private readonly ApplicationDbContext _db;
        public MSUDCRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        
    }
}
