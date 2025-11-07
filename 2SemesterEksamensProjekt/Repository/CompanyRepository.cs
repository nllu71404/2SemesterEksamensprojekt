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
                "SELECT * FROM vm_SelectAllCompanies", conn);
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
            using var cmd = new SqlCommand("Navnet på vores StoredProcedure", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@CompanyName", company.CompanyName);

            //Her kan man tilføje Id med det samme, men mit hovede tænkte at det giver mening vi bare lader DB om at give Id automatisk
            var newCompanyId = cmd.ExecuteScalar(); //Returnerer der nye ide
            return Convert.ToInt32(newCompanyId);

            //HUSK at StoredProcedure også skal returnere id. 
            //Vi bruger Id'et på listerne, uden at vi ser dem i UI

        });
    }

    public void DeleteCompany(Company company)
    {
        ExecuteSafe(conn =>
        {
            //Standard for connection til SQL (skal altid bruges)
            using var cmd = new SqlCommand("Navnet på vores Delete StoredProcedure", conn);

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

    public void EditCompany(Company company)
    {
        ExecuteSafe(conn =>
        {
            using var cmd = new SqlCommand("Navnet på vores Edit StoredProcedure", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            //Tilføjer de nye værdier til CompanyName og CompanyId
            cmd.Parameters.AddWithValue("@CompanyId", company.CompanyId);
            cmd.Parameters.AddWithValue("@CompanyName", company.CompanyName);

            cmd.ExecuteNonQuery();
            return true;
        });
    }
}
}
