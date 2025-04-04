using Microsoft.EntityFrameworkCore;
using ShoppingApp2.Business.Operations.User;
using ShoppingApp2.Data.Context;
using ShoppingApp2.Data.Repositories;
using ShoppingApp2.Data.UnitOfWork;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ���nc� b�y�k
var connectionString = builder.Configuration.GetConnectionString("Default");
builder.Services.AddDbContext<ShoppingApp2DbContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>)); // Generic oldu�u i�in TypeOf
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IUserService, UserManager>();

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
