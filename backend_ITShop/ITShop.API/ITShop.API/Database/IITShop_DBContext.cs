using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database
{
    public interface IITShop_DBContext
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    }
}
