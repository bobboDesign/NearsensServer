using Nearsens.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nearsens.DataAccess
{
    public class SqlOffersRepository
    {

        
        string connectionString;

        public SqlOffersRepository()
            : this("nearsensCS")
        {
        }

        public SqlOffersRepository(string connectionStringName)
        {
            var cs = ConfigurationManager.ConnectionStrings[connectionStringName];
            if (cs == null)
                throw new ApplicationException(string.Format("ConnectionString '{0}' not found", connectionStringName));
            connectionString = cs.ConnectionString;
        }
        public IEnumerable<GetOffersByPlaceIdQuery> GetOfferByPlaceId(long id)
        {
            List<GetOffersByPlaceIdQuery> offers = new List<GetOffersByPlaceIdQuery>();

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = @"
SELECT  id ,
		icon ,
		expiration_date ,
		start_date ,
        title
FROM    dbo.offers
WHERE id_place = @id
";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add(new SqlParameter("@id", id));
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            GetOffersByPlaceIdQuery offer = new GetOffersByPlaceIdQuery();

                            offer.Id = (long)reader["id"];
                            offer.Title = (string)reader["title"];
                            offer.ExpirationDate = (DateTime)reader["expiration_date"];
                            offer.StartDate = (DateTime)reader["start_date"];
                            offer.Icon = reader["icon"] == DBNull.Value ? (string)null : (string)reader["icon"];
                           
                            offers.Add(offer);
                        }
                    }
                }
            }
            return offers;
        }

        public GetOfferQuery GetOfferDetail(long id)
        {

            GetOfferQuery offer = new GetOfferQuery();

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = @"
SELECT  id ,
		id_place ,
        description ,
		icon ,
		expiration_date ,
		start_date ,
		link ,
        previous_price,
        price,
        title
FROM    dbo.offers
WHERE id = @id
";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add(new SqlParameter("@id", id));
                    using (SqlDataReader reader = command.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            offer.Id = (long)reader["id"];
                            offer.Title = (string)reader["title"];
                            offer.Description = (string)reader["description"];
                            offer.ExpirationDate = (DateTime)reader["expiration_date"];
                            offer.StartDate = (DateTime)reader["start_date"];
                            offer.PreviousPrice = (double)reader["previous_price"];
                            offer.Price = (double)reader["price"];
                            offer.Link = reader["link"] == DBNull.Value ? (string)null : (string)reader["link"];
                            offer.Icon = reader["icon"] == DBNull.Value ? (string)null : (string)reader["icon"];
                        }
                    }
                }
            }
            return offer;
        }

        public IEnumerable<GetNearestOffersQuery> GetNearestOffers(double lat, double lng, string category, string subcategory, int? distanceLimit)
        {
            List<GetNearestOffersQuery> offers = new List<GetNearestOffersQuery>();

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = @"
SELECT  dbo.offers.id ,
		title ,
		price ,
		previous_price ,
        dbo.offers.icon ,
        name ,
        lat ,
        lng
FROM    dbo.offers, dbo.places
WHERE dbo.offers.id_place = dbo.places.id
";
                query = BuildWhereClause(query, category, subcategory);
                using (var command = new SqlCommand(query, connection))
                {
                    if (category != null)
                        command.Parameters.Add(new SqlParameter("@category", category));
                    if (subcategory != null)
                        command.Parameters.Add(new SqlParameter("@subcategory", subcategory));
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            GetNearestOffersQuery offer = new GetNearestOffersQuery();

                            offer.Id = (long)reader["id"];
                            offer.Title = (string)reader["title"];
                            offer.Price = (double)reader["price"];
                            offer.PreviousPrice = (double)reader["previous_price"];
                            offer.Icon = reader["icon"] == DBNull.Value ? (string)null : (string)reader["icon"];
                            offer.PlaceName = (string)reader["name"];
                            offer.PlaceLat = (double)reader["lat"];
                            offer.PlaceLng = (double)reader["lng"];

                            offers.Add(offer);
                        }
                    }
                }
            }

            var orderedList = offers.OrderBy(xx => Utilities.GeoUtilities.CalculateDistance(xx.PlaceLat, lat, xx.PlaceLng, lng));
            if (distanceLimit != null)
                return orderedList.Where(xx => Utilities.GeoUtilities.CalculateDistance(xx.PlaceLat, lat, xx.PlaceLng, lng) < distanceLimit);
            return orderedList;
        }

        private string BuildWhereClause(string query, string category, string subcategory)
        {
            if (category == null && subcategory == null)
                return query;
            query += " AND ";
            if (category != null)
                query += "main_category = @category AND ";
            if (subcategory != null)
                query += "subcategory = @subcategory AND ";

            return query.Remove(query.LastIndexOf("AND"));
        }
    }
}
