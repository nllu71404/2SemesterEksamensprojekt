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
    public class CompanyRepository : BaseRepository
    {
        public List<Company> GetAllCompanies()
    {
        return ExecuteSafe(conn =>
        {
            var companies = new List<Company>();
            using var cmd = new Microsoft.Data.SqlClient.SqlCommand(
                "SELECT * FROM vwSelectAllCompanies", conn);
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

    public int SaveNewCompany(Company company)
    {
        return ExecuteSafe(conn =>
        {
            using (SqlCommand cmd = new SqlCommand("uspCreateCompany", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CompanyName", company.CompanyName);

                object result = cmd.ExecuteScalar();
                return Convert.ToInt32(result);
            }
            //using var cmd = new SqlCommand("dbo.uspInsertCompany", conn);
            //cmd.CommandType = CommandType.StoredProcedure;

            //var newCompanyId = cmd.ExecuteScalar(); //Returnerer der nye ide
            //return Convert.ToInt32(newCompanyId);

        });
    }

    public void DeleteCompany(Company company)
    {
        ExecuteSafe(conn =>
        {
            //Standard for connection til SQL (skal altid bruges)
            using var cmd = new SqlCommand("uspDeleteCompany", conn);

            // Fortæller systemet at det er StoredProcedure vi kalder (skal altid bruges) 
            cmd.CommandType = CommandType.StoredProcedure;

            //Standard måde til at tøje params (vores parameter, og property)
            cmd.Parameters.AddWithValue("@CompanyId", company.CompanyId);

            //Denne ændres baseret på hvilken operation Delete/Update/Insert
            cmd.ExecuteNonQuery();

            //Standard return for ExecuteSafe
            return true;
        });

    }


        public void UpdateCompany(Company company)
        {
            using var connection = GetConnection();
            connection.Open();

            using var command = new SqlCommand("uspUpdateCompany", connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@CompanyId", company.CompanyId);
            command.Parameters.AddWithValue("@CompanyName", company.CompanyName);

            command.ExecuteNonQuery();
        }
    }
}

