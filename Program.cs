using System.ComponentModel;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using project_cursed.Models;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.CustomSchemaIds(x => x.GetCustomAttributes(false).OfType<DisplayNameAttribute>().FirstOrDefault()?.DisplayName ?? x.Name);
});

string connectionString = builder.Configuration["ConnectionString:DefaultConnection"];

builder.Services.AddDbContext<SQLiteContext>(options =>
{
    options.UseSqlite(connectionString);
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(swaggeroptions =>
    {
        swaggeroptions.SwaggerEndpoint("v1/swagger.json", "v1 api");
        swaggeroptions.RoutePrefix = "swagger";
    });
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
