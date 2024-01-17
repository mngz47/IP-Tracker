using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace IP_Tracker{

    public static class Database {

        public static string[] data_s;

        public static Stopwatch sw = new Stopwatch();

        // Get the elapsed time as a TimeSpan
        public static TimeSpan ts;


        // Uses StreamReader object and bufferSize argument to control load speed.
        public static void RR_String()
        {
            sw.Start();
            string fileName = AppContext.BaseDirectory + "/geobase.dat";
            
            const int bufferSize = 900*1024; //Will fetch data in 650kb chuncks

            var sb = new StringBuilder();
            var buffer = new Char[bufferSize];
            var length = 0L;
            var totalRead = 0L;
            var count = bufferSize;

            using (var sr = new StreamReader(fileName))
            {
                length = sr.BaseStream.Length;
                while (count > 0)
                {
                    count = sr.Read(buffer, 0, bufferSize);
                    sb.Append(buffer, 0, count); //Merge data chunks using string builder
                    totalRead += count;
                }
            }
            sw.Stop(); //measure process duration in (ms)
            ts = sw.Elapsed;

            DataToArray(sb);
        }


        public static void DataToArray(StringBuilder sb)
        {

            data_s = sb.ToString().Split(
            new string[] { Environment.NewLine },
            StringSplitOptions.None
                            );

        }

        //Binary Search 
        public static object FindMyObject(Array myArr, object myObject)
    {
        int myIndex=Array.BinarySearch(myArr, myObject);
        if (myIndex < 0)
        {
            return "No Match for "+myObject+" , "+~myIndex;

        }else
        {
            return myArr.GetValue(myIndex);

        }
    }

        public static object BinarySearchDisplay(string[] arr, string key)
        {
            
            int minNum = 0;
            int maxNum = arr.Length - 1;
            while (minNum <= maxNum)
            {
                int mid = (minNum + maxNum) / 2;
                if (arr[mid].Contains(key))
                {
                    return arr[mid];
                }
            }
            return "None";
        }




    }
}

 