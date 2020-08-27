using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace crudTest.App_Start
{
    public class MongoContext
    {
        MongoClient _client;
        MongoServer _server;
        public MongoDatabase _database;
        public MongoContext()        //constructor   
        {
            // Reading credentials from Web.config file   
            var MongoDatabaseName = Defenitions.MONGO_DATABASE; // Project2019"; 
            //var MongoUsername = ConfigurationManager.AppSettings["MongoUsername"]; //demouser  
            //var MongoPassword = ConfigurationManager.AppSettings["MongoPassword"]; //Pass@123  
            var MongoPort = Defenitions.MONGO_PORT; //27017;
            var MongoHost = Defenitions.MONGO_HOST;

            //// Creating credentials  
            //var credential = MongoCredential.CreateMongoCRCredential
            //                (MongoDatabaseName,
            //                 MongoUsername,
            //                 MongoPassword);

            // Creating MongoClientSettings  
            var settings = new MongoClientSettings
            {
                //Credentials = new[] { credential },
                Server = new MongoServerAddress(MongoHost, Convert.ToInt32(MongoPort))
            };
            _client = new MongoClient(settings);
            _server = _client.GetServer();
            _database = _server.GetDatabase(MongoDatabaseName);
        }
    }
}