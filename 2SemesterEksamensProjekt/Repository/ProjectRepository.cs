using _2SemesterEksamensProjekt.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2SemesterEksamensProjekt.Repository
{
    public class ProjectRepository : BaseRepository
    {
        public List<Project> GetAllProjects()
        {
            return ExecuteSafe(conn =>
            {
                    var projects = new List<Project>();
                    using var cmd = new SqlCommand(@"
                    SELECT ProjectId, CompanyId, Title, Description
                    FROM dbo.Project
                    ORDER BY ProjectId DESC;", conn);

                    using var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        var statusStr = reader.GetString(4);
                        projects.Add(new Project
                        {
                            ProjectId = reader.GetInt32(0),
                            CompanyId = reader.GetInt32(1),
                            Title = reader.GetString(2),
                            Description = reader.IsDBNull(3) ? null : reader.GetString(3),
                        });
                    }
                    return projects;
            });
        }

        public List<Project> GetProjectsByCompanyId(int companyId)
        {
                return ExecuteSafe(conn =>
                {
                    var projects = new List<Project>();
                    using var cmd = new SqlCommand("uspGetProjectsByCompanyId", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CompanyId", companyId);

                    using var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        projects.Add(new Project
                        {
                            ProjectId = reader.GetInt32(0),
                            CompanyId = reader.GetInt32(1),
                            Title = reader.GetString(2),
                            Description = reader.IsDBNull(3) ? null : reader.GetString(3)
                        });
                    }

                    return projects;
                });
        }
        public int SaveNewProject(Project project)
        {
            return ExecuteSafe(conn =>
            {
                using var cmd = new SqlCommand("uspCreateProject", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@CompanyId", project.CompanyId);
                cmd.Parameters.AddWithValue("@Title", project.Title);
                cmd.Parameters.AddWithValue("@Description", (object?)project.Description ?? DBNull.Value);

                var newId = cmd.ExecuteScalar();
                return Convert.ToInt32(newId);
            });
        }

        public void UpdateProject(Project project)
        {
            ExecuteSafe(conn =>
            {
                using var cmd = new SqlCommand("uspUpdateProject", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ProjectId", project.ProjectId);
                cmd.Parameters.AddWithValue("@CompanyId", project.CompanyId);
                cmd.Parameters.AddWithValue("@Title", project.Title);
                cmd.Parameters.AddWithValue("@Description", (object?)project.Description ?? DBNull.Value);

                cmd.ExecuteNonQuery();
                return true; 
            });
        }
        public void DeleteProject(int projectId)
        {
            ExecuteSafe(conn =>
            {
                using var cmd = new SqlCommand("uspDeleteProject", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ProjectId", projectId);

                cmd.ExecuteNonQuery();
                return true; 
            });
        }
    }
    }
