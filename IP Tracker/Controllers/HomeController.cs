using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using AutoMapper;
using IP_Tracker.Models;

namespace IP_Tracker.Controllers
{
   // [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            //read geobase.dat into app memory

             Database dd = new Database();
             if(dd.DataFromMemory(HttpContext.ApplicationInstance.Context)==null) //check if memory has existing data
              dd.DataToMemory(dd.ReadBytes(), HttpContext.ApplicationInstance.Context);


            //print file read duration

            var item = new GetViewModel() { Data = "geobase.dat loaded in "+Database.ts.Milliseconds+" ms" };

            return View(item);
        }

    }
}
