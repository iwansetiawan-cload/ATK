using INVENTORYWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INVENTORYWeb.DataAccess.Repository.IRepository
{
    public interface IRequestItemHeaderRepository : IRepository<REQUEST_ITEM_HEADER>
    {
        void Update(REQUEST_ITEM_HEADER REQUEST_ITEM_HEADER);
    }
}
