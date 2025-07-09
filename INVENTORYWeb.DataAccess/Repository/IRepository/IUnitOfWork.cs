using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INVENTORYWeb.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork : IDisposable
    {
        IApplicationUserRepository ApplicationUser { get; }
        IMSUDCRepository MSUDC { get; }
        ISP_Call SP_Call { get; }
        ICategoryRepository Category { get; }
        IMasterProjectRepository MasterProject { get; }
        IItemsRepository Items { get; }
        IRequestItemHeaderRepository RequestItemHeader { get; }
        IRequestItemDetailRepository RequestItemDetail { get; }
        void Save();
    }
}
