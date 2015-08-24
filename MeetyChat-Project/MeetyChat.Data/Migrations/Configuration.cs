namespace MeetyChat.Data.Migrations
{
    using System.Data.Entity.Migrations;
    using Data;

    internal sealed class Configuration : DbMigrationsConfiguration<MeetyChatDbContext>
    {
        public Configuration()
        {
            this.AutomaticMigrationsEnabled = true;
            this.AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(MeetyChatDbContext context)
        {
        }
    }
}
