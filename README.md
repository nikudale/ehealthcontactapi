# ehealthcontactapi
this is REST API built with basic functionality of contact details using  asp.net core 3.1, entity framework core and xUnit test.

## Dependencies
- [ASP.NET Core 3.1]
- [Entity Framework Core]
- [Entity Framework Sql Server Provider]
- [Entity Framework In-Memory Provider]
- [AutoMapper]
- [Moq]
- [xUnit]
- [Serilog]

## How to Build API Project
install .net core  
open command prompt and goto api directory (`src\EHealth.ContactApp\EHealth.Api.Contacts`)
run below commands
```
dotnet restore
dotnet build
dotnet ef database update (if not present apply migration to database)
dotnet run
```
Alternate option with Visual studion open solution (`src\EHealth.ContactApp\EHealth.ContactApp.sln`) and run


Navigate to ```https://localhost:44393/api/contact/check``` to check if the API is working

## How to Test API Project
to test endpoints please open [Postman](https://www.getpostman.com/).
- Get all contacts [GET] (`https://localhost:44393/api/contact/`)
  --f.x `https://localhost:44393/api/contact/1` \
- Get single contact [GET] (`https://localhost:44393/api/contact/{id}`)
- Create Contact [POST] (`https://localhost:44393/api/contact/`) with passing json object in Body --> raw \
  --f.x  ``{	"firstName": "Test","lastName": "Abc","email": "abc1123@test.com","phoneNumber": "1111111111","status": 1}``
- Update Contact [PUT] (`https://localhost:44393/api/contact/`) with passing json object in Body --> raw \
  --f.x  ``{ "Id":1, "firstName": "TestUpdate","lastName": "Abc","email": "abc1123@test.com","phoneNumber": "1111111111","status": 1}``
- Delete Contact [DELETE] (`https://localhost:44393/api/contact/{id}`) \
  --f.x `https://localhost:44393/api/contact/1` \
  NOTE: delete functionality just update do soft delete with update status to 0 \
  
  exception scenarios handle and updated log exception, output wrap and provided in below format \
  {  \
     Result,    ==> output \
     Message,   ==> message if error \
     IsSuccess  ==> success or failed \
  }
  
## How to Build and Test Contact Test Project
open command prompt and goto api directory (`/src/EHealth.ContactApp\EHealth.Api.Contacts.Test`) \
currently test update for service layer.
run below commands
```
dotnet restore
dotnet build
dotnet test
```

## Database Dependencies
this project configure with Sql Server local database.
- Add test sample insert data to ContactDB (script available in (`src\DatabaseScripts\insert_table_contact`)
- create table `Log` for Serilog (script available in (`src\DatabaseScripts\create_table_log`)

## Publish to Azure
this application with database is deploy to azure and available for testing (``https://ehealthcontactapi.azurewebsites.net/api/contact/``)
