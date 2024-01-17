﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using IP_Tracker.Models;

namespace IP_Tracker.Controllers
{
   // [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {

            var item = new GetViewModel() { Data = "geobase.dat loaded in "+Database.ts.Milliseconds+" ms" };
            return View(item);
        }

        //create json response object
        private readonly IMapper _mapper;

        //HTTP API method
        [Route("ip/location")]
        [HttpGet]
        public ActionResult GetLocationByIp(string ip)
        {

            Console.WriteLine("Route Works");
            var row =  Database.FindMyObject(Database.data_s, ip);

            string[] cols = row.ToString().Split(' ');

            var item = new GetViewModel() { Data = _mapper.Map<string>(cols) };

            return View("~/Views/Home/_Home.cshtml", item);
        }

        //HTTP API method
        [Route("city/locations")]
        [HttpGet]
        public ActionResult GetLocationByCity(string city)
        {

            var row = Database.BinarySearchDisplay(Database.data_s, city);

            string[] cols = row.ToString().Split(' ');

            var item = new GetViewModel() { Data = _mapper.Map<string>(cols) };

            return View("~/Views/Home/Index.cshtml", item);

        }


    }
}
