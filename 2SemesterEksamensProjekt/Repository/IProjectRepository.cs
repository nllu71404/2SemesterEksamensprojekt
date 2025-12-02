using _2SemesterEksamensProjekt.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2SemesterEksamensProjekt.Repository
{
    public interface IProjectRepository
    {
        int SaveNewProject(Project project);
        void UpdateProject(Project project);
        void DeleteProject(int projectId);
        List<Project> GetAllProjects();
        List<Project> GetProjectsByCompanyId(int companyId);
    }
}
