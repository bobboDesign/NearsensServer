using Nearsens.DataAccess;
using Nearsens.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Nearsens.Web.Controllers
{
    public class OffersController : ApiController
    {
        SqlOffersRepository repository = new SqlOffersRepository();

        // GET: api/Offers
        public IEnumerable<GetNearestOffersQuery> Get(double lat, double lng, string category = null, string subcategory = null, int? distanceLimit = null)
        {
            return repository.GetNearestOffers(lat, lng, category, subcategory, distanceLimit);
        }

        // GET: api/Offers/5
        public GetOfferQuery Get(long id)
        {
            return repository.GetOfferById(id);
        }

        // POST: api/Offers
        public long Post([FromBody]Offer offer)
        {
            return repository.InsertOffer(offer);
        }

        // PUT: api/Offers/5
        public void Put([FromBody]Offer offer)
        {
            repository.UpdateOffer(offer);
        }

        // DELETE: api/Offers/5
        public void Delete(long id)
        {
            repository.DeleteOffer(id);
        }
    }
}
