using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MechanicNearMe.Models
{
    class GMapApi
    {
        string url;
        List<Result> allresults;

        public GMapApi()
        {
            allresults = new List<Result>();

        }


        public List<Result> FetchData(string lat, string lng, string radius, string keyword)
        {
            url = @"https://maps.googleapis.com/maps/api/place/nearbysearch/json?location="
                           + lat
                           + "," + lng
                           + "&radius=" + radius
                           + "&keyword=" + keyword;


            //      url += "&key=AIzaSyDh-5cQqym0fy2BgOyF0VOmFkH9y71VNUg";
            url += "&key=AIzaSyAYqT-F1tNfpNiFYt2tHSjWHgdpYD_yeuI";


            var json = new WebClient().DownloadString(url);

            var Jsonobject = JsonConvert.DeserializeObject<RootObject>(json);

            List<Result> list = Jsonobject.results;


            foreach (var item in list)
            {
                allresults.Add(item);

            }

            FetchNext(Jsonobject.next_page_token, keyword);

            return allresults;
        }


        public void FetchNext(string token, string keyword)
        {
            Thread.Sleep(3000);
            if (token == null)
            {
                return;

            }
            else
            {
                url += "&pagetoken=" + token;
                var json = new WebClient().DownloadString(url);

                var Jsonobject = JsonConvert.DeserializeObject<RootObject>(json);

                List<Result> list = Jsonobject.results;


                foreach (var item in list)
                {
                    allresults.Add(item);

                }

                url = url.Substring(0, url.Length - token.Length - 1);
                url = url.Replace("&pagetoken", "");
                FetchNext(Jsonobject.next_page_token, keyword);
            }

        }


    }
}
