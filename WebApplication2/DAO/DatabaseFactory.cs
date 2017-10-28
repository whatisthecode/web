namespace WebApplication2.DAO
{
    public static class DatabaseFactory
    {
        public static DBContext context;
        public static void Initialize()
        {
            DatabaseFactory.context = new DBContext();
            DatabaseFactory.context.Database.CreateIfNotExists();
            DatabaseFactory.context.Database.Initialize(false);
        }
    }
}