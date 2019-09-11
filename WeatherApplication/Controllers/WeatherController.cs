using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace WeatherApplication.Controllers
{
    public class WeatherController : Controller
    {
        // GET: Weather
        public ActionResult Index()
        {
            IEnumerable<User> users;
            using (var dbContext = new WeatherEntities())
            {
                users = (
                    from u in dbContext.Users
                    select u
                    ).ToList();
            }

            var selectItems = new List<SelectListItem>();
            foreach(User user in users)
            {
                SelectListItem itemToAdd = new SelectListItem();
                itemToAdd.Text = string.Concat(user.FirstName, " ", user.LastName);
                itemToAdd.Value = user.CityID;
                selectItems.Add(itemToAdd);
            }

            ViewBag.Users = selectItems;

            ViewBag.Message = "Weather Page";
            return View();
        }

        public ActionResult FindByCityID(string cityId)
        {
            ContentResult result = new ContentResult() { ContentType = "application/json" };
            var apiKey = ConfigurationManager.AppSettings["WeatherAPIKey"];
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create($"http://api.openweathermap.org/data/2.5/weather?id={cityId}&units=imperial&APPID=" + apiKey);
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    result.Content = new StreamReader(response.GetResponseStream()).ReadToEnd();
                }
                else
                {
                    throw new Exception(string.Concat(response.StatusCode, string.Empty));
                }
            }

            return result;
        }

        public ActionResult FindByName(string cityName)
        {
            ContentResult result = new ContentResult() { ContentType = "application/json" };
            var apiKey = ConfigurationManager.AppSettings["WeatherAPIKey"];
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create($"http://api.openweathermap.org/data/2.5/weather?q={cityName}&APPID=" + apiKey);
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    result.Content = new StreamReader(response.GetResponseStream()).ReadToEnd();
                } else
                {
                    throw new Exception(string.Concat(response.StatusCode, string.Empty));
                }
            }

                return result;
        }
    }
}