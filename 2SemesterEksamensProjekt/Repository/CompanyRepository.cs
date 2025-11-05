using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _2SemesterEksamensProjekt.Models;
using Microsoft.Data.SqlClient;

namespace _2SemesterEksamensProjekt.Repository
{
    public class CompanyRepository : BaseRepository
    {
        public List<Company> GetAllCompanies()
        {
            return ExecuteSafe(conn =>
            {
                var companies = new List<Company>();
                using var cmd = new Microsoft.Data.SqlClient.SqlCommand(
                    "SELECT CompanyId, CompanyName FROM Company", conn);
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var company = new Company
                    {
                        CompanyId = reader.GetInt32(0),
                        CompanyName = reader.IsDBNull(1) ? "" : reader.GetString(1)
                    };
                    companies.Add(company);
                }
                return companies;
            });
        }

        public void DeleteCompany(Company company)
        {
            using (var connection = GetConnection())
            {
                connection.Open();

                string query = "Delete FROM Company WHERE CompanyId = @CompanyId";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CompanyId", company.CompanyId);
                    command.ExecuteNonQuery();
                }
            }

        }
    }
}
