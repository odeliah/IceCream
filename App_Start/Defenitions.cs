using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace crudTest.App_Start
{
    public class Defenitions
    {
        public static string ADMIN_EMAIL = "Admin@gmail.com";
        public static string ADMIN_PASSWORD = "admin123";
               
        public static string MAIL_ADDRESS = ConfigurationManager.AppSettings["MAIL_ADDRESS"];
        public static string MAIL_PASSWORD = ConfigurationManager.AppSettings["MAIL_PASSWORD"];
               
        public static string MAIL_HOST = ConfigurationManager.AppSettings["MAIL_HOST"];
        public static int MAIL_PORT = Int32.Parse(ConfigurationManager.AppSettings["MAIL_PORT"]);
               
        public static string IMAGGA_API_KEY = ConfigurationManager.AppSettings["IMAGGA_API_KEY"];
               
        public static string IMAGGA_API_SECRET = ConfigurationManager.AppSettings["IMAGGA_API_SECRET"];
               
        public static string MONGO_HOST= ConfigurationManager.AppSettings["MONGO_HOST"];
        public static string MONGO_DATABASE = ConfigurationManager.AppSettings["MONGO_DATABASE"];
        public static string MONGO_PORT = ConfigurationManager.AppSettings["MONGO_PORT"];
        //public static string MONGO_PORT = ConfigurationManager.AppSettings["MONGO_USER"];
               
        public static string GOOGLE_MAPS_API = ConfigurationManager.AppSettings["GOOGLE_MAPS_API"];
        public static string GOOGLE_MAPS_URL = "http://maps.google.co.uk/maps/geo?output=csv&key=";
        public static string GOOGLE_MAPS_VIEW = "https://maps.googleapis.com/maps/api/js?key=";
               
        public static string USDA_URL = "https://api.nal.usda.gov/fdc/v1/";
        public static string USDA_API_KEY = ConfigurationManager.AppSettings["USDA_API_KEY"];

    }
}