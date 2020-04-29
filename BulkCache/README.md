# BulkCache

# Purpose

This is a .NET console application that utilizes the API calls, and the Entity Framework backend to pull the entries from the API and push them into SQL Server.

# Getting Started and Navigating

## Preparation and Execution
To run these, you can execute: `dotnet run` from within this directory.  This will run in `DEBUG`, and will reference the `appsettings.Development.json` file.  To run this, that file has to exist, so...
1. Copy `appsettings.json` to `appsettings.Development.json`
2. Edit `appsettings.Development.json`, and replace each entry that is enclosed in brackets (< and >).

Right now, there's no selective operation on what API sources are cached, so all API sources need to be defined.

## Navigating

`Program.cs` is the entry point for this project.  Most of the functionality is in the `lib/BulkCache.cs` file.  The overall logic is pretty simple. We have a main entry, and submethods for each.  This is a good way to view how to pass parameters that are needed to get these libraries to work.  To better allow for expansion, we make heavy use of the config file.