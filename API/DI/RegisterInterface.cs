using Docusign.Repository.Peticion;
using Docusign.Services;
using Docusign.Utilidades.Session;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace API.DI
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

            builder.AddScoped<IDocusignService, DocusignService>();
        }
    }
}
