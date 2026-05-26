using Cassandra;

namespace SploinkyAPI.Models
{
    public class User : IDbItem<User>
    {
        public static User FromDBRow(Row row)
        {
            throw new NotImplementedException();
        }

        public User LoadFromDb(Guid Id)
        {
            throw new NotImplementedException();
        }
    }
}
