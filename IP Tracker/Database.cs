using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Net;

namespace IP_Tracker{

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct Header //60bytes
    {
        int version;           // database version
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        sbyte[] name;        // database name/prefix
        ulong timestamp;         // database creation time
        int records;           // the total number of records
        uint offset_ranges;     // offset from the beginning of the file to the beginning of the list of records with geo data
        uint offset_cities;     // offset from the beginning of the file to the beginning of the index sorted by city names
        uint offset_locations;  // offset from the beginning of the file to the beginning of the list of location records
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct Ranges //12bytes
    {
        uint ip_from;           // beginning of the range of IP addresses
        uint ip_to;             // end of the range of IP addresses
        uint location_index;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct Bycity //4bytes
    {
    int location_index;
    }


    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct Location //96bytes
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        sbyte[] country;        // country name (random string with the "cou_" prefix)
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
        sbyte[] region;        // region name (random string with the "reg_" prefix)
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
        sbyte[] postal;        // postal code (random string with the "pos_" prefix)
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 24)]
        sbyte[] city;          // city name (random string with the "cit_" prefix)
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        sbyte[] organization;  // organization name (random string with the "org_" prefix)
        float latitude;          // latitude
        float longitude;
    }

    public class Database {

        public static Stopwatch sw = new Stopwatch();

        // Get the elapsed time as a TimeSpan
        public static TimeSpan ts;

        string SessionKeyName = "_Data";

        // Uses BinaryReader object and bufferSize argument to control load speed.
       
        public string ReadBytes()
        {
            sw.Start();

            const int bufferSize = 350 * 1024; //Will fetch data in 250kb chuncks

            string sb = "";
            var buffer = new byte[bufferSize];
            var count = bufferSize;

            using (BinaryReader b = new BinaryReader(
                File.Open(AppContext.BaseDirectory + "/geobase.dat", FileMode.Open)))
            {

                while (count > 0)
                {
                    count = b.Read(buffer, 0, bufferSize);
                    var str = Encoding.Default.GetString(buffer);

                    sb += (str);
                }
            }
            sw.Stop();
            ts = sw.Elapsed;
            return sb;
        }

        // Store geobase.dat into session memory for usage in next request
        public void DataToMemory(string _data, HttpContext ss)
        {
            
             if (string.IsNullOrEmpty((string)ss.Session[SessionKeyName]))
            {
                ss.Session[SessionKeyName] = _data;

            }
        }

        // Read geobase.dat from session memory
        public string DataFromMemory(HttpContext ss)
        {

            if (string.IsNullOrEmpty((string)ss.Session[SessionKeyName]))
            {
                return null;
            }
            else{
                return (string)ss.Session[SessionKeyName];
            }

        }

        // Binary Search 
        public static int BinarySearch(string[] aa, string a)
        {
            int index = 0;
            for (int i = 0; i < aa.Length; i++)
            {
                if ((sbyte)BinaryToInt(aa[i]) == (sbyte)BinaryToInt(a))
                {
                    index = i;
                    break;
                }
            }
            return index;
        }

        public static int BinarySearch(int[] aa, int a)
        {
            int index = 0;
            for (int i = 0; i < aa.Length; i++)
            {
                if ((sbyte)aa[i] == (sbyte)a)
                {
                    index = i;
                    break;
                }
            }
            return index;
        }

        // Data Utilities
        public static string ArrayToJson(string[] cols)
        {
            var aa = cols.Select(x => new { row = x}).ToArray();
            var serializer = new JavaScriptSerializer();
            var json = serializer.Serialize(aa);
            return (string)json;
        }

        public static int BinaryToInt(string dd)
        {
            var buffer = Encoding.Unicode.GetBytes(dd);
            return BitConverter.ToInt32(buffer, 0);
        }

        public static float BinaryToFloat(string dd)
        {
            var buffer = new byte[16];
            Encoding.Unicode.GetBytes(dd).CopyTo(buffer,0);
            return BitConverter.ToSingle(buffer, 0);
        }

        public static int IpToInt(string ip)
        {

            return BitConverter.ToInt32(IPAddress.Parse(ip).GetAddressBytes().Reverse().ToArray(),0);

        }

       

    }
}