# How to run tutorial

## Step 1.

`docker compose up` in `Backend` and wait around a minute until it starts up fully (it will download all the containers and set up the schema)

## Step 2.

Aquire `Visual Studio` and open `SploinkyAPI.sln` in `Backend/SploinkyAPI`
And make sure the `CassandraCSarpDriver NuGet package by DataStax` is installed (if it isn't already in the solution)
You **might** also need to install the `ASP.NET API` project type in the `Visual Studio Installer`

When you run it you can test individual api requests on swagger if you want
All available requests are defined in `ReservationController.cs`

## Step 3.

`npm install` and then `npm run dev` in `Frontend` (react required)

# Stress tests

They're in `Backend/Tests/StressTests` as standalone `.py` scripts, make sure backend is running, also keep in mind the default schema makes a few reservations on container startup (some 409 errors might show up due to seats being taken)
