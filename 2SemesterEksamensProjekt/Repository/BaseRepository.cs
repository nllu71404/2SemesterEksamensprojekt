using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace _2SemesterEksamensProjekt.Repository
{
    public abstract class BaseRepository
    {
        protected readonly string _connectionString;

        //Opretter en ny sqlConnection
        protected SqlConnection GetConnection() => new SqlConnection(_connectionString);

        // Metode til at udføre SQL-kode sikkert, oprette forbindelse og lukke forbindelsen igen
        protected T ExecuteSafe<T>(Func<SqlConnection, T> action)
        {
            try
            {
                using var conn = GetConnection();
                conn.Open();
                return action(conn);
            }
            catch
            {
                return default!;
            }
        }
    }
}
