using API.Middleware;
using API.Routes.MapDocusign;
using Docusign.Repository.DataBase.Conexion;
using API.DI;
using API.Routes.MapWeatherForecast;
using API.Routes.MapAutodesk;
using API.Routes.MapAutoDesk;

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

//{
app.UseSwagger();
app.UseSwaggerUI();
//}

//app.UseHttpsRedirection();

if (!app.Environment.IsDevelopment())
    app.UseMiddleware(typeof(AuthenticationMiddleware));

app.UseRouting();

app.UseCors(origins);
app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware(typeof(AuthenticationMiddleware));

app.RegisterDocusign();
app.RegisterDocusignDS();
app.RegisterWeatherForecast();



app.RegisterDataManagement();
app.RegisterModelDerivative();
app.RegisterOAuth();
app.RegisterProyectos();
app.RegisterTrimble();


app.RegisterDocusignDS();
app.Run();

