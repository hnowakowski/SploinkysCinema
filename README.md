# How to run tutorial

## Step 1.

`docker compose up` in `Backend` and wait around a minute until it starts up fully

## Step 2.

Aquire Visual Studio and install the CassandraCSarpDriver NuGet package by DataStax (if it isn't already in the solution)

When you run it you can test individual api requests on swagger if you want

## Step 3.

`npm run dev` in `Frontend`

# Stress tests

They're in `Backend/Tests/StressTests` as standalone `.py` scripts, make sure backend is running, also keep in mind the default schema makes a few reservations on container startup
