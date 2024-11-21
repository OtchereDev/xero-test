namespace refactor_this.Models
{
    public interface IDatabase
    {
        IDbConnectionWrapper GetConnection();
    }
}