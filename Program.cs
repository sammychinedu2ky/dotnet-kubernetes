using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddHealthChecks().AddCheck<MongoHealthCheck>("mongo");
builder.Services.AddSingleton<IMongoCollection<Person>>(s =>
{
    var connectionString = builder.Configuration.GetSection(MongoConnectionSettings.position).Get<MongoConnectionSettings>().MONGO_URL;
    var client = new MongoClient(connectionString);
    var database = client.GetDatabase("test");
    return database.GetCollection<Person>("people");
});

var app = builder.Build();
app.MapHealthChecks("/health/ready", new HealthCheckOptions
{
    Predicate = hc => hc.Name.Contains("mongo"),
    //AllowCachingResponses = false
});
app.MapHealthChecks("/health/live", new HealthCheckOptions
{
    Predicate = _ => false
});
app.MapGet("/", () => "Hello world Chik🥂");

app.MapGet("/{name}", async (string name, IMongoCollection<Person> collection) =>
{
    // find users bearing same name
    app.Logger.LogInformation("Finding users with name {name}", name);
    var result = await collection
    .Find(Builders<Person>.Filter.Eq(x => x.name, name))
    .Project(Builders<Person>.Projection.Expression(x => new Person(x.name, x.age)))
    .ToListAsync();

    // return 404 if not found
    if (result == null) return Results.NotFound();

    //return result
    return Results.Ok(result);
});
app.MapPost("/create", async (Person person, IMongoCollection<Person> collection) =>
{
    //crete user
    await collection.InsertOneAsync(new Person(person.name, person.age));

    // return 202
    return Results.Accepted();
});

app.Run();
internal record Person(string name, int age);

