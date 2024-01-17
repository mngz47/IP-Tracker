using System;
using System.IO;

namespace IP_Tracker{
	
	public static class Database{

        public static byte[] Data_b;
	
	    public static string[] Data_s;


        //read data into byte array
        public static void R_Byte()
    {
            // 1.
            using (BinaryReader b = new BinaryReader(
                File.Open(AppContext.BaseDirectory + "/geobase.dat", FileMode.Open)))
            {
               
                int pos = 0;
                int length = (int)b.BaseStream.Length;

                Data_b = new byte[length];


                while (pos < length)
            {
                int v = b.ReadInt32();
				Data_b[pos] = ((byte)v);
                pos += sizeof(int);
            }
        }
    }


        //read data line by line into string array
        public static void R_String(){
		
		System.IO.StreamReader file = new System.IO.StreamReader(AppContext.BaseDirectory + "/geobase.dat");
		    int counter = 0;
            string line;


            int length = (int)file.BaseStream.Length;
            Data_s = new string[length];

            while ((line = (file.ReadLine())) != null){
	
			Data_s[counter] = line;
	
			counter++;
		}
	   
	}

        //Binary Search method 1
        public static object FindMyObject(Array myArr, object myObject)
    {
        int myIndex=Array.BinarySearch(myArr, myObject);
        if (myIndex < 0)
        {
            return "No Match "+myObject+" , "+~myIndex;
        }
        else
        {
            return myObject + " , " + myIndex;
        }
    }

        //Binary Search method 2
        public static object BinarySearchDisplay(int[] arr, int key)
        {
            int max = 0;
            int min = 0;
            int minNum = 0;
            int maxNum = arr.Length - 1;
            while (minNum <= maxNum)
            {
                int mid = (minNum + maxNum) / 2;
                if (key == arr[mid])
                {
                    return ++mid;
                }
                else if (key < arr[mid])
                {
                    max = mid - 1;
                }
                else
                {
                    min = mid + 1;
                }
            }
            return "None";
        }

	
	}
}

 