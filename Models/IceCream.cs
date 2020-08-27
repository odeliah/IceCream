using crudTest.App_Start;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace crudTest.Models
{
    public class IceCream
    {
        [BsonId]
        public ObjectId ID { get; set; }


        [BsonElement("Name")]
        public string Name { get; set; }


        [BsonElement("Store")]
        public string Store { get; set; }


        [BsonElement("Recommendations")]
        public List<Recommendation> Recommendations { get; set; }


        private string _usdaId;
        [BsonIgnore]
        public string UsdaID
        {
            get { return _usdaId; }
            set
            {
                try
                {
                    _usdaId = value;
                    Task<Nutrition> task = RunAsync(value);
                    task.Wait();
                    _energy = task.Result.Energy;
                    _fats = task.Result.Fats;
                    _protein = task.Result.Protein;
                    if (_energy == 0 && _fats == 0 && _protein == 0)
                        _usdaId = "0";
                }
                catch
                {
                    _usdaId = "0";
                }
            }
        }
             
        [BsonElement("ImageUrl")]
        public string ImageUrl { get; set; }

        private double _energy;

        [BsonElement("Calories")] 
        public double Energy
        {
            get { return _energy; }
            set { _energy = value; }
        }

        private double _fats;

        [BsonElement("Fats")]
        public double Fats
        {
            get { return _fats; }
            set { _fats = value; }

        }

        private double _protein;

        [BsonElement("Protein")]
        public double Protein
        {
            get { return _protein; }
            set { _protein = value; }

        }


        static async Task<Nutrition> RunAsync(string usdaId)
        {
            string content = "";

            try
            {                
                string url = Defenitions.USDA_URL + usdaId + "?api_key=" + Defenitions.USDA_API_KEY;

                var httpClient = new HttpClient();
                var response = await httpClient.GetAsync(url).ConfigureAwait(false);
                //will throw an exception if not successful
                response.EnsureSuccessStatusCode();
                content = await response.Content.ReadAsStringAsync();
            }
            catch (Exception)
            {
                Console.WriteLine("could not work api");
            }

            JObject jsonO = JObject.Parse(content);

            string p = "";
            string f = "";
            string e = "";
            bool flag = false;

            foreach (var item in jsonO["foodNutrients"])
            {
                if (item["nutrient"]["name"].ToString() == "Protein")
                    p = item["amount"].ToString();
                if ((item["nutrient"]["name"].ToString() == "Energy") && (flag==false))
                {
                    flag = true;
                    e = item["amount"].ToString();
                }
                if (item["nutrient"]["name"].ToString()== "Total lipid (fat)")
                    f = item["amount"].ToString();
            }

            Nutrition nutrition = new Nutrition();
                  
            nutrition.Energy = double.Parse(e);
            nutrition.Fats = double.Parse(f);
            nutrition.Protein = double.Parse(p);

            return nutrition;         
        }
    }   
}

