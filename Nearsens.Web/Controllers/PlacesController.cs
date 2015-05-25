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
using System.IO;

namespace Nearsens.Web.Controllers
{
    public class PlacesController : ApiController
    {
        SqlPlacesRepository placesRepository = new SqlPlacesRepository();
        SqlOffersRepository offersRepository = new SqlOffersRepository();

        // GET: api/Places
        //public IEnumerable<GetNearestPlacesQuery> GetNearestPlaces(double lat, double lng)
        //{
        //    return repository.GetNearestPlaces(lat, lng);
        //}

        public IEnumerable<GetNearestPlacesQuery> GetNearestPlacesWithFilters(double lat, double lng, int? distanceLimit = null, string category = null, string subcategory = null)
        {
            return placesRepository.GetNearestPlacesWithFilters(lat, lng, distanceLimit, category, subcategory);
        }

        // GET: api/Places/5
        public GetPlaceQuery Get(long id)
        {
            return placesRepository.GetPlaceById(id);
        }

        [Authorize]
        public IEnumerable<GetPlacesByUserIdQuery> GetPlacesByUserId()
        {
            var userId = HttpContext.Current.User.Identity.GetUserId();
            return placesRepository.GetPlacesByUserId(userId);
        }

        // POST: api/Places
        [Authorize]
        public long Post([FromBody]Place place)
        {
            place.UserId = HttpContext.Current.User.Identity.GetUserId();
            return placesRepository.InsertPlace(place);
        }

        // PUT: api/Places/5
        [Authorize]
        public void Put(Place place)
        {
            placesRepository.UpdatePlace(place);
        }

        // DELETE: api/Places/5
        [Authorize]
        public void Delete(long id)
        {
            var userId = HttpContext.Current.User.Identity.GetUserId();
            string pathToDelete = HttpContext.Current.Server.MapPath("~/Images/" + userId + "/" + id);
            offersRepository.DeleteOffersByPlaceId(id);
            placesRepository.DeletePlace(id);
            if (Directory.Exists(pathToDelete))
                Directory.Delete(pathToDelete, true);
        }
    }
}
