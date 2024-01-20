using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Linq;

namespace IP_Tracker
{
    public static class StructTools
    {
       
        //inputs raw byte array and performs marshal using struct <T> to map fields and returns field_val corresponding to field_name
        public static string RawDeserialize<T>(byte[] rawData, int position,string fieldName)
        {
            int rawsize = Marshal.SizeOf(typeof(T));
            if (rawsize > rawData.Length - position)
                throw new ArgumentException("Not enough data to fill struct. Array length from position: " + (rawData.Length - position) + ", Struct length: " + rawsize);
            IntPtr buffer = Marshal.AllocHGlobal(rawsize);

            Marshal.Copy(rawData, position, buffer, rawsize);

           
            IntPtr retobj = Marshal.OffsetOf(typeof(T), fieldName);

            var field_val = Marshal.ReadInt32(buffer, Int32.Parse(retobj.ToString())).ToString();
             
            
            Marshal.FreeHGlobal(buffer);
            return field_val;
        }


    }
}