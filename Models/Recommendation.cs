using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace crudTest.Models
{
    public class Recommendation
    {
            [BsonId]
            public ObjectId RecID { get; set; }

            [BsonElement("Name")]
            public string Name { get; set; }


            [BsonElement("Rank")]
            [RegularExpression(@"^\(?([1-5]{1})\)?$", ErrorMessage = "Please insert Rank between 1 to 5")]
            public int Rank { get; set; }


            [BsonElement("ImageUrlUrl")]
            public string ImageUrl { get; set; }

            [BsonElement("Description")]
            public string Description { get; set; }

        }

}
