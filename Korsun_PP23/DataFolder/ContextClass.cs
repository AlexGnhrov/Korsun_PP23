using System.Data.Entity;

namespace Korsun_PP23.DataFolder
{
    public partial class DBEntities : DbContext
    {
        private static DBEntities context;

        public static DBEntities GetContext()
        {
            if (context == null)
            {
                context = new DBEntities();
            }

            return context;
        }

        public static void NullContext()
        {
            context = null;
        }
    }
}
