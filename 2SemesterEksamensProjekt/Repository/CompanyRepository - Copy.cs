using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using _2SemesterEksamensProjekt.Models;
using Microsoft.Data.SqlClient;

namespace _2SemesterEksamensProjekt.Repository
{
//    public class CompanyRepository : BaseRepository
//    {
//        public List<Company> GetAllCompanies()
//        {
//            return ExecuteSafe(conn =>
//            {
//                var companies = new List<Company>();

//                using var cmd = new SqlCommand(
//                    "SELECT CompanyId, CompanyName FROM dbo.vwSelectAllCompanies;", conn);
//                using var reader = cmd.ExecuteReader();
//                while (reader.Read())
//                {
//                    var company = new Company
//                    {
//                        CompanyId = reader.GetInt32(0),
//                        CompanyName = reader.IsDBNull(1) ? "" : reader.GetString(1)
//                    };
//                    companies.Add(company);
//                }
//                return companies;
//            });
//        }

//        public int SaveNewCompany(Company company)
//        {
//            return ExecuteSafe(conn =>
//            {
//                using (SqlCommand cmd = new SqlCommand("uspCreateCompany", conn))
//                {
//                    cmd.CommandType = CommandType.StoredProcedure;
//                    cmd.Parameters.AddWithValue("@CompanyName", company.CompanyName);
//                    object result = cmd.ExecuteScalar();
//                    return Convert.ToInt32(result);
//                }
//            });
//        }

//        public void DeleteCompany(int companyId)
//        {
//            ExecuteSafe(conn =>
//            {
//                using var cmd = new SqlCommand("uspDeleteCompany", conn);
//                cmd.CommandType = CommandType.StoredProcedure;
                
//                cmd.Parameters.AddWithValue("@CompanyId", companyId);
//                cmd.ExecuteNonQuery();

//                return true;
//            });

//        }
//        public void UpdateCompany(Company company)
//        {
//            ExecuteSafe(conn =>
//            {
//                using var cmd = new SqlCommand("uspUpdateCompany", conn);
//                cmd.CommandType = CommandType.StoredProcedure;

//                cmd.Parameters.AddWithValue("@CompanyId", company.CompanyId);
//                cmd.Parameters.AddWithValue("@CompanyName", company.CompanyName);

//                cmd.ExecuteNonQuery();
//                return true;
//            });
//        }
//    }
}
