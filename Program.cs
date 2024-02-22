using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using PizzaStore.Models;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("Pizzas") ?? "Data Source=Pizzas.db";
builder.Services.AddSqlite<PizzaDb>(connectionString);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDbContext<PizzaDb>(options => options.UseInMemoryDatabase("items"));
builder.Services.AddSwaggerGen(c =>{
           c.SwaggerDoc("v1", new OpenApiInfo { Title = "Todo API", Description = "Keep track of your tasks", Version = "v1" });

});

var app = builder.Build();

app.MapGet("/", () => "Hello World!");
app.MapGet("/pizzas",async (PizzaDb db) => await db.Pizzas.ToListAsync());
app.MapGet("/pizzas/{id}", async (PizzaDb db,int id) => await db.Pizzas.FirstOrDefaultAsync(p => p.Id == id));
app.MapPost("/pizzas", async (PizzaDb db, Pizza pizza) => {
    await db.Pizzas.AddAsync(pizza);
    await db.SaveChangesAsync();
    return Results.Created($"/pizza/{pizza.Id}",pizza);
});
app.MapPut("/pizzas", async (PizzaDb db,Pizza updatePizza, int id) =>{
    var p = await db.Pizzas.FindAsync(id);
    if (p is null) return Results.NotFound();
    p.Name = updatePizza.Name;
    p.Description = updatePizza.Description;
    await db.SaveChangesAsync();
    return Results.NoContent();
} );
app.MapDelete("/pizzas/{id}", async (PizzaDb db,int id) => {
        var p = await db.Pizzas.FindAsync(id);
        if(p is null) return Results.NotFound();
         db.Pizzas.Remove(p);
         await db.SaveChangesAsync();
         return Results.Ok();
});


app.UseSwagger();
app.UseSwaggerUI(c => {
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Todo API V1");
});
app.Run();
