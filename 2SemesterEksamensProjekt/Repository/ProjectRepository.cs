using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _2SemesterEksamensProjekt.Models;

namespace _2SemesterEksamensProjekt.Repository
{
    public class ProjectRepository : BaseRepository
    {
        /*
        public List<Project> GetAllProjects()
        {
            /*
            return ExecuteSafe(conn =>
            {
                var projects = new List<Project>();
                using var cmd = new Microsoft.Data.SqlClient.SqlCommand(
                    "SELECT ProjectId, ProjectTitle, ProjectStatys, Company FROM Project", conn);
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var project = new Project
                    {
                        ProjectId = reader.GetInt32(0),
                        ProjectTitle = reader.IsDBNull(1) ? "" : reader.GetString(1),
                        ProjectStatus = reader.GetString(2),
                        Company = reader.GetString(3),
                    };
                    projects.Add(project);
                }
                return projects;
            }) ?? new List<Project>();
            
        }
        */
    }
}
