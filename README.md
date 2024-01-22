![image](https://github.com/mngz47/IP-Tracker/assets/15697629/20384edd-ce51-4c33-8759-ec717d982267)


Uses Binary Reader to get bytes when <strong>HomeController.cs</strong> is fired then saves data as string onto a session.

The <strong>Database.cs</strong> contains 3 structs Header, Ranges and Location used to map fields of binary data during deserialization. Also consists of functions to store data into session memory and various utilities to convert Ip to integer and array to json format.

The <strong>StructTools.cs</strong> contains deserialization function.

<strong>IpController.cs</strong> Fetches data from session uses struct to deserialize the data and extract field names and values. The fields are used to get coordinates using Ip argument.

<strong>CityContoller.cs</strong> Also to get Locations by city name when  is routed.

