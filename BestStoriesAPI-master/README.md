# BestStoriesAPI
API project build for BST, S.A.

Solution developed to create an API that retrreves the top 20 stories from Hacker News ordered by score value descending.
This project was developed with Microsoft Visual Studio 2019 Version 16.3.10 using C# with .net core 2.2

The application first validates if there is already an books list stored in cache, if it doesn't exist it will use hacker-news api to retrive a list of best stories id's, order them and store the first 20 records in descending order in Cache.

To test/validate this project:

  1. Make a local copy of this repository.
  2. Open it with Visual Studio 2019 
  3. It can be executed using a browser with the address https://localhost:44331/api/stories (tested in Chrome, Firefox and Microsoft Edge - the last option will only display raw JSON response).
  4. It was also tested with Postman with "No Auth" and disabling Postman SSL, requesting a GET operation to the address https://localhost:44331/api/stories

When accessing the first time in the development tests the average time is 54 seconds, after the first access the result iastored in cache and the average time is 
42 miliseconds using Microsoft Edge and very similar values for Chrome and Firefox.
The stored records will be cleaned every 60 minutes, that will require a longer period to retrieve the values. This value (60 minutes) can be changed in the class Constants located in Models/Stories.cs 




