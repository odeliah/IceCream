using crudTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace crudTest.Controllers
{
    public class StoresApiController : ApiController
    {

        // GET: api/IceDetails/5
      
        public List<Store> Get()
        {
            var a = DataServices.Instance;
            var stores = a.getStores().Select(x => x).ToList();
            return stores;
        }

    }
}
