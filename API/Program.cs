using API.Middleware;
using Docusign.Repository.DataBase.Conexion;
using API.RegisterInterface;
using API.Routes;

var builder = WebApplication.CreateBuilder(args);
const string origins = "CorsPolicy";


builder.Services.Register();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



builder.Services.AddDbContext<DB_ADPRO>();


builder.Services.AddCors(options =>
{
    options.AddPolicy(origins,
        builder => builder
        //.WithOrigins("http://localhost:3000","*").
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader());
});

builder.Services.AddAuthorization();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();




app.UseCors(origins);

//app.UseHttpsRedirection();
app.UseCors(origins);

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware(typeof(AuthenticationMiddleware));



//endpoint api
app.RegisterRoutes();

app.Run();
