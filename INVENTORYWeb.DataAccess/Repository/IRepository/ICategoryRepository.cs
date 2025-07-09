using INVENTORYWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INVENTORYWeb.DataAccess.Repository.IRepository
{
    public interface ICategoryRepository : IRepository<CATEGORY>
    {
        void Update(CATEGORY CATEGORY);
    }
}
