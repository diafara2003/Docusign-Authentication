using Addons.Services;
using Docusign.Repository.Peticion;
using Docusign.Services;
using HandleError;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Repository.DataBase.Model;
using Services.BIM360Services;
using Services.Inventarios;
using System.Reflection;
using System.Text.Json;
//using Services.Inventarios;

namespace API.RegisterInterface
{
    public class NullPropertyNamingPolicy : JsonNamingPolicy
    {
        public override string ConvertName(string name)
        {
            return name;
        } // Mantener los nombres de las propiedades sin cambios
    }

    public static class RegisterInterface
    {
        public static void Register(this IServiceCollection builder)
        {

        
            builder.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            builder.AddScoped<IConstruirSession, ConstruirSession>();
            builder.AddScoped<IPeticionDocusignRepository, PeticionDocusignRepository>();
            builder.AddScoped<IPeticionDocusignAuth, PeticionDocusignAuth>();
            builder.AddScoped<IDocusignCallbackService, DocusignCallbackService>();
            builder.AddScoped<IEjemplo, Ejemplo>();
            builder.AddScoped<IBIM360Services, BIM360Services>();
            builder.AddScoped<IDocusignService, DocusignService>();


            builder.RegisterAssemblyTypes(Assembly.Load("Addons"));
            builder.AddScoped<IHelpDesk, HelpDesk>();
            builder.AddScoped<HandleError.IHandleError, HandleError.HandleExeption>();
            
        }

        public static void RegisterAssemblyTypes(this IServiceCollection services, Assembly assembly)
        {
            var typesToRegister = assembly.GetTypes()
                .Where(type => type.IsClass && !type.IsAbstract)
                .ToList();

            foreach (var type in typesToRegister)
            {
                var interfaces = type.GetInterfaces();
                if (interfaces.Length > 0)
                {
                    foreach (var @interface in interfaces)
                    {
                        services.AddScoped(@interface, type);
                    }
                }

            }
        }
    }
}
