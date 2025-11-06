using System.Configuration;
using System.Data;
using System.Linq.Expressions;
using System.Windows;
using Microsoft.Data.SqlClient;

namespace _2SemesterEksamensProjekt
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            try
            {
                using (var conn = new SqlConnection("Server=tcp:team5semester2.database.windows.net,1433;" +
                         "Initial Catalog=team5semester2.database.windows.net;" +
                         "Persist Security Info=False;" +
                         "User ID=adminteam5;" +
                         "Password=team5Admin;" +
                         "MultipleActiveResultSets=False;" +
                         "Encrypt=True;" +
                         "TrustServerCertificate=False;" +
                         "Connection Timeout=30;"))
                {
                    conn.Open();
                    MessageBox.Show("Forbindelsen virker!");
                }
            } catch (Exception ex)
            {
                MessageBox.Show($"Kunne ikke forbinde til databasen: {ex.Message}");
            }
        }
    }

}
