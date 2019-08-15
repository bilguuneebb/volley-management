# README #

Welcome to Volley Management project repository

## Status

[![Build status](https://ci.appveyor.com/api/projects/status/1ueugqjgg8qv7ajm?svg=true)](https://ci.appveyor.com/project/VolleyManagement/volley-management) [![Build Status](https://dev.azure.com/VolleyManagement/%CE%94%CE%B9%CE%B1%CF%87%CE%B5%CE%B9%CF%81%CE%B9%CF%80%CE%B7/_apis/build/status/VolleyManagement.volley-management?branchName=master)](https://dev.azure.com/VolleyManagement/%CE%94%CE%B9%CE%B1%CF%87%CE%B5%CE%B9%CF%81%CE%B9%CF%80%CE%B7/_build/latest?definitionId=1&branchName=master)

🚧 ⬇ Not actual. Will be fixed soon.

![Sonar Quality Gate](https://sonarcloud.io/api/project_badges/measure?project=volley-management&metric=alert_status) ![LoC](https://sonarcloud.io/api/project_badges/measure?project=volley-management&metric=ncloc) ![Coverage](https://sonarcloud.io/api/project_badges/measure?project=volley-management&metric=coverage)

![Maintainability](https://sonarcloud.io/api/project_badges/measure?project=volley-management&metric=sqale_rating) ![Security](https://sonarcloud.io/api/project_badges/measure?project=volley-management&metric=security_rating) ![Reliability](https://sonarcloud.io/api/project_badges/measure?project=volley-management&metric=reliability_rating)

## What is this repository for? ##

The goal of the project is to create system to manage Volleyball sport events for amateur leagues.
Also it serves as a training ground to try different engineering and architectural approaches to learn in a safe environment.
See more history here: [Volley Management v3 - Διαχειριπη](https://diachenko.info/volley-management-v3-diakheiripi/)

Current Version: v3

## How do I get set up? ##

### Summary of set up ###

Project uses .NET Core and Angular at the moment. Recommended tools are VS 2019 (any edition) and VS Code. Although it should run using any other setup , I hope :)

Target framework is .NET Core 3.0.

### Dependencies ###

All .NET dependencies are managed via NuGet. Angular uses NPM.

### Database configuration (🚧 outdated) ###

Database is automatically deployed to server first time you run application. Specify proper connection string in Web.config file in VolleyManagement.UI project.

By default it is configured to use SQL Server 2016 LocalDB. But you can can change altering connection string.

### How to run tests (🚧 outdated) ###

Project uses MSTest framework. Tests can be triggered via Visual Studio test menu.

🚧 ToDo: Add guide how to run Code Coverage using JetBrains dotCover CLI runner

### Deployment instructions ###

To run code locally it is recommended to use Docker to spin up local instance and test. This options is the closest to the production setting.
You have few options there:

* Use VS Container Tools and run start project using `Docker` launch profile. VS injects few useful items like debugger so you'll be able to debug code running inside Docker.

* Build and run container manually. See `/dev-env` folder for some scripts.

If you want to shorten development cycle you can run application locally. Use `VolleyManagement.API` launch profile. It will execute `dotnet run`.

In production we use Azure DevOps Pipeline to build and deploy code.

## Additional references ##

* [License](/LICENSE.md)
* [Contribution Guidelines](/CONTRIBUTING.md)
* [Code of Conduct](/CODE_OF_CONDUCT.md)

## Who do I talk to? ##

* All questions should be directed to Sergii Diachenko(mailto:sdiachenko AT outlook DOT com)
