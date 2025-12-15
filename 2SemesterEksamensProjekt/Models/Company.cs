using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2SemesterEksamensProjekt.Models
{
    public class Company 
    {
  
        //Auto-Properties (Indkapsler også!)
        public string CompanyName { get; set; }
        public int CompanyId { get; set; }


        public Company (string companyName, int companyId)
        {
            CompanyName = companyName;
            CompanyId = companyId;
           
        }

        public Company() //parameterløs constructor
        {

        }
    }
}
