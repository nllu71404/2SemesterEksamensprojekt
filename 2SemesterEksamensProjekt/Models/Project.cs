using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2SemesterEksamensProjekt.Models
{
    public class Project
    {

        // --Auto-Properties-- 
        public int ProjectId { get; set; }
        public int CompanyId { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }

        //--Constructor--
        public Project(int companyId, string title, string description) 
        {
            CompanyId = companyId;
            Title = title;
            Description = description;
        }

        //--Parameterløs constructor--
        public Project() 
        {

        }
    }
}
