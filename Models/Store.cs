using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace crudTest.Models
{
    public class Store
    {
        [BsonId]
        public ObjectId ID { get; set; }


        [BsonElement("Name")]
        public string Name { get; set; }

        [BsonElement("ImageUrl")]
        public string ImageUrl { get; set; }

        [BsonElement("Address")]
        public string Address { get; set; }


        [BsonElement("Kosher")]
        public string Kosher { get; set; }


        [BsonElement("Hours")]
        public string Hours { get; set; }

        [BsonElement("Tel")]
        public string Tel { get; set; }


    }
}
