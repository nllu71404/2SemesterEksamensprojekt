using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2SemesterEksamensProjekt.Models
{
    public class Project
    {

        // Auto-Properties (Indkapsler også!) 
        public int ProjectId { get; set; }
        public int CompanyId { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }

        public Project(int companyId, string title, string? description) //Description behøver ikke at være med i constructor, tror jeg, da den ikke behøver at være udfyldt?:-)
        {
            CompanyId = companyId;
            Title = title;
            Description = description;
        }

        public Project() //parameterløs constructor
        {

        }
    }
}
