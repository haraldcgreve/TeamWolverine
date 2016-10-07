using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using System.Web.Script.Serialization;
using TeamWolverine.Models;
using Microsoft.Owin.Security.Twitter.Messages;

namespace TeamWolverine.Controllers
{
    public class ImagesController : Controller
    {

        string _Instagram_client_id = "833d878fa43c4f24b80a4631e50bfb1a";
        private const string AccessToken = "2233692468.97d3404.d6f0d30225504e978d424b8db1091997";
        private double plusminusLatLng = 0.01;

        // GET: Images
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Main()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Index(string query)
        {
            ImageListViewModel model = GetImageList(query);

            return View(model);
        }

        private ImageListViewModel GetImageList(string query)
        {
            if (!string.IsNullOrEmpty(query))
            {
                var coordinates = GetLatLngFromLocation(query);


                var imgList = GetImages(coordinates[0], coordinates[1]);
                

                return new ImageListViewModel { ImageList = imgList };
            }
            return new ImageListViewModel();
        }



        public ActionResult GetImageJson(string query)
        {
            ImageListViewModel model = GetImageList(query);

            return Json(model, JsonRequestBehavior.AllowGet);

            //return Content(new JavaScriptSerializer().Serialize(model));
        }

        public ActionResult GetImageJsonForLatLng(string lat, string lng)
        {
            ImageListViewModel model;

            var imgList = GetImages(Convert.ToDouble(lat), Convert.ToDouble(lng));

            return Json(new ImageListViewModel { ImageList = imgList }, JsonRequestBehavior.AllowGet);
        }

        //public ActionResult GetEventDataJson()
        //{
        //    return Json(FetchPinData(), JsonRequestBehavior.AllowGet);
        //}

        private IEnumerable<ImageModel> GetImages(double lati, double lngi)
        {
            //var code =


            //string uriInstagram =
            //    "https://www.instagram.com/oauth/authorize/?client_id=97d34045767c42abbeadc0343aa651b1&redirect_uri=http://teamwolverine.azurewebsites.net&response_type=code&scope=public_content";
            //        //string.Format("https://api.instagram.com/v1/tags/teammacmillan/media/recent?access_token={0}",
            //         //   AccessToken);

            //var resultCode = new WebClient().DownloadString(uriInstagram);

            //var request = (HttpWebRequest)WebRequest.Create("https://api.instagram.com/oauth/access_token");

            //var postData = "client_id=97d34045767c42abbeadc0343aa651b1";
            //postData += "&client_secret=837b554f87384b9ca97100c098a21409";
            //postData += "&grant_type=authorization_code";
            //postData += "&redirect_uri=http://teamwolverine.azurewebsites.net";
            //postData += "&code=9c4d542cf9a348e891aed2f58b4daded";
            //var datas = Encoding.ASCII.GetBytes(postData);

            //request.Method = "POST";
            //request.ContentType = "application/x-www-form-urlencoded";
            //request.ContentLength = datas.Length;

            //using (var stream = request.GetRequestStream())
            //{
            //    stream.Write(datas, 0, datas.Length);
            //}

            //var response = (HttpWebResponse)request.GetResponse();

            //var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();


            string urisInstagram =
                    string.Format("https://api.instagram.com/v1/tags/teammacmillan/media/recent?access_token={0}",
                        AccessToken);
        
            var result = new WebClient().DownloadString(urisInstagram);

            JavaScriptSerializer jss = new JavaScriptSerializer();

            var instaJson = jss.Deserialize<dynamic>(result);

            var data = instaJson["data"];

            List<ImageModel> imgList = new List<ImageModel>();

            foreach (var img in data)
            {
                if (img["location"] != null)
                {

                    var lat = (double)img["location"]["latitude"];
                    var lng = (double)img["location"]["longitude"];

                    if (lat + plusminusLatLng > lati
                        && lat - plusminusLatLng < lati
                        && lng + plusminusLatLng > lngi
                        && lng - plusminusLatLng < lngi)
                    {
                        var name = img["location"]["name"];

                        imgList.Add(new ImageModel
                        {
                            ImageUrl = img["images"]["low_resolution"]["url"],
                            Latitude = lat,
                            Longitude = lng,
                            Name = name
                        });
                    }
                }
            }

            return imgList;
        }

        private List<double> GetLatLngFromLocation(string query)
        {
            string urigeo = string.Format("http://maps.googleapis.com/maps/api/geocode/json?address={0}", query);

            var result = new WebClient().DownloadString(urigeo);
            JavaScriptSerializer jss = new JavaScriptSerializer();
            var geoJson = jss.Deserialize<dynamic>(result);

            List<double> latLongs = new List<double>();

            if (geoJson != null)
            {
                latLongs.Add((double)geoJson["results"][0]["geometry"]["location"]["lat"]);
                latLongs.Add((double)geoJson["results"][0]["geometry"]["location"]["lng"]);
            }

            return latLongs;
        }
    }
}