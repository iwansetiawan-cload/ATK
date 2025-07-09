using INVENTORYWeb.DataAccess.Data;
using INVENTORYWeb.DataAccess.Repository.IRepository;
using INVENTORYWeb.Models;
using INVENTORYWeb.DataAccess.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INVENTORYWeb.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;
        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            ApplicationUser = new ApplicationUserRepository(_db);
            MSUDC = new MSUDCRepository(_db);
            SP_Call = new SP_Call(_db);
            Category = new CategoryRepository(_db);
            MasterProject = new MasterProjectRepository(_db);
            Items = new ItemsRepository(_db);
            RequestItemHeader = new RequestItemHeaderRepository(_db);
            RequestItemDetail = new RequestItemDetailRepository(_db);
        }
        public IApplicationUserRepository ApplicationUser { get; private set; }
        public IMSUDCRepository MSUDC { get; private set; }
        public ISP_Call SP_Call { get; private set; }
        public ICategoryRepository Category { get; private set; }
        public IMasterProjectRepository MasterProject { get; private set; }
        public IItemsRepository Items { get; private set; }
        public IRequestItemHeaderRepository RequestItemHeader { get; private set; }
        public IRequestItemDetailRepository RequestItemDetail { get; private set; }

        public void Dispose()
        {
            _db.Dispose();
        }
        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
