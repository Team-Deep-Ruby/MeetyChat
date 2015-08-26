namespace MeetyChat.Data.Data
{
    using System;
    using System.Collections.Generic;
    using Interfaces;
    using Models;

    public class MeetyChatData : IMeetyChatData
    {
        private readonly IMeetyChatDbContext context;
        private readonly IDictionary<Type, object> repositories;

        public MeetyChatData(IMeetyChatDbContext context)
        {
            this.context = context;
            this.repositories = new Dictionary<Type, object>();
        }

        public MeetyChatData()
            : this(new MeetyChatDbContext())
        {
        }

        public IRepository<ApplicationUser> Users
        {
            get { return this.GetRepository<ApplicationUser>(); }
        }

        public IRepository<Room> Rooms
        {
            get { return this.GetRepository<Room>(); }
        }

        public IRepository<Message> Messages
        {
            get { return this.GetRepository<Message>(); }
        }

        public int SaveChanges()
        {
            return this.context.SaveChanges();
        }

        private IRepository<T> GetRepository<T>() where T : class
        {
            var typeOfModel = typeof (T);

            if (!this.repositories.ContainsKey(typeOfModel))
            {
                var type = typeof (Repository<T>);
                this.repositories.Add(typeOfModel, Activator.CreateInstance(type, this.context));
            }

            return this.repositories[typeOfModel] as IRepository<T>;
        } 
    }
}
