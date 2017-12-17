namespace WebApplication2.DAO
{
    public static class DatabaseFactory
    {
        public static void Initialize()
        {
            using (DBContext context = new DBContext())
            {
                context.Database.CreateIfNotExists();
                context.Database.Initialize(false);
                context.Database.ExecuteSqlCommand("Update token set is_login = 0 where is_login = 1");
            }
        }
    }
}