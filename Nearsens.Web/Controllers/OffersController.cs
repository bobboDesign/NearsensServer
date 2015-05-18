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
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Offers
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Offers/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Offers/5
        public void Delete(int id)
        {
        }
    }
}
