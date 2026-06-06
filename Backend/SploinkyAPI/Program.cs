using Cassandra.Mapping;
using SploinkyAPI.Controllers;
using SploinkyAPI.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//setting up a db connection and establishing a session
// TODO: read db address and port from some config yaml
Cassandra.ICluster cluster = Cassandra.Cluster.Builder().AddContactPoint("127.0.0.1").WithPort(9042).Build();
Cassandra.ISession session = cluster.Connect("reservations");

builder.Services.AddSingleton(cluster);
builder.Services.AddSingleton(session);

// config for model mappings
MappingConfiguration.Global.Define<ModelMappings>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}



app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
