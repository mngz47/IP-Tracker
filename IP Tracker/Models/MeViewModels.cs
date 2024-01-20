using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IP_Tracker.Models
{
    // Models returned by MeController actions.
    public class GetViewModel
    {
        public string Hometown { get; set; }

        public string Data { get; set; }

        public string Latitude { get; set; }

        public string Longitude { get; set; }

    }
}