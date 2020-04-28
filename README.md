# ArchServer.API

![.NET Core](https://github.com/TheDarkTrumpet/ArchServer.API/workflows/.NET%20Core/badge.svg)

## Purpose

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

## Getting Started and Navigating

Largely speaking, each subfolder/project has a dedicated readme associated with it.  This repository is as much for teaching, as it is for actual use.  I recommend navigating to each project individually and reading what the purpose for each is.  From a very high level, the following projects exist and their pupose explained (very shortly) is:

| Project     | Description                                                                                                        |
|-------------|--------------------------------------------------------------------------------------------------------------------|
| BulkCache   | A console application that utilizes lib* (minus tests) to insert/update the records                                |
| libAPICache | This is the Entity Framework model for this solution.  It contains, largely, the CRUD operations                   |
| libKimai    | This is the API library for Kimai (Time tracking), which relies on direct MySQL connections (their API is limited) |
| libToggl    | This is the API library for Toggl (Time tracking), which utilizes their API.                                       |
| libVSTS     | This is the API library for Visual Studio Team Services (Azure DevOps, primarily)                                  |
| libTeamwork | This is the API library for Teamwork (task tracking system)                                                        |

