using Microsoft.EntityFrameworkCore;
using Portifolio.API;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSqlServer<PortifolioContext>(builder.Configuration.GetConnectionString("ServerConecttion"));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors(p => p
    .AllowAnyHeader()
    .AllowAnyOrigin()
    .AllowAnyMethod());

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapPost("/contacts", async (PortifolioContext context, Contact contact) =>
{
    await context.Contacts.AddAsync(contact);
    await context.SaveChangesAsync();

    return Results.Ok(contact);
})
.WithOpenApi();

app.MapGet("/contacts", async (PortifolioContext context) =>
{
    var contacts = await context.Contacts.ToListAsync();

    return Results.Ok(contacts);
});

app.Run();

public record Contact
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Subject { get; set; }
    public string Message { get; set; }
    public DateTime Date { get; set; }
}
