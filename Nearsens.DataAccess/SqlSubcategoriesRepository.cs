using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nearsens.DataAccess
{
    public class SqlSubcategoriesRepository
    {
        string connectionString;

        public SqlSubcategoriesRepository()
            : this("nearsensCS")
        {
        }

        public SqlSubcategoriesRepository(string connectionStringName)
        {
            var cs = ConfigurationManager.ConnectionStrings[connectionStringName];
            if (cs == null)
                throw new ApplicationException(string.Format("ConnectionString '{0}' not found", connectionStringName));
            connectionString = cs.ConnectionString;
        }

        public IEnumerable<string> GetAll()
        {
            List<string> subcategories = new List<string>();
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = @"
SELECT  name
      
FROM    dbo.subcategories
";
                using (var command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            subcategories.Add((string)reader["name"]);
                        }
                    }
                }
            }
            return subcategories;
        }
    }
}
