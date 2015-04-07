using System.Linq;

namespace Zeitgeist.Core.Data.Interfaces
{
    public partial interface IRepository<T> where T : IEntity
    {
        T GetById(object id);
        void Insert(T entity);
        void Update(T entity);
        void Delete(T entity);
        IQueryable<T> Table { get; }
    }
}
