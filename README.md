# Sploinky's Cinema

## 160302, 159351

A simple demo app of a cinema website for making reservations

# The stack

The app uses:
 - **Cassandra** as the database
 - **C# ASP.NET** as the backend
 - **React TS** as the frontend

# Database schema

The database was two tables:

Movies (movie_id, movie_name, image_path) - stores information about various movies

Reservations (movie_id, movie_name, username, row, seat) - stores information about who has reserved which seats on what movie

# API

The app utilises api requests for fetching data from the database and passing it to the frontend, such as:
 - getting the list of all movies
 - making a reservation
 - updating a reservation
 - cancelling a reservation

Every request returns various appropriate status codes and messages, and is also asynchronous to not have the entire backend blocked by a single query.

# Frontend

The web app has two pages:
 - IndexPage - displays the list of movies
 - MoviePage - allows the user to view, make, transfer, or cancel reservations on a given movie

To make a reservation you need to specify your username by clicking the `log in` button on the navigation bar.

The component for making reservations is a 10x10 grid of accordinly colored buttons each of which will allow you to either view, make, transfer, or cancel a reservation depending on whether it belongs to you or not.

# Issues encountered

Since the individual making the backend was very insistent on learning ASP.NET, there was quite a ton of back-and-forth in regards to query design, db schemas, and file/class structure to make it work well and somewhat adhere to proper design patterns.

The datastax driver provides a nice mapper for directly using class objects without having to write verbose queries (for all the CRUD operations), which was extensively used in the earlier version of the project.

However, due to multi-client issues, adding `IF EXISTS` and similar clauses to queries was necessary, which led us back to writing verbose queries for everything aside from very basic selects.

There were also a few issues due to connection security and certificates, but with a few additional parameters and rule exceptions we made it work both for test requests and the frontend.

# How to run

## Step 1.

`docker compose up` in `Backend` and wait around a minute until it starts up fully (it will download all the containers and set up the schema)

## Step 2.

Aquire `Visual Studio` and open `SploinkyAPI.sln` in `Backend/SploinkyAPI` <br>
And make sure the `CassandraCSarpDriver NuGet package by DataStax` is installed (if it isn't already in the solution) <br>
You **might** also need to install the `ASP.NET API` project type in the `Visual Studio Installer` <br>

When you run it you can test individual api requests on swagger if you want <br>
All available requests are defined in `ReservationController.cs` <br>

## Step 3.

`npm install` and then `npm run dev` in `Frontend` (react required)

# Stress tests

They're in `Backend/Tests/StressTests` as standalone `.py` scripts, make sure backend is running, also keep in mind the default schema makes a few reservations on container startup (some 409 errors might show up due to seats being taken)
