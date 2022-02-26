using System;

namespace Domain.Common.Dal
{
    public class DbConnectionProvider : IDbConnectionProvider
    {

        public DbConnectionProvider(string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentNullException(nameof(connectionString));
            }

            ConnectionString = connectionString;
        }

        public string ConnectionString { get; }
    }
}