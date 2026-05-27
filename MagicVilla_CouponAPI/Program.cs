using Dapper;
using MagicVilla_CouponAPI.DTO;
using MagicVilla_CouponAPI.Models;
using Microsoft.Data.SqlClient;
using System.Reflection.Metadata.Ecma335;
using System.Xml.Linq;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//==============================================================================================

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");


app.MapGet("/Prueba", () => "Hola Mundo desde el endpoint de prueba");


app.MapGet("/GetCustomers", async () =>
{
    using (var connection = new SqlConnection(connectionString))
    {
        var coupons = await connection.QueryAsync<Cliente>("SELECT * FROM Cliente");
        return Results.Ok(coupons);
    }
});

app.MapGet("/GetClienteId/{id}", async (Guid id) => 
{
    using (var connection = new SqlConnection(connectionString))
    {
        try
        {
            var parametro = new
            {
                Id = id
            };

            var cliente = await connection.QueryFirstOrDefaultAsync<Cliente>("SELECT Id, CompanyName, ContactName, ContactTitle FROM Cliente WHERE Id = @Id", parametro );
            return (cliente == null)? Results.NotFound("El cliente no se encuentra") : Results.Ok(cliente);
            
        }
        catch (Exception ex)
        {
            return Results.BadRequest(ex);
        }
    }
});

app.MapPost("/AddCliente", async (ClienteDTO cliente) => 
{
    using var connection = new SqlConnection(connectionString);

    var parametro = new
    {
        Id = Guid.NewGuid(),
        CompanyName = cliente.CompanyName,
        ContactName = cliente.ContactName,
        ContactTitle = cliente.ContactTitle
    };

    string sql = @"INSERT INTO Cliente (Id, CompanyName, ContactName, ContactTitle)
                   VALUES (@Id, @CompanyName, @ContactName, @ContactTitle)";

    int rowsAffected = await connection.ExecuteAsync(sql, parametro);

    return rowsAffected > 0
        ? Results.Created($"/elementos/{parametro.Id}", cliente)
        : Results.BadRequest("No se pudo insertar el elemento");
});



app.Run();

