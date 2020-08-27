using crudTest.App_Start;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Driver.Linq;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;

namespace crudTest.Models
{
    public class DataServices
    {
        private static DataServices instance;
        
        private MongoContext _dbContext;

        public bool isAdmin;

        private DataServices()
        {
            _dbContext = new MongoContext();

            isAdmin = false;
        }

        public static DataServices Instance
        {
            get
            {
                if (instance == null)
                    instance = new DataServices();
                return instance;
            }
        }

        #region Icecream
        public List<IceCream> getIcecreams()
        {
            var list = _dbContext._database.GetCollection<IceCream>("iceCreams").FindAll().ToList();
            list.Sort((a, b) => getAverage(a).CompareTo(getAverage(b)));
            list.Reverse();
            return list;
        }

        

        public List<IceCream> getIcecreams(Search search)
        {
            List<IceCream> iceCreams = _dbContext._database.GetCollection<IceCream>("iceCreams").AsQueryable().ToList();
            // field of usda Json
            if (search.Description != null)
            {
                iceCreams = iceCreams.Where(r => r.Name.ToLower().Contains(search.Description.ToLower())).ToList();
            }

            if (search.Energy != null)
            {
                iceCreams = iceCreams.Where(r => r.Energy <= search.Energy).ToList();

            }

            if (search.Fats != null)
            {
                iceCreams = iceCreams.Where(r => r.Fats <= search.Fats).ToList();

            }

            if (search.Protein != null)
            {
                iceCreams = iceCreams.Where(r => r.Protein <= search.Protein).ToList();

            }
            if (search.Rank != null)
            {
                iceCreams = iceCreams.Where(r => getAverage(r) >= search.Rank).ToList();
            }
            return iceCreams;
        }


        public List<IceCream> getIcecreams(SearchParams search)
        {
            List<IceCream> iceCreams = _dbContext._database.GetCollection<IceCream>("iceCreams").AsQueryable().ToList();
            // field of usda Json
            if (search.Description != null)
            {
                iceCreams = iceCreams.Where(r => r.Name.ToLower().Contains(search.Description.ToLower())).ToList();
            }

            if (search.Energy != null)
            {
                iceCreams = iceCreams.Where(r => r.Energy <= search.Energy).ToList();

            }

            if (search.Fats != null)
            {
                iceCreams = iceCreams.Where(r => r.Fats <= search.Fats).ToList();

            }

            if (search.Protein != null)
            {
                iceCreams = iceCreams.Where(r => r.Protein <= search.Protein).ToList();

            }
            if (search.Rank != null)
            {
                iceCreams = iceCreams.Where(r => getAverage(r) >= search.Rank).ToList();
            }
            return iceCreams;
        }



        private int getAverage(IceCream iceCream)
        {
            var temp = 0;
            if (iceCream.Recommendations != null)
            {
                foreach (var Rec in iceCream.Recommendations)
                { temp += Rec.Rank; }
                temp = temp / iceCream.Recommendations.Count();
            }
            return temp;
        }


        public StoreStruct getIcecream(string id)
        {
            var iceId = Query<IceCream>.EQ(p => p.ID, new MongoDB.Bson.ObjectId(id));
            StoreStruct ice = new StoreStruct();

            var iceCream = _dbContext._database.GetCollection<IceCream>("iceCreams").FindOne(iceId);
            ice.IceCreams.Add(iceCream);

            ice.Store = _dbContext._database.GetCollection<Store>("stores").AsQueryable().Where(r => r.Name.ToLower() == iceCream.Store.ToLower()).FirstOrDefault();
            return ice;
        }

        public bool insertIcecream(IceCream iceCream)
        {
            var document = _dbContext._database.GetCollection<IceCream>("iceCreams");
            var query = Query.And(Query.EQ("Name", iceCream.Name), Query.EQ("Store", iceCream.Store));

            var count = document.FindAs<IceCream>(query).Count();

            if (count == 0)
            {
                document.Insert(iceCream);

                return true;
            }
            return false;
        }

        public async Task<StoreStruct> addRecommendation(string id, Recommendation recommendation)
        {
            var iceObjectId = Query<IceCream>.EQ(p => p.ID, new ObjectId(id));

            var iceCream = _dbContext._database.GetCollection<IceCream>("iceCreams").FindOne(iceObjectId);
            //iceCream.ID = new ObjectId(id);

            StoreStruct ice = new StoreStruct();

            ice.Store = _dbContext._database.GetCollection<Store>("stores").AsQueryable().Where(r => r.Name.ToLower() == iceCream.Store.ToLower()).FirstOrDefault();

            if (iceCream.Recommendations == null)
                iceCream.Recommendations = new List<Recommendation>();

            //****************************************************************************************************
            #pragma warning disable CS4014
            bool task = await RunAsync(recommendation.ImageUrl);
            #pragma warning disable CS4014

            if (!task)
            {
                throw new Exception("The Picture has nothing to do with ice cream!");
            }
            //****************************************************************************************************
           iceCream.Recommendations.Add(recommendation);

            // Document Collections  
            var collection = _dbContext._database.GetCollection<IceCream>("iceCreams");
            // Document Update which need Id and Data to Update  
            var result = collection.Update(iceObjectId, Update.Replace(iceCream), UpdateFlags.None);

            ice.IceCreams.Add(iceCream);
            return ice;
        }

        #endregion

        #region Store

        public List<Store> getStores()
        {
            return _dbContext._database.GetCollection<Store>("stores").FindAll().ToList();
        }

        public bool storeExists(string storeName)
        {
            return (_dbContext._database.GetCollection<Store>("stores").AsQueryable().Where(r => r.Name.ToLower() == storeName.ToLower()).ToList().Count() != 0);
        }

        public StoreStruct getStore(string id)
        {
            var storeId = Query<Store>.EQ(p => p.ID, new MongoDB.Bson.ObjectId(id));
            Store storeDetails = _dbContext._database.GetCollection<Store>("stores").FindOne(storeId);

            var result = _dbContext._database.GetCollection<IceCream>("iceCreams").AsQueryable().Where(r => r.Store.ToLower() == storeDetails.Name.ToLower()).ToList();

            StoreStruct store = new StoreStruct()
            { Store = storeDetails, IceCreams = result };
            return store;
        }

        public bool isValidAddress(Store store)
        {
                string address = store.Address;

                var url = Defenitions.GOOGLE_MAPS_URL + Defenitions.GOOGLE_MAPS_API + "&q=" + HttpContext.Current.Server.UrlEncode(address);

                var request = WebRequest.Create(url);
                var response = (HttpWebResponse)request.GetResponse();

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return false;
                }
       
            return true;

        }

        public bool insertStore(Store store)
        {
            var document = _dbContext._database.GetCollection<Store>("stores");
            var query = Query.And(Query.EQ("Name".ToLower(), store.Name.ToLower()), Query.EQ("Address", store.Address));
            var count = document.FindAs<Store>(query).Count();

            var count2 = document.AsQueryable().Where(r => r.Name.ToLower() == store.Name.ToLower() && r.Address.ToLower() == store.Address.ToLower()).ToList().Count();


            if (count2 == 0)
            {
                document.Insert(store);
                return true;
            }
            return false;
        }

        #endregion

        static async Task<bool> RunAsync(string imageUrl)
        {
            string basicAuthValue = System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(String.Format("{0}:{1}", 
                                                                            Defenitions.IMAGGA_API_KEY, Defenitions.IMAGGA_API_SECRET)));
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://api.imagga.com/v2/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(@"application/json"));
                client.DefaultRequestHeaders.Add("Authorization", String.Format("Basic {0}", basicAuthValue));

                HttpResponseMessage response = await client.GetAsync(String.Format("tags?image_url={0}", imageUrl));
                HttpContent content = response.Content;
                string result = await content.ReadAsStringAsync();

                JObject jobject = JObject.Parse(result);

                var selectedTages = (from t in jobject["result"]["tags"]

                                     select new { confidence = (double)t["confidence"], tag = (string)t["tag"]["en"] });

                int chance = 0;
                foreach (var item in selectedTages)
                {
                    if ((item.tag == "ice cream" || item.tag == "ice") && item.confidence >= 40)
                    {
                        return true;
                    }
                    if ((item.tag == "ice" || item.tag == "food" || item.tag == "stick" || item.tag == "cold" || item.tag == "bar" || 
                        item.tag == "cream" || item.tag == "cone" || item.tag == "popsicle" || item.tag == "vanilla")
                        && item.confidence >= 18)
                        chance += 50;
                }
                if (chance >= 90)
                    return true;
                else
                    return false;

            }
        }

        public class Tag
        {
            public double confidence { get; set; }
            public string tag { get; set; }
        }
    }
}
