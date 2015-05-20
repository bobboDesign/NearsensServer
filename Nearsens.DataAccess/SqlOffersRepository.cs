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
        discount,
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
                            offer.Discount = (int)reader["discount"];
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
		discount ,
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
                            offer.Discount = (int)reader["discount"];
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

        public void InsertOffer(Offer offer)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = @"
INSERT INTO [dbo].[offers]
		   ([title]
		   ,[description]
		   ,[start_date]
		   ,[expiration_date]
           ,[price]
           ,[discount]
           ,[id_place])
	 VALUES
		   (@title
		   ,@description
		   ,@start_date
		   ,@expiration_date
		   ,@price
           ,@discount
           ,@id_place)";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add(new SqlParameter("@title", offer.Title));
                    command.Parameters.Add(new SqlParameter("@description", offer.Description));
                    command.Parameters.Add(new SqlParameter("@start_date", offer.StartDate));
                    command.Parameters.Add(new SqlParameter("@expiration_date", offer.ExpirationDate));
                    command.Parameters.Add(new SqlParameter("@price", offer.Price));
                    command.Parameters.Add(new SqlParameter("@discount", offer.Discount));
                    command.Parameters.Add(new SqlParameter("@id_place", offer.IdPlace));

                    int count = command.ExecuteNonQuery();
                }
            }

        }
        public void DeleteOffer(long id)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = @"
DELETE FROM [dbo].[offers]
 WHERE id = @id";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add(new SqlParameter("@id", id));

                    int count = command.ExecuteNonQuery();
                }
            }
        }

        public void UpdateOffer(Offer offer)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = @"
UPDATE [dbo].[offers]
   SET [description] = @description
	  ,[title] = @title
	  ,[price] = @price
	  ,[discount] = @discount
	  ,[start_date] = @start_date
	  ,[expiration_date] = @expiration_date
 WHERE id = @id
";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add(new SqlParameter("@id", offer.Id));
                    command.Parameters.Add(new SqlParameter("@description", offer.Description));
                    command.Parameters.Add(new SqlParameter("@title", offer.Title));
                    command.Parameters.Add(new SqlParameter("@price", offer.Price));
                    command.Parameters.Add(new SqlParameter("@discount", offer.Discount));
                    command.Parameters.Add(new SqlParameter("@start_date", offer.StartDate));
                    command.Parameters.Add(new SqlParameter("@expiration_date", offer.ExpirationDate));
                    int count = command.ExecuteNonQuery();
                }
            }
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
