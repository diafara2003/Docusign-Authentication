using API.Middleware;
using API.Routes.MapDocusign;
using Docusign.Repository.DataBase.Conexion;
using API.DI;
using API.Routes.MapWeatherForecast;

var builder = WebApplication.CreateBuilder(args);
const string origins = "CorsPolicy";


builder.Services.Register();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


//builder.Services.Register();
builder.Services.AddDbContext<DB_ADPRO>();


builder.Services.AddCors(options =>
{
    options.AddPolicy(origins,
        builder => builder
        .WithOrigins("*")
        .AllowAnyMethod()
        .AllowAnyHeader());
});




builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
app.UseSwagger();
app.UseSwaggerUI();
//}

//app.UseHttpsRedirection();
//app.UseMiddleware(typeof(AuthenticationMiddleware));

app.UseRouting();

app.UseCors(origins);
app.UseAuthentication();
app.UseAuthorization();



app.RegisterDocusign();
app.RegisterDocusignDS();
app.RegisterWeatherForecast();



app.Run();

