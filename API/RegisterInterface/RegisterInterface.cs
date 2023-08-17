using Autodesk.Forge.Client;
using Docusign.Repository.Peticion;
using Docusign.Services;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Repository.DataBase.Model;
using Services.BIM360Services;

namespace API.RegisterInterface
{
    public static class RegisterInterface
    {
        public static void Register(this IServiceCollection builder) {

            builder.AddHttpContextAccessor();            
            builder.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            
            builder.AddScoped<IConstruirSession, ConstruirSession>();
            builder.AddScoped<IPeticionDocusignRepository, PeticionDocusignRepository>();
            builder.AddScoped<IPeticionDocusignAuth, PeticionDocusignAuth>();                        
            builder.AddScoped<IDocusignCallbackService, DocusignCallbackService>();
            builder.AddScoped<IEjemplo, Ejemplo>();
            builder.AddScoped<IBIM360Services, BIM360Services>();

            builder.AddScoped<IDocusignService, DocusignService>();
        }
    }
}
