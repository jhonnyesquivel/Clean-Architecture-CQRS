 # Post and Comments API Technical Test
 
This is my technical test for the Zemoga company, a web api was made with two main endpoints and one authentication using the Microsoft IdentityServer api. The development time used was around 24 hours in total, although I have the capacity to make a ReactJS or Next JS frontend, I did not have enough time to finish that stage of the test. The architecture proposed is a CleanArquitecture with the CQRS pattern.
## Technologies

* [ASP.NET Core 7](https://docs.microsoft.com/en-us/aspnet/core/introduction-to-aspnet-core)
* [Entity Framework Core 7](https://docs.microsoft.com/en-us/ef/core/)
* [MediatR](https://github.com/jbogard/MediatR)
* [AutoMapper](https://automapper.org/)
* [FluentValidation](https://fluentvalidation.net/)

## Getting Started

The easiest way to get started is follow the next steps:
1. Clone repositoru `git clone https://github.com/jhonnyesquivel/ZemogaTest.git` 
2. Cofigure the connection string in the appsetting.json file `"DefaultConnection": "Server=<server>;Database=Zemoga_TestDb;Trusted_Connection=True;TrustServerCertificate=True;Encrypt=False;;MultipleActiveResultSets=true;"`
3. Run the command `dotnet run -p .\src\WebAPI\` to launch the project
4. Navigate to `https://localhost:5001/api/index.html`

### Database Configuration

Verify that the **DefaultConnection** connection string within **appsettings.json** points to a valid SQL Server instance. 

When you run the application the database will be automatically created (if necessary) and the latest migrations will be applied.

### Database Migrations

To use `dotnet-ef` for your migrations first ensure that "UseInMemoryDatabase" is disabled, as described within previous section.
Then, add the following flags to your command (values assume you are executing from repository root)

* `--project src/Infrastructure` (optional if in this folder)
* `--startup-project src/WebAPI`
* `--output-dir Persistence/Migrations`

For example, to add a new migration from the root folder:

 `dotnet ef migrations add "SampleMigration" --project src\Infrastructure --startup-project src\WebUI --output-dir Persistence\Migrations`

## Consuming API

The application is using IdentityServer by a client credentias model, you can import the postman file to your client to see the endpoints.

Lets import the file `Zemoga_test.postman_collection.json` to your postmant to see how to execute easily the both endpoints.

### Authentication POST: /connect/token

Endpoint `https://localhost:5001/connect/token`

There are three users to test the distinct endpoints

| Username | Password | Role |
| -------- | -------- | ---- |
| writer    | Password1!  | writer |
| editor    | Password1!  | editor  |
| public   | Password1!  | public  |

#### Request

```curl: 
curl --location --request POST 'https://localhost:5001/connect/token' \
--header 'Content-Type: application/x-www-form-urlencoded' \
--data-urlencode 'client_id=ZemogaWeb' \
--data-urlencode 'grant_type=password' \
--data-urlencode 'username=<UserName>' \
--data-urlencode 'password=<Password>'
```

##### Response
```json: 
{
    "access_token": "<jwt token>",
    "expires_in": 3600,
    "token_type": "Bearer"
}
```

### POST endpoints /api/posts

To consume the Post and Comment endpoints is needed take fron the `/connect/token` endpoint the jwt token and send it througth the authorization header

| Endpoint  | Method | Description |
| ------------- | ------------- | ------------- |
| /api/posts  | GET  | Returns a list of published posts  |
| /api/posts/mine/ | GET  | Returns list of posts created by the authenticated user |
| /api/posts  | POST  | Creates a new post  |
| /api/posts:postId  | PUT  | Allow updated a post, used by writer and editor roles  |

Example:
```curl:
curl --location --request POST 'https://localhost:5001/api/posts' \
--header 'Authorization: Bearer <jwt token>' \
--header 'Content-Type: application/json' \
--header 'Cookie: .AspNetCore.Antiforgery.hY16-X1FcPE=CfDJ8MUpIxJMjUBFmgU0Ra_Jt0MIx03ECiP2w4VARAkGJnvsaSBt9MbtSgx_3saUc7eG_sbk2NTD-kRB_bp3dcjwzhcBIB_Mcda0w4AAzXESxmLfbV1VGO1MFw_9_s3bFWq0fhagC4fsgg9P9nWK3SxRP_o' \
--data-raw '{
    "title":"The Garden of Lost Memories Test 2",
    "content":"When a young woman is sent to take care of the neglected garden of a mysterious family, she discovers a long-forgotten history filled with secrets, lies and forgotten tragedies. As she digs deeper, she learns more about the family and the garden'\''s dark past.",
    "status": "draft"
}'
```

### COMMENT endpoints /api/comments
| Endpoint  | Method | Description |
| ------------- | ------------- | ------------- |
| /api/comments  | GET  | Get the list of comments in a post. This has query params  |
| /api/comments  | POST  | Creates a comment in  specific post  |

Example:

```curl:
curl --location --request GET 'https://localhost:5001/api/comments?PostId=<postid>&PageNumber=1&PageSize=10' \
--header 'Authorization: Bearer <jwttoken>' \
```


## Architecture
This project has a clean architecture based in CQRS pattern.

### Domain

This will contain all entities, enums, exceptions, interfaces, types and logic specific to the domain layer.

### Application

This layer contains all application logic. It is dependent on the domain layer, but has no dependencies on any other layer or project. This layer defines interfaces that are implemented by outside layers. For example, if the application need to access a notification service, a new interface would be added to application and an implementation would be created within infrastructure.

### Infrastructure

This layer contains classes for accessing external resources such as file systems, web services, smtp, and so on. These classes should be based on interfaces defined within the application layer.

### WebAPI

This layer is where live the application controllers/endpoints. This layer depends on both the Application and Infrastructure layers, however, the dependency on Infrastructure is only to support dependency injection. Therefore only *Startup.cs* should reference Infrastructure.
