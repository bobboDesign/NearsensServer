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
                            offer.ExpirationDate = (DateTime)reader["expiration_date"];
                            offer.StartDate = (DateTime)reader["start_date"];
                            offer.Icon = reader["icon"] == DBNull.Value ? (string)null : (string)reader["icon"];
                            offer.Title =(string)reader["title"];
                           
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
                            offer.Description = (string)reader["description"];
                            offer.ExpirationDate = (DateTime)reader["expiration_date"];
                            offer.StartDate = (DateTime)reader["start_date"];
                            offer.PreviousPrice = (double)reader["previous_price"];
                            offer.Price = (double)reader["price"];
                            offer.Link = reader["link"] == DBNull.Value ? (string)null : (string)reader["link"];
                            offer.Icon = reader["icon"] == DBNull.Value ? (string)null : (string)reader["icon"];
                            offer.Title = (string)reader["title"];
                        }
                    }
                }
            }
            return offer;
        }
    }
}
