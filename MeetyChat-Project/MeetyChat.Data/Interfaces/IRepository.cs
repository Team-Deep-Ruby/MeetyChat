namespace MeetyChat.Data.Interfaces
{
    using System.Linq;

    public interface IRepository<T> where T : class
    {
        IQueryable<T> All();
        T GetById(object id);
        void Add(T entity);
        void Delete(T entity);
        void Update(T entity);
        void Detach(T entity);
    }
}
