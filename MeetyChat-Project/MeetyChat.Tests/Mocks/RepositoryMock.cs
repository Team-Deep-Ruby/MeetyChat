namespace MeetyChat.Tests.Mocks
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Data.Interfaces;

    public class RepositoryMock<T> : IRepository<T> 
        where T : class
    {
        public IList<T> Entities
        {
            get;
            set;
        }

        public RepositoryMock()
        {
            this.Entities = new List<T>();
        }

        public void Add(T entity)
        {
            this.Entities.Add(entity);
        }

        public IQueryable<T> All()
        {
            return this.Entities.AsQueryable();
        }

        public void Delete(T entity)
        {
            this.Entities.Remove(entity);
        }

        public void Update(T entity)
        {
            throw new NotImplementedException();
        }

        public void SaveChanges()
        {
            this.IsSaveCalled = true;
        }

        public bool IsSaveCalled
        {
            get;
            set;
        }

        public T GetById(object id)
        {
            throw new NotImplementedException();
        }

        public void Delete(object id)
        {
            throw new NotImplementedException();
        }

        public void Detach(T entity)
        {
            throw new NotImplementedException();
        }
    }
}
