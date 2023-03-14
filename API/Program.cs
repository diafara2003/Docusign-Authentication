using API.Middleware;
using Docusign.Repository.DataBase.Conexion;

var builder = WebApplication.CreateBuilder(args);
const string origins = "CorsPolicy";

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddDbContext<DB_ADPRO>();


builder.Services.AddCors(options =>
{
    options.AddPolicy(origins,
        builder => builder
        .WithOrigins("*")
        .AllowAnyMethod()
        .AllowAnyHeader());
});

builder.Services.AddHttpContextAccessor();


builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}

app.UseHttpsRedirection();


app.UseRouting();

app.UseCors(origins);
app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware(typeof(AuthenticationMiddleware));



app.Run();
