using Nearsens.DataAccess;
using Nearsens.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Microsoft.AspNet.Identity;

namespace Nearsens.Web.Controllers
{
    public class PlacesController : ApiController
    {
        SqlPlacesRepository repository = new SqlPlacesRepository();

        // GET: api/Places
        //public IEnumerable<GetNearestPlacesQuery> GetNearestPlaces(double lat, double lng)
        //{
        //    return repository.GetNearestPlaces(lat, lng);
        //}

        public IEnumerable<GetNearestPlacesQuery> GetNearestPlacesWithFilters(double lat, double lng, int? distanceLimit = null, string category = null, string subcategory = null)
        {
            return repository.GetNearestPlacesWithFilters(lat, lng, distanceLimit, category, subcategory);
        }

        // GET: api/Places/5
        public GetPlaceQuery Get(long id)
        {
            return repository.GetPlaceById(id);
        }

        [Authorize]
        public IEnumerable<GetPlacesByUserIdQuery> GetPlacesByUserId()
        {
            var userId = HttpContext.Current.User.Identity.GetUserId();
            return repository.GetPlacesByUserId(userId);
        }

        // POST: api/Places
        [Authorize]
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Places/5
        [Authorize]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Places/5
        [Authorize]
        public void Delete(int id)
        {
        }
    }
}
