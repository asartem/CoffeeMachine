namespace Domain.Common.Dal
{
    public interface IDbConnectionProvider
    {
        string ConnectionString { get; }
    }
}