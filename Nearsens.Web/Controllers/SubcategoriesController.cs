using Nearsens.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Nearsens.Web.Controllers
{
    public class SubcategoriesController : ApiController
    {
        SqlSubcategoriesRepository repository = new SqlSubcategoriesRepository();

        // GET: api/Subcategories
        public IEnumerable<string> GetAllCategories()
        {
            return repository.GetAll();
        }
    }
}
