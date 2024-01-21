
using IP_Tracker.Models;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Linq;
using System;
using System.Text;

namespace IP_Tracker.Controllers
{

    public class IpController : Controller
    {

        
        // GET: Ip
        public ActionResult Index(string ip)
        {
            Database dd = new Database();
           
            var data = Encoding.Default.GetBytes(dd.DataFromMemory(HttpContext.ApplicationInstance.Context));

            var coord = FetchLocation(data, ip);

            var item = new GetViewModel() {

                Data = coord[0],
                Latitude = coord[1],
                Longitude = coord[2]
             
             };
            
            return View(item);
        }

        
        public string[] FetchLocation(byte[] data, string ip)
        {

            var str_data = "";

            str_data += "[{'ip':'" + Database.IpToInt(ip) + "'}, ";
            str_data += "{'datasize':'" + data.Length + "'}, ";

            var start = StructTools.RawDeserialize<Header>(data, 0, "offset_ranges"); //Start of ip ranges index
            var end = data.Length; //End of file and ip ranges index
           
            str_data += "{'offset_ranges':'" + start + "'},";
            str_data += "{'ranges_end':'" + end + "'}]";

            var loc_index = 0;

            var intIp = Database.IpToInt(ip);//Convert Ip to Integer 
            //Iterate through ranges rows
            for (int i = start; i < end; i += 12)
            {
                
                var ip_from = StructTools.RawDeserialize<Ranges>(data, i, "ip_from");
                var ip_to = StructTools.RawDeserialize<Ranges>(data, i, "ip_to");

                if (ip_from <= intIp && ip_to >= intIp)//Finding range of Ip
                {
                    loc_index = StructTools.RawDeserialize<Ranges>(data, i, "location_index"); //Initializing index of latitue and longitute
                    break;
                }

            }

            var x = "" + Database.BinaryToFloat(""+StructTools.RawDeserialize<Location>(data, loc_index, "latitude"));
            var y = "" + Database.BinaryToFloat(""+StructTools.RawDeserialize<Location>(data, loc_index, "longitude"));

            string[] coord = { str_data, x , y };

            return coord;
        }

    }
}