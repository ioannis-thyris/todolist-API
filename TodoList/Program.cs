using System.Text.Json.Serialization;
using TodoListAPI.AppContext.Sql;
using TodoListAPI.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper(typeof(Program));

//// Add cloud database
builder.Services.AddHttpClient();

// Add SQL Server database
builder.Services.AddTransient<ISqlContext, DapperContext>();

// Register the repository depending on if you want to access SQL Server or Firebase.
builder.Services.AddScoped<ITodoRepository, TodoDapperRepository>();

builder.Services.AddCors(c =>
{
    c.AddPolicy("default", options => options.AllowAnyMethod().AllowAnyHeader().AllowAnyOrigin());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseCors("default");

app.MapControllers();

app.Run();
