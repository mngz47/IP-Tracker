using IP_Tracker.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;

namespace IP_Tracker.Controllers
{
    public class CityController : Controller
    {

        // GET: City
        public ActionResult Index(string city)
        {
            Database dd = new Database();

            var data = Encoding.Default.GetBytes(dd.DataFromMemory(HttpContext.ApplicationInstance.Context));

            var param = FetchLocations(data, city);

            var item = new GetViewModel() { Config=param[0] , Data = param[1] };

            return View(item);
        }

        public string[] FetchLocations(byte[] data, string city)
        {
            var str_data = "[{'city':'" + city + "'}, ";

            var start = StructTools.RawDeserialize<Header>(data, 0, "offset_locations");
            var end = StructTools.RawDeserialize<Header>(data, 0, "offset_cities");
            
            str_data += "{'offset_locations':'" + start + "'},";
            str_data += "{'offset_cities':'" + end + "'},";


            var recordCount = (int)((end - start) / 96);

            str_data += "{'record_count':'" + recordCount + "'},";

            var cities = StructTools.RawStringArrayDeserialize<Location>(data, start,"city", recordCount);

            str_data += "{'location_cities':'" + cities.Length + "'},";

            var locationIndex = Database.BinarySearch(cities, city);

            str_data += "{'city_loc_index':'" + locationIndex + "'},";

              
         start = StructTools.RawDeserialize<Header>(data, 0, "offset_cities");
         end = StructTools.RawDeserialize<Header>(data, 0, "offset_ranges");

         str_data += "{'offset_cities':'" + start + "'},";
         str_data += "{'offset_ranges':'" + end + "'},";
        
       
         List<int> locationByCity = new List<int>();

         //binary search for index in location indexes sorted by city
     

            var offset_by_city = Database.BinarySearch(StructTools.RawIntArrayDeserialize<Bycity>(data, start, "location_index",(end-start)/4), locationIndex);

            for (int e = offset_by_city; e < offset_by_city + 20 * 4; e += 4)
            {
              var  loc_id = StructTools.RawDeserialize<Bycity>(data, e, "location_index");

                locationByCity.Add((sbyte)loc_id);

            }

            str_data += "{'locationByCity':'" + String.Join(",", locationByCity) +"'}]";

            //Fetch locations by city name
            var loc_data = "";
         for (int ii = 0; ii < locationByCity.Count; ii++)
         {
             loc_data +=
             "[{'country':'" + StructTools.RawDeserialize<Location>(data, locationByCity[ii], "country") + "'}," +
             "{'region':'" + StructTools.RawDeserialize<Location>(data, locationByCity[ii], "region") + "'}," +
             "{'postal':'" + StructTools.RawDeserialize<Location>(data, locationByCity[ii], "postal") + "'}," +
             "{'city':'" + StructTools.RawDeserialize<Location>(data, locationByCity[ii], "city") + "'}," +
             "{'organization':'" + StructTools.RawDeserialize<Location>(data, locationByCity[ii], "organization") + "'}]";
         }
         
         string[] param = { str_data , loc_data};

            return param;
        }

    }
}