using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Linq;

namespace IP_Tracker
{
    public static class StructTools
    {
       
        //inputs raw byte array and performs marshal using struct <T> to map fields and returns field_val corresponding to field_name
       public static int RawDeserialize<T>(byte[] rawData, int position,string fieldName)
        {
            int rawsize = Marshal.SizeOf(typeof(T));
            if (rawsize > rawData.Length - position)
                throw new ArgumentException("Not enough data to fill struct. Array length from position: " + (rawData.Length - position) + ", Struct length: " + rawsize);
            IntPtr buffer = Marshal.AllocHGlobal(rawsize);

            Marshal.Copy(rawData, position, buffer, rawsize);

           
            IntPtr retobj = Marshal.OffsetOf(typeof(T), fieldName);

            var field_val = Marshal.ReadInt32(buffer, Int32.Parse(retobj.ToString()));
             
            
            Marshal.FreeHGlobal(buffer);
            return field_val;
        }
    
        public static string RawStringDeserialize<T>(byte[] rawData, int position, string fieldName)
       {
           int rawsize = Marshal.SizeOf(typeof(T));
           if (rawsize > rawData.Length - position)
               throw new ArgumentException("Not enough data to fill struct. Array length from position: " + (rawData.Length - position) + ", Struct length: " + rawsize);
           IntPtr buffer = Marshal.AllocHGlobal(rawsize);

           Marshal.Copy(rawData, position, buffer, rawsize);

           IntPtr retobj = Marshal.OffsetOf(typeof(T), fieldName);

           var field_val = String.Join("", IntPtrToStringArrayUni(buffer,Int32.Parse(retobj.ToString())));

           Marshal.FreeHGlobal(buffer);
           return field_val;
       }

        public static string[] RawStringArrayDeserialize<T>(byte[] rawData, int position, string fieldName, int recordCount)
        {

            string[] resultArray = new string[recordCount];

            int rawsize = Marshal.SizeOf(typeof(T));

            int count = 0;
            for (int i = position; i < recordCount*rawsize; i+= rawsize)

            {
                resultArray[count] = RawStringDeserialize<T>(rawData, i, fieldName);
                count++;
            }

                return resultArray;
        }


        public static int[] RawIntArrayDeserialize<T>(byte[] rawData, int position, string fieldName, int recordCount)
        {

            int [] resultArray = new int[recordCount];

            int rawsize = Marshal.SizeOf(typeof(T));

            int count = 0;
            for (int i = position; i < recordCount * rawsize; i += rawsize)

            {
                resultArray[count] = RawDeserialize<T>(rawData, i, fieldName);
                count++;
            }

            return resultArray;
        }


        public static string[] IntPtrToStringArrayUni(IntPtr ptr,int index)
       {
           var lst = new List<string>();

           do
           {
               lst.Add(Marshal.PtrToStringUni(ptr,index));

               while (Marshal.ReadByte(ptr) != 0)
               {
                   ptr = IntPtr.Add(ptr, 1);
               }

               ptr = IntPtr.Add(ptr, 1);
           }
           while (Marshal.ReadByte(ptr) != 0);

           // See comment of @zneak
           if (lst.Count == 1 && lst[0] == string.Empty)
           {
               return new string[0];
           }

           return lst.ToArray();
       }

    }
}