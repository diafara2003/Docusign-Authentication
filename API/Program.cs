using API.Middleware;
using Repository.DataBase.Conexion;
using API.RegisterInterface;
using API.Routes;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
const string origins = "CorsPolicy";



//builder.Services.ConfigureHttpJsonOptions(c =>
//{
//    c.SerializerOptions.PropertyNamingPolicy = new NullPropertyNamingPolicy();
//});



builder.Services.AddHttpContextAccessor();

builder.Services.Register();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    // Define el esquema de seguridad global
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Es necesario el token del marco ERP para poder ejecutar los API en lista",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",

    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer",
                }
            },
            new string[] {}
        }
    });

    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "ADPRO API",
        Version = "v1",
        Description = "A continuación se lista todos los API que se tienen por parte del ADPRO",

    });

    c.OrderActionsBy(x => x.GroupName);


});




builder.Services.AddDbContext<DB_ADPRO>();


builder.Services.AddCors(options =>
{
    options.AddPolicy(origins,
        builder => builder
       // .WithOrigins("http://localhost:3000","*")
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
