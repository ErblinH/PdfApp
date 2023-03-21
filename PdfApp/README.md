# Interview Assignment

### Introduction

In this assignment you'll be creating a PDF generation API. This API needs to be able to convert HTML into a PDF document. 

> ***NOTE*** Please read this entire document before starting the assignment

**Functional requirements**

- JSON input & ouput
- Input validation
- API Key validation:
  - API should be call with the following header: X-API-KEY: 7a8a7cd837b042b58b56617114f4d3d7

- Input requirements
  - Html to convert (Base64 encoded)
  - Page Orientation
  - Page ColorMode
  - Page PaperSize
  - Page Margins
- Output requirements
  - PDF Document (Base64 encoded)
  - PDF Document size

**Technical requirements**

- .NET 6
- .NET Dependency Injection
- .NET minimal API: https://learn.microsoft.com/en-us/aspnet/core/fundamentals/minimal-apis?view=aspnetcore-6.0
- Docker Support
- Clean architecture
- Use NuGet package: https://www.nuget.org/packages/Haukcode.WkHtmlToPdfDotNet

**Getting Started**

Attached to this assignment you'll find two things:

- A starter solution with some boilerplate code (possibly contains bugs)
- A Postman collection to test the application

***Good luck!***
