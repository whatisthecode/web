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

            context.Database.ExecuteSqlCommand("Update token set is_login = 0 where is_login = 1");
        }
    }
}