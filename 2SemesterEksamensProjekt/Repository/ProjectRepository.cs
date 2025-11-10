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
                    SELECT ProjectId, CompanyId, Title, Description, ProjectStatus
                    FROM dbo.Project
                    ORDER BY ProjectId DESC;", conn);

                    using var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        var statusStr = reader.GetString(4);
                        var ok = Enum.TryParse<ProjectStatus>(statusStr, true, out var status);
                        projects.Add(new Project
                        {
                            ProjectId = reader.GetInt32(0),
                            CompanyId = reader.GetInt32(1),
                            Title = reader.GetString(2),
                            Description = reader.IsDBNull(3) ? null : reader.GetString(3),
                            ProjectStatus = ok ? status : ProjectStatus.Created
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
                    using var cmd = new SqlCommand(@"
                    SELECT ProjectId, CompanyId, Title, Description, ProjectStatus
                    FROM dbo.Project
                    WHERE CompanyId = @CompanyId
                    ORDER BY ProjectId DESC;", conn);

                    cmd.Parameters.AddWithValue("@CompanyId", companyId);

                    using var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        var statusStr = reader.GetString(4);
                        var ok = Enum.TryParse<ProjectStatus>(statusStr, true, out var status);
                        projects.Add(new Project
                        {
                            ProjectId = reader.GetInt32(0),
                            CompanyId = reader.GetInt32(1),
                            Title = reader.GetString(2),
                            Description = reader.IsDBNull(3) ? null : reader.GetString(3),
                            ProjectStatus = ok ? status : ProjectStatus.Created
                        });
                    }
                    return projects;
                });
            }

            public Project? GetProjectById(int projectId)
            {
                return ExecuteSafe(conn =>
                {
                    using var cmd = new SqlCommand(@"
                    SELECT ProjectId, CompanyId, Title, Description, ProjectStatus
                    FROM dbo.Project
                    WHERE ProjectId = @ProjectId;", conn);

                    cmd.Parameters.AddWithValue("@ProjectId", projectId);

                    using var reader = cmd.ExecuteReader();
                    if (!reader.Read()) return null;

                    var statusStr = reader.GetString(4);
                    var ok = Enum.TryParse<ProjectStatus>(statusStr, true, out var status);
                    return new Project
                    {
                        ProjectId = reader.GetInt32(0),
                        CompanyId = reader.GetInt32(1),
                        Title = reader.GetString(2),
                        Description = reader.IsDBNull(3) ? null : reader.GetString(3),
                        ProjectStatus = ok ? status : ProjectStatus.Created
                    };
                });
            }

            // Gemme funktion der bruger vores Stored Procedure
            public int SaveNewProject(Project project)
            {
                return ExecuteSafe(conn =>
                {
                    using var cmd = new SqlCommand("spCreateProject", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@CompanyId", project.CompanyId);
                    cmd.Parameters.AddWithValue("@Title", project.Title);
                    cmd.Parameters.AddWithValue("@Description", (object?)project.Description ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@ProjectStatus", project.ProjectStatus.ToString());

                    var newId = cmd.ExecuteScalar();
                    return Convert.ToInt32(newId);
                });
            }

            public void UpdateProjectStatus(int projectId, ProjectStatus newStatus)
            {
                ExecuteSafe(conn =>
                {
                    using var cmd = new SqlCommand("spUpdateProjectStatus", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@ProjectId", projectId);
                    cmd.Parameters.AddWithValue("@ProjectStatus", newStatus.ToString());

                    cmd.ExecuteNonQuery();
                    return true;
                });
            }

            public void EditProject(Project project)
            {
                ExecuteSafe(conn =>
                {
                    using var cmd = new SqlCommand("spEditProject", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@ProjectId", project.ProjectId);
                    cmd.Parameters.AddWithValue("@CompanyId", project.CompanyId);
                    cmd.Parameters.AddWithValue("@Title", project.Title);
                    cmd.Parameters.AddWithValue("@Description", (object?)project.Description ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@ProjectStatus", project.ProjectStatus.ToString());

                    cmd.ExecuteNonQuery();
                    return true;
                });
            }

            public void DeleteProject(int projectId)
            {
                ExecuteSafe(conn =>
                {
                    using var cmd = new SqlCommand("spDeleteProject", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@ProjectId", projectId);

                    cmd.ExecuteNonQuery();
                    return true;
                });
            }
        }
    }
