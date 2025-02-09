using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using TodoApi;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ToDoDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("ToDoDB"), ServerVersion.Parse("8.0.41-mysql")));


builder.Services.AddCors(options =>
    options.AddPolicy("AllowAll", p => p
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader()
));

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My Minimal API", Version = "v1" });
});

var app = builder.Build();
 app.UseCors("AllowAll");

// builder.Services.AddSwaggerGen();



//if (app.Environment.IsDevelopment())
// {
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My Minimal API V1");
        c.RoutePrefix = string.Empty; 
    });
//}

app.MapGet("/items", async (ToDoDbContext dbContext) =>
{
    return  await dbContext.Items.ToListAsync();
});

app.MapPost("/items", async (ToDoDbContext dbContext, Item newItem) =>
{
    dbContext.Items.Add(newItem);
    await dbContext.SaveChangesAsync();
    return Results.Created($"/{newItem.Id}", newItem);
});

app.MapPut("/items/{id}", async (ToDoDbContext dbContext, int id, Item updatedItem) => {
  
    var existingItem = await dbContext.Items.FindAsync(id);
    if (existingItem == null)
    {
        return Results.NotFound();
    }
    existingItem.Name = updatedItem.Name;
    existingItem.IsComplete = updatedItem.IsComplete;
    await dbContext.SaveChangesAsync();
    return Results.Ok( existingItem);
}
);

app.MapDelete("/items/{id}", async (ToDoDbContext dbContext, int id) =>{
 var item = await dbContext.Items.FindAsync(id);
    if (item == null)
    {
        return Results.NotFound();
    }

    dbContext.Items.Remove(item);
    await dbContext.SaveChangesAsync();
    return Results.NoContent();
});


app.Run();
