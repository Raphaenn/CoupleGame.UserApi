using CoupleGame.Backend.Auth;
using CoupleGame.Backend.Auth.Context;
using CoupleGame.Backend.Auth.Interfaces;
using CoupleGame.Backend.Auth.Repositories;
using CoupleGame.Backend.Auth.Services;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Rewrite;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options =>
{
    options.Conventions.Add(new RouteTokenTransformerConvention(new SlugifyParameterTransformer()));
});

builder.Services.AddScoped<StoreDataContext>();

builder.Services.AddTransient<IGoogleService, GoogleService>();
builder.Services.AddTransient<IJwtService, JwtService>();
builder.Services.AddTransient<IAppleService, AppleService>();
builder.Services.AddTransient<IFacebookService, FacebookService>();
builder.Services.AddTransient<IUserRepository, UserRepository>();

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

app.UseAuthorization();

app.MapControllers();

app.Run();
