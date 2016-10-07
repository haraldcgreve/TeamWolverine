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
        private const string AccessToken = "2233692468.97d3404.d6f0d30225504e978d424b8db1091997";

        // GET: Images
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Main()
        {
            return View();
        }

        public ActionResult GetImages()
        {
            string urisInstagram =
                    string.Format("https://api.instagram.com/v1/tags/macmillanselfie/media/recent?access_token={0}",
                        AccessToken);
        
            var result = new WebClient().DownloadString(urisInstagram);

            JavaScriptSerializer jss = new JavaScriptSerializer();

            var instaJson = jss.Deserialize<dynamic>(result);

            var data = instaJson["data"];

            List<ImageModel> imgList = new List<ImageModel>();

            var list = new ImageListViewModel {Total = 0};

            foreach (var img in data)
            {
                list.Total++;

                var liked = (bool) img["user_has_liked"];

                if (liked)
                {
                    imgList.Add(new ImageModel
                    {
                        LowResImageUrl = img["images"]["low_resolution"]["url"],
                        HighResImageUrl = img["images"]["standard_resolution"]["url"],
                        Location = img["location"]["name"] ?? img["location"]["name"]
                });
                }
            }

            list.ImageList = imgList;

            return Json(list, JsonRequestBehavior.AllowGet); ;
        }
    }
}