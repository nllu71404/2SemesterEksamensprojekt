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
        //                                                                          gammel string til Azure SQL
        //protected readonly string _connectionString =
        //new Microsoft.Data.SqlClient.SqlConnectionStringBuilder
        //{
        //    DataSource = "tcp:team5semester2.database.windows.net,1433",
        //    InitialCatalog = "Team5Semester2",        // <-- kun DB-navn
        //    UserID = "adminteam5",
        //    Password = "team5Admin",
        //    Encrypt = true,
        //    TrustServerCertificate = false,
        //    MultipleActiveResultSets = false,
        //    ConnectTimeout = 30
        //}.ConnectionString;

        //                                                                     Lauras connection string
        protected readonly string _connectionString =
        new SqlConnectionStringBuilder
        {
            DataSource = @"DESKTOP-20JEDR6\SQLEXPRESS",
            InitialCatalog = "StackhouseDB",
            IntegratedSecurity = true,           // Windows login
            TrustServerCertificate = true,       // undgår certifikatfejl
            Encrypt = false,                     // Express kræver det ikke
            MultipleActiveResultSets = true,
            ConnectTimeout = 30
        }.ConnectionString;

        //                                                                    Mias connection string
        /*
        protected readonly string _connectionString =
        new SqlConnectionStringBuilder
        {
            DataSource = "localhost",
            InitialCatalog = "StackhouseDB",
            IntegratedSecurity = true,           // Windows login
            TrustServerCertificate = true,       // undgår certifikatfejl
            Encrypt = false,                     // Express kræver det ikke
            MultipleActiveResultSets = true,
            ConnectTimeout = 30
        }.ConnectionString;
        */

        //                                                                      Melindas connection string

        //protected readonly string _connectionString = new SqlConnectionStringBuilder
        //{
        //    DataSource = @"(localdb)\MSSQLLocalDB",
        //    InitialCatalog = "StackhouseDB",
        //    IntegratedSecurity = true,           // Windows login
        //    TrustServerCertificate = true,       // undgår certifikatfejl
        //    Encrypt = false,                     // Express kræver det ikke
        //    MultipleActiveResultSets = true,
        //    ConnectTimeout = 30
        //}.ConnectionString;


        //                                                                          Ninnas connection string
        //protected readonly string _connectionString =
        //new SqlConnectionStringBuilder
        //{
        //    DataSource = @"(localdb)\MSSQLLocalDB",
        //    InitialCatalog = "StackhouseDB",
        //    IntegratedSecurity = true,
        //    TrustServerCertificate = true,
        //    Encrypt = false,
        //    MultipleActiveResultSets = true,
        //    ConnectTimeout = 30
        //}.ConnectionString;

        //                                                                        Rebeccas connection string     
        //protected readonly string _connectionString =
        //new SqlConnectionStringBuilder
        //{
        //    DataSource = "localhost",
        //    InitialCatalog = "StackhouseDB",
        //    IntegratedSecurity = true,
        //    TrustServerCertificate = true,
        //    Encrypt = false,
        //    MultipleActiveResultSets = true,
        //    ConnectTimeout = 30
        //}.ConnectionString;


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
