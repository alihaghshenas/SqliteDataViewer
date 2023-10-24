# Sqlite Data Viewer/ dotnet core
we had a challenge for saving the user activity inside the website and logging the server messages + informative messages. we decided to use SQLite for saving and managing the data.
in this project I created an SQLite data viewer that will search into the project and find all of the sqlite.db databases, then we can view and manage all of the data from that database.
I created a query and search system as well so you can manipulate the data from within the UI

to implement this in your project just copy the contoller,services,interfaces and views 
you also need to customize the *.db searching folder, in here we are searching for all of the databases inside the "Privatefiles->LogData" folder.
