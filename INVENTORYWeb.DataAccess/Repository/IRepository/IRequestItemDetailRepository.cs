using INVENTORYWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INVENTORYWeb.DataAccess.Repository.IRepository
{
    public interface IRequestItemDetailRepository : IRepository<REQUEST_ITEM_DETAIL>
    {
        void Update(REQUEST_ITEM_DETAIL REQUEST_ITEM_DETAIL);
    }
}
