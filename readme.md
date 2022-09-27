# TodoList-API

My backend version of the infamous *Todo-list* application.

## Technical information

The API was built using ASP.NET Core Web API on .NET 6 and it was primarily a learning playground for familiarizing with the concepts of .NET Core.

1. [Database](#database)
2. [Endpoints](#endpoints)
3. [How to run](#how-to-run)

### Database

The main focus of the API is to perform CRUD operations against a database.

Initially, the database of choice was a local relational database within SQL Server. The CRUD operations were performed using the micro ORM [Dapper](https://github.com/DapperLib/Dapper), known for being performant and for providing ease of development over [ADO.NET](https://learn.microsoft.com/en-us/dotnet/framework/data/adonet/ado-net-overview).

Later, while reviewing the project, it became apparent that this project is a rather bad use case for a relational database. Thus, the option to use [Realtime Database](https://firebase.google.com/docs/database?hl=en&authuser=0) from Firebase was implemented as well. *Realtime Database* provides a NoSQL cloud-hosted database, to store and access the data, while the API ensures data integrity.

### Endpoints

The endpoints provided by the API are the following:

| Method     | URL             | Description |
| ---------- | -----------     |     ---     |
| `GET`      | /api/todos      | Retrieve all Todos
| `POST`     | /api/todos      | Create a new Todo
| `GET`      | /api/todos/{id} | Retrieve details for Todo with id = {id}
| `DELETE`   | /api/todos/{id} | Delete Todo with id = {id}
| `PUT`      | /api/todos/{id} | Update details for Todo with id = {id}

Both `POST:/api/todos` and `PUT:/api/todos/{id}` require a Todo object in the request body with the following format:

```json
{
  "title": "Create Readme",
  "dueTo": {
    "date": "29/09/2022",
    "time": "22:30"
  },
  "priority": "Low"
}
```

and with `"priority"` being an array of the following choices `[ Low, Medium, High ]`.

### How to run

The following steps indicate how to launch the API:

1. In the `appsettings.json`, replace the *server* value in *SqlConnection* with your own SQL Server or the *Realtime Database* base URL with your own. (Check [this](https://firebase.google.com/docs/database/web/start?authuser=0&hl=en) link for instructions on how to create a Realtime Database).
2. Choose between SQL Server and Firebase by registering the correspoding repository in the DI container within `program.cs`.

    For SQL Server:

    ```c#
    builder.Services.AddScoped<ITodoRepository, TodoDapperRepository>();
    ```

    For Firebase:

    ```c#
    builder.Services.AddScoped<ITodoRepository, TodoFirebaseRepository>();
    ```

3. Run the app.
