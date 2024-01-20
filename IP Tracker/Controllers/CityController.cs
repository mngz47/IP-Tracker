using IP_Tracker.Models;
using System;
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

            var item = new GetViewModel() { Data =  FetchLocations(data, city) };

            return View(item);
        }

        public string FetchLocations(byte[] data, string city)
        {
            string str_data = "";

            str_data += "[{'city':'" + city + "'}, ";

            string start = StructTools.RawDeserialize<Header>(data, 0, "offset_cities");
            string end = StructTools.RawDeserialize<Header>(data, 0, "offset_locations");

            str_data += "{'offset_cities':'" + start + "'},";
            str_data += "{'offset_locations':'" + end + "'},";

            var locationIndex = 0;
            //search for city in locationDataLines then return index
            for (int i = Int32.Parse(end); i < Int32.Parse(start); i+=96)
            {

                var loc_city = StructTools.RawDeserialize<Location>(data, i, "city");

                if (Int32.Parse(loc_city) == Database.BinaryToInt(city))
                {
                    str_data += "{'city_found':'" + loc_city + "'},";
                    break;
                }
                else {
                    locationIndex++;
                }
            }
            str_data += "{'city_loc_index':'" + locationIndex + "'},";


            start = StructTools.RawDeserialize<Header>(data, 0, "offset_cities");
            end = StructTools.RawDeserialize<Header>(data, 0, "offset_ranges");

            str_data += "{'offset_cities':'" + start + "'},";
            str_data += "{'offset_ranges':'" + end + "'},";

            var locationByCity = new int[10];
            //binary search for index in sorted cityDataLines
            for (int i = Int32.Parse(end); i < Int32.Parse(start); i += 4)
            {

                var loc_id = StructTools.RawDeserialize<Bycity>(data, i, "location_index");

                if (locationIndex == Int32.Parse(loc_id)) {


                    locationByCity[locationByCity.Length - 1] = Int32.Parse(loc_id);

                    for (int e = i+1; e < e+10; e+=4)
                    {
                        loc_id = StructTools.RawDeserialize<Bycity>(data, e, "location_index");
                        locationByCity[locationByCity.Length - 1] = Int32.Parse(loc_id);

                    }

                    break;
                }
            }

            str_data += "{'locationByCity':'" + String.Join(",", locationByCity) +"'},"; 

            //Transfer locations by city name from locationDataLines

            for (int i = 0; i < locationByCity.Length; i++)
            {
                str_data +=
                "[{'country':'" + StructTools.RawDeserialize<Location>(data, locationByCity[i], "country") + "'}," +
                "{'region':'" + StructTools.RawDeserialize<Location>(data, locationByCity[i], "region") + "'}," +
                "{'postal':'" + StructTools.RawDeserialize<Location>(data, locationByCity[i], "postal") + "'}," +
                "{'city':'" + StructTools.RawDeserialize<Location>(data, locationByCity[i], "city") + "'}," +
                "{'organization':'" + StructTools.RawDeserialize<Location>(data, locationByCity[i], "organization") + "'}]";

            }

            str_data += "]";

            return str_data;
        }

    }
}