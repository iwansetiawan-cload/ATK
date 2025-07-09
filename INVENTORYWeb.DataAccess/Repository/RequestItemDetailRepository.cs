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
    public class RequestItemDetailRepository : Repository<REQUEST_ITEM_DETAIL>, IRequestItemDetailRepository
    {
        private readonly ApplicationDbContext _db;
        public RequestItemDetailRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(REQUEST_ITEM_DETAIL obj)
        {
            _db.Update(obj);            
        }

    }
}
