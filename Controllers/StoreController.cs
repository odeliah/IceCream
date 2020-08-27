using crudTest.App_Start;
using crudTest.Models;
using MongoDB.Driver.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace crudTest.Controllers
{
    public class StoreController : Controller
    {
        DataServices ds;

        public StoreController()
        {
            ds = DataServices.Instance;
        }
        public void isAdminLayout()
        {
            if (ds.isAdmin) ControllerContext.RouteData.Values["id"] = "True";
        }

        // GET: Store
        public ActionResult Index()
        {
            isAdminLayout();
            return View(ds.getStores());
        }

        // GET: Store/Details/5
        public ActionResult Details(string id)
        {
            isAdminLayout();
            ViewBag.GoogleApi = Defenitions.GOOGLE_MAPS_VIEW + Defenitions.GOOGLE_MAPS_API + "&callback = myMap";
            return View(ds.getStore(id)); 
        }


        // GET: Store/Create
        public ActionResult Create(FormCollection collection)
        {
            isAdminLayout();
            return View();
        }

        // POST: Store/Create
        [HttpPost]
        public ActionResult Create(Store store)
        {
            isAdminLayout();
           
            if(!ds.insertStore(store))
            {
                ViewBag.Error = "Store ALready Exist";
                return View("Create", store);
            }
            return RedirectToAction("Create", new Store());
        }


        public ActionResult MapView()
        {
            isAdminLayout();
            ViewBag.GoogleApi = Defenitions.GOOGLE_MAPS_VIEW + Defenitions.GOOGLE_MAPS_API + "&callback = myMap";
            return View();
        }
    }
}
