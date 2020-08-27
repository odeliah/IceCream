using crudTest.App_Start;
using crudTest.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Web;

namespace crudTest.Controllers
{
    public class IceCreamController : Controller
    {
        DataServices ds;
        

        public IceCreamController()
        {
            ds = DataServices.Instance;           
        }

        public void isAdminLayout()
        {
            if (ds.isAdmin) ControllerContext.RouteData.Values["id"] = "True";
        }

        // GET: IceCream
        public ActionResult Index()
        {
            isAdminLayout();
            return View(ds.getIcecreams()); 
        }

        // GET: IceCream/Details/5
        public ActionResult Details(string id) 
        {
            isAdminLayout();
            var v = ds.getIcecream(id);
            ViewBag.GoogleApi = Defenitions.GOOGLE_MAPS_VIEW + Defenitions.GOOGLE_MAPS_API + "&callback = myMap";
            return View(v);
        }

        [HttpGet]
        public ActionResult Search(FormCollection collection)
        {
            isAdminLayout();
            SearchParams s = new SearchParams();
            s.IceCreams = ds.getIcecreams();
            return View(s);
        }

        [HttpPost]
        public ActionResult Search(SearchParams search)
        {
            isAdminLayout();
            SearchParams s = new SearchParams();
            s.IceCreams = ds.getIcecreams(search);
            return View("Search", s);
        }

        // GET: IceCream/Create
        [HttpGet]
        public ActionResult Create(FormCollection collection)
        {
            isAdminLayout();
            ViewBag.StoreList = ToSelectList(ds.getStores());
            return View();
        }

        // POST: IceCream/Create
        [HttpPost]
        public ActionResult Create(IceCream iceCream)
        {
            isAdminLayout();
            ViewBag.StoreList = ToSelectList(ds.getStores());
            if (iceCream.UsdaID == "0")
            {
                ViewBag.Error = "Usda identifier does not exists or network failure!";
                return View("Create", iceCream);
            }
            if (!ds.storeExists(iceCream.Store))
            {
                ViewBag.Error = "Store does not exists in the system!";
                return View("Create", iceCream);
            }
            if (! ds.insertIcecream( iceCream))
            { 
               ViewBag.Error = "Ice Cream ALready Exist";
                return View("Create", iceCream);
            }
            else
                return RedirectToAction("Create", new IceCream()) ;
        }


        [NonAction]
        public SelectList ToSelectList(List<Store> stores)
        {
            List<SelectListItem> list = new List<SelectListItem>();

            foreach (var item in stores)
            {
                list.Add(new SelectListItem()
                {
                    Text = item.Name.ToString(),
                    Value = item.Name.ToString()
                });
            }
            return new SelectList(list, "Value", "Text");
        }

        [HttpGet]
        // GET: IceCream/Edit/5
        public ActionResult Edit(string id, FormCollection collection)
        {
            isAdminLayout();
            return View();
        }

        // POST: IceCream/Edit/5
        [HttpPost]
        public async Task<ActionResult> Edit(string id, Recommendation recommendation)
        {
            isAdminLayout();
            try
            {
                StoreStruct iceCream = await ds.addRecommendation(id, recommendation);
                ViewBag.GoogleApi = Defenitions.GOOGLE_MAPS_VIEW + Defenitions.GOOGLE_MAPS_API + "&callback = myMap";
                return View("Details", iceCream); 
            }
            catch(Exception e)
            {
                ViewBag.Error = e.Message;
                return View();
            }
            //return View();
        }

        //*************************************************************************************************************

        [HttpPost]
        public ActionResult Login(FormCollection form)
        {
            if (form["email"] == Defenitions.ADMIN_EMAIL && form["password"] == Defenitions.ADMIN_PASSWORD)
            {
                ds.isAdmin = true;
                return Redirect("/indexAdmin.html");
            }
            else
            {
                return RedirectToAction("WrongPswrd");
            }
        }

        public ActionResult WrongPswrd()
        {
            isAdminLayout();
            return View();
        }


        [HttpPost]
        public ActionResult Logout(FormCollection form)
        {
            ds.isAdmin = false;
            return Redirect("/index.html");
        }

        [HttpPost]
        public ActionResult SendEmail(FormCollection form)
        {
            string body = "\nBy staying connected to our outstanding project, you will keep up to date with the news of the ice cream world!" +
                          "\n\nWe are happy to join you in the fan audience!\n\n" + "Riki Zilbershlag & Odeliah Movadat\nCloud Course 2019";
            SendEmail(form["email"], "Hello Ice cream fan!", "So what's new in Ice cream world? \n" + body);
            
            return Redirect("~/index.html");
        }

     
        public static void SendEmail(string emailTo, string subject, string body)
        {
            var client = new SmtpClient(Defenitions.MAIL_HOST, Defenitions.MAIL_PORT)
            {
                Credentials = new NetworkCredential(Defenitions.MAIL_ADDRESS, Defenitions.MAIL_PASSWORD),
                EnableSsl = true
            };

            try
            {
                client.Send(Defenitions.MAIL_ADDRESS, emailTo, subject, body);
            }
            catch (Exception e)
            {
                Console.WriteLine("Problem in sending mail");
            }        
        }
    }
}
