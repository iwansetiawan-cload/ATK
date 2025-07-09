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
    public class ItemsRepository : RepositoryAsync<ITEMS>, IItemsRepository
    {
        private readonly ApplicationDbContext _db;
        public ItemsRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(ITEMS items)
        {
            var objFromDb = _db.ITEMS.FirstOrDefault(s => s.ID == items.ID);
            if (objFromDb != null)
            {
                objFromDb.NAME = items.NAME;
                //objFromDb.STOCK = items.STOCK;
                //objFromDb.QTY = items.QTY;
                objFromDb.PIECE = items.PIECE;
                objFromDb.DESCRIPTION = items.DESCRIPTION;
                _db.SaveChanges();
            }
        }
    }
}
