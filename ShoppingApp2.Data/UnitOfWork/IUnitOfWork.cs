using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingApp2.Data.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        Task<int> SaveChangesAsync(); // Kaç kayda etki ettiğini geriye döner, o yüzden int.
        Task BeginTransaction(); // Task asenkron metotların void''i gibi düşünülebilir.
        Task CommitTransaction();
        Task RollbackTransaction();

    }
}
