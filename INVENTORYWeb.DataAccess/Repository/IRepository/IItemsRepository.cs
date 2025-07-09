using INVENTORYWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INVENTORYWeb.DataAccess.Repository.IRepository
{
    public interface IItemsRepository : IRepositoryAsync<ITEMS>
    {
        void Update(ITEMS ITEMS);
    }
}
