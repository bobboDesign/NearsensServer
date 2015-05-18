using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nearsens.Models;
using System.Data.SqlClient;

namespace Nearsens.DataAccess
{
    public class SqlPlacesRepository
    {
        string connectionString;

        public SqlPlacesRepository()
            : this("nearsensCS")
        {
        }

        public SqlPlacesRepository(string connectionStringName)
        {
            var cs = ConfigurationManager.ConnectionStrings[connectionStringName];
            if (cs == null)
                throw new ApplicationException(string.Format("ConnectionString '{0}' not found", connectionStringName));
            connectionString = cs.ConnectionString;
        }

//        public IEnumerable<GetNearestPlacesQuery> GetNearestPlaces(double lat, double lng)
//        {
//            List<GetNearestPlacesQuery> places = new List<GetNearestPlacesQuery>();
//            using (var connection = new SqlConnection(connectionString))
//            {
//                connection.Open();

//                string query = @"
//        SELECT  id ,
//        		name ,
//        		main_category ,
//        		subcategory ,
//        		lat ,
//        		lng ,
//                icon
//              
//        FROM    dbo.places
//        ";
//                using (var command = new SqlCommand(query, connection))
//                {
//                    using (SqlDataReader reader = command.ExecuteReader())
//                    {
//                        while (reader.Read())
//                        {
//                            GetNearestPlacesQuery place = new GetNearestPlacesQuery();
//                            place.Id = (long)reader["id"];
//                            place.Name = (string)reader["name"];
//                            place.MainCategory = (string)reader["main_category"];
//                            place.Subcategory = (string)reader["subcategory"];
//                            place.Lat = (double)reader["lat"];
//                            place.Lng = (double)reader["lng"];
//                            place.Icon = reader["icon"] == DBNull.Value ? (string)null : (string)reader["icon"];

//                            places.Add(place);
//                        }
//                    }
//                }
//            }
//            return places.OrderBy(xx => Utilities.GeoUtilities.CalculateDistance(xx.Lat, lat, xx.Lng, lng));
//        }

        public GetPlaceQuery GetPlaceById(long id)
        {
            GetPlaceQuery place = new GetPlaceQuery();
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = @"
SELECT  id ,
		name ,
        description ,
		main_category ,
		subcategory ,
		lat ,
		lng ,
        icon
      
FROM    dbo.places
WHERE id = @id
";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add(new SqlParameter("@id", id));
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            place.Id = (long)reader["id"];
                            place.Name = (string)reader["name"];
                            place.Description = (string)reader["description"];
                            place.MainCategory = (string)reader["main_category"];
                            place.Subcategory = (string)reader["subcategory"];
                            place.Lat = (double)reader["lat"];
                            place.Lng = (double)reader["lng"];
                            place.Icon = reader["icon"] == DBNull.Value ? (string)null : (string)reader["icon"];

                        }
                    }
                }
            }
            return place;
        }

        public IEnumerable<GetPlacesByUserIdQuery> GetPlacesByUserId(string id)
        {
            List<GetPlacesByUserIdQuery> places = new List<GetPlacesByUserIdQuery>();
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = @"
SELECT  id ,
		name ,
        icon
      
FROM    dbo.places
WHERE user_id = @id
";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add(new SqlParameter("@id", id));
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            GetPlacesByUserIdQuery place = new GetPlacesByUserIdQuery();
                            place.Id = (long)reader["id"];
                            place.Name = (string)reader["name"];
                            place.Icon = reader["icon"] == DBNull.Value ? (string)null : (string)reader["icon"];
                            places.Add(place);
                        }
                    }
                }
            }
            return places;
        }



        public IEnumerable<GetNearestPlacesQuery> GetNearestPlacesWithFilters(double lat, double lng, int? distanceLimit, string category, string subcategory)
        {
            List<GetNearestPlacesQuery> places = new List<GetNearestPlacesQuery>();
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = @"
SELECT  id ,
		name ,
		main_category ,
		subcategory ,
		lat ,
		lng ,
        icon
      
FROM    dbo.places
";
                query = BuildWhereClause(query, category, subcategory);
                using (var command = new SqlCommand(query, connection))
                {
                    if (category != null) command.Parameters.Add(new SqlParameter("@category", category));
                    if (subcategory != null) command.Parameters.Add(new SqlParameter("@subcategory", subcategory));
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            GetNearestPlacesQuery place = new GetNearestPlacesQuery();
                            place.Id = (long)reader["id"];
                            place.Name = (string)reader["name"];
                            place.MainCategory = (string)reader["main_category"];
                            place.Subcategory = (string)reader["subcategory"];
                            place.Lat = (double)reader["lat"];
                            place.Lng = (double)reader["lng"];
                            place.Icon = reader["icon"] == DBNull.Value ? (string)null : (string)reader["icon"];

                            places.Add(place);
                        }
                    }
                }
            }

            var orderedList = places.OrderBy(xx => Utilities.GeoUtilities.CalculateDistance(xx.Lat, lat, xx.Lng, lng));
            if (distanceLimit != null)
                return orderedList.Where(xx => Utilities.GeoUtilities.CalculateDistance(xx.Lat, lat, xx.Lng, lng) < distanceLimit);
            return orderedList;
        }

        private string BuildWhereClause(string query, string category, string subcategory)
        {
            if (category == null && subcategory == null)
                return query;
            query += " WHERE ";
            if (category != null)
                query += "main_category = @category AND ";
            if (subcategory != null)
                query += "subcategory = @subcategory AND ";
            
            return query.Remove(query.LastIndexOf("AND"));

        }
    }
}
