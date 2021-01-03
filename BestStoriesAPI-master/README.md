# BestStoriesAPI
API project build for SGT

To test/validate this project:

  1. Make a local copy of this repository.
  2. Open it with Visual Studio 2019 
  3. execute it using IIS Express option, it will display a raw display of the JSON response
  4. For a cleaner view you can use Postman with no credentials using the address https://localhost:44331/api/stories

This project was built with Visual Studio 2019 using C# and .Net Core 2.2

The application first validates if there is already a Top Twenty Stories List ordered by each story score descending stored in cache. 
If it doesn't exist it will use hacker-news api to retrive a list of best stories id's, order them and store the first 20 records in 
descending order in Cache.
When accessing the first time in the development tests the average time is 54 seconds, when acessing the cache the average time is 
42 miliseconds using Microsoft Edge and very similar values for Chrome and Firefox.
The stored records will be cleaned every 60 minutes, this value can be changed in the class Constants located in Models/Stories.cs




