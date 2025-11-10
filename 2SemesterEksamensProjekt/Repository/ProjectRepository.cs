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
                    using var cmd = new SqlCommand(@"
                SELECT ProjectId, CompanyId, Title, Description
                FROM dbo.Project
                WHERE CompanyId = @CompanyId
                ORDER BY ProjectId DESC;", conn);

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

                    return projects ?? new List<Project>(); // sikrer at der aldrig returneres null
                });
            }

            public Project? GetProjectById(int projectId)
                {
                    return ExecuteSafe(conn =>
                    {
                        using var cmd = new SqlCommand(@"
                        SELECT ProjectId, CompanyId, Title, Description
                        FROM dbo.Project
                        WHERE ProjectId = @ProjectId;", conn);

                        cmd.Parameters.AddWithValue("@ProjectId", projectId);

                        using var reader = cmd.ExecuteReader();
                        if (!reader.Read()) return null;

                        var statusStr = reader.GetString(4);
                        return new Project
                        {
                            ProjectId = reader.GetInt32(0),
                            CompanyId = reader.GetInt32(1),
                            Title = reader.GetString(2),
                            Description = reader.IsDBNull(3) ? null : reader.GetString(3),
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

                        var newId = cmd.ExecuteScalar();
                        return Convert.ToInt32(newId);
                    });
                }

            public void UpdateProject(Project project)
            {
                using var connection = GetConnection();
                connection.Open();

                var sql = @"
                    UPDATE Project
                    SET 
                        Title = @Title,
                        Description = @Description
                    WHERE ProjectId = @ProjectId";

                using var command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@Title", project.Title);
                command.Parameters.AddWithValue("@Description", (object?)project.Description ?? DBNull.Value);
                command.Parameters.AddWithValue("@ProjectId", project.ProjectId);

                command.ExecuteNonQuery();
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
