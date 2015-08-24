namespace MeetyChat.Data.Data
{
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using Interfaces;

    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly IMeetyChatDbContext context;

        public Repository(IMeetyChatDbContext context)
        {
            this.context = context;
        }

        public Repository()
            :this(new MeetyChatDbContext())
        {
        }

        public IQueryable<T> All()
        {
            return this.context.Set<T>();
        }

        public T GetById(object id)
        {
            return this.context.Set<T>().Find(id);
        }

        public void Add(T entity)
        {
            this.ChangeState(entity, EntityState.Added);
        }

        public void Delete(T entity)
        {
            this.ChangeState(entity, EntityState.Deleted);
        }

        public void Update(T entity)
        {
            this.ChangeState(entity, EntityState.Modified);
        }

        public void Detach(T entity)
        {
            this.ChangeState(entity, EntityState.Detached);
        }

        private void ChangeState(T entity, EntityState state)
        {
            var entry = this.AttachIfDetached(entity);
            entry.State = state;
        }

        private DbEntityEntry AttachIfDetached(T entity)
        {
            var entry = this.context.Entry(entity);

            if (entry.State == EntityState.Detached)
            {
                this.context.Set<T>().Attach(entity);
            }

            return entry;
        }
    }
}
