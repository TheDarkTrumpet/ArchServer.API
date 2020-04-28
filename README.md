# ArchServer.API

![.NET Core](https://github.com/TheDarkTrumpet/ArchServer.API/workflows/.NET%20Core/badge.svg)

# Purpose

This project is a personal project that can be used as reference or largely out of box for people with similar use cases.  The purpose of this repo, for myself, is to reduce the frustration I have regarding task tracking, notes, and the like when it comes to the various systems I use.  A large part of my goal here is to allow for better reporting - cross system, as needed.  This project aims to do the following:
1.  Create a local cache of data from the various systems used.
2.  Create an endpoint that can be contacted easily through Emacs to create linkages in my notes.
3.  The automatic task/item creation in the time tracking systems, depending on the source system.
    - VSTS -> Kimai
	- Teamwork -> Toggl
4.  The facilitation of reporting on time.  Largely this would be in Excel, but would utilize data connectors within Excel.
5.  Plug into systems such as Zapier and Huginn
6.  Provide some simple Nuget packages that can be used in other applications (limited scope)

Long with the above goals (some of which are done, and some not), this solution also aims to:
1.  Provide potential employers with a real-life view of my skill set.  This application touches on: Web Development, API development, Unit and Integration Testing, performance and optmization and advanced C# concepts such as Reflection.
2.  Serve as a reference for the destination APIs.  There is some (intentional) code duplication here between the APIs and the model used to save.  This is intentional to keep the application accessible for people.
3.  Provide a test-bed for new technologies and design ideas.  My aim is to keep this repository easy to understand and work with, but also provide a place where I can test new ideas (such as the heavy reflection use in the Entity Framework operations) in a way that is not only immediately useful, but also with immediate feedback.
4.  Provide a reference to various coding styles and paradigms.  We all tend to hit Stackoverflow when we don't know something, but having a working example can be invaluable.  This repository is aimed to provide that reference, not only for myself, but for others.

# Getting Started and Navigating

Largely speaking, each subfolder/project has a dedicated readme associated with it.  This repository is as much for teaching, as it is for actual use.  I recommend navigating to each project individually and reading what the purpose for each is.  From a very high level, the following projects exist and their pupose explained (very shortly) is:

| Project     | Description                                                                                                        |
|-------------|--------------------------------------------------------------------------------------------------------------------|
| BulkCache   | A console application that utilizes lib* (minus tests) to insert/update the records                                |
| libAPICache | This is the Entity Framework model for this solution.  It contains, largely, the CRUD operations                   |
| libKimai    | This is the API library for Kimai (Time tracking), which relies on direct MySQL connections (their API is limited) |
| libToggl    | This is the API library for Toggl (Time tracking), which utilizes their API.                                       |
| libVSTS     | This is the API library for Visual Studio Team Services (Azure DevOps, primarily)                                  |
| libTeamwork | This is the API library for Teamwork (task tracking system)                                                        |

Getting started with the application is fairly simple.  In any directory you see an `appsettings.json` file, means that the settings are loaded from these types of files.  But, there's a catch.  There are two files that aren't included in the repository:
* appsettings.Development.json
* appsettings.Release.json

These files are very important.  The `appsettings.json` itself is meant as a guide for the other two files, not that these are used!  There's more details in the subprojects, but I felt it important to mention this.  For example, if you run migrations in libAPICache, under development build (-c Development, or default), it **WILL** look for `appsettings.Development.json`.

Outside that, these are all built in .NET Core 3.1.  You can use Linux, Windows, or OSX to run these.  Most everything is included as Nuget packages (although Npm or Bower will eventually make an appearance).  Docker is optional, but recommended.  There are `Dockerfile` for build, and compose files for getting up and running fairly easily.  Long term, I may make this a bit simpler and include MSSQL in the `docker-compose` file, along with these - but right now that's unlikely.  Largely speaking, this is due to the nature of the API calls. They need to be configured before running.

# Contributing

Depending on the feature/change, contributions are more than welcome.  Please feel free to create an issue, and a merge request with your changes.  Please take in consideration some high level, important goals:
1.  Keep it simple.  Aim to keep your code easy to understand, or at least somewhat easy.  Really complicated, but efficient code, is nice to also learn from - but not the aim for this repo.  If your code is on the more complicated side, please comment appropriately and/or create documentation.  My goal is to eventually include a wiki that can be linked to that would provide some further references.
2.  Keep use cases scoped.  The API calls aren't necessarily designed to replicate the entire model of their source system.  If you really *need* a property, please free to add it - but please don't add what you don't absolutely need.  This is again for simplicity sake.
3.  Write tests.  The goal is to allow everything here to be tested or at least testable.  Before this becomes public (and likely before you read this), my goal is to have near everything tested with at least unit tests. Integration tests will likely be lacking, and there's not much I can do about that.  The fact we're dealing with real-life apis means that integration tests contained here will likely not happen.  That said, I do have my own pipeline locally to test.
