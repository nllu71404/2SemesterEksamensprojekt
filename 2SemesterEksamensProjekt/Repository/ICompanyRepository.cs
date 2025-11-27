using _2SemesterEksamensProjekt.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2SemesterEksamensProjekt.Repository
{
    public interface ICompanyRepository
    {
        List<Company> GetAllCompanies();

        int SaveNewCompany(Company company);

        void DeleteCompany(int companyId);

        void UpdateCompany(Company company);
    }
}
