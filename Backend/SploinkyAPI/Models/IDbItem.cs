namespace SploinkyAPI.Models
{
    public interface IDbItem<T> where T : IDbItem<T> 
    {
        public T LoadFromDb(Guid Id);

        public static abstract T FromDBRow(Cassandra.Row row);
    }
}
