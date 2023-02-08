using Autofac;
using Docusign.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Model.DTO;
using Repository.DataBase.Conexion;
using Repository.Repositories.Peticion;
using Services.DocusignServices;
using System;
using System.Reflection;
using Utilidades.Session;

namespace Docusign
{
    public class Startup
    {

        public IConfiguration Configuration { get; }
        public ContainerBuilder containerBuiler { get; set; }
        const string origins = "CorsPolicy";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            containerBuiler = new ContainerBuilder();
        }



        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //DSConfigurationDTO config = new DSConfigurationDTO();
            //Configuration.Bind("DocuSign", config);
            //services.AddSingleton(config);

            services.AddControllers();

            //services.AddScoped<IHttpContextAccessor, HttpContextAccessor>();
            //services.AddScoped<IConstruirSession, ConstruirSession>();
            //services.AddScoped<IPeticionDocusignRepository, PeticionDocusignRepository>();
            services.AddScoped<IDocusignService, DocusignService>();
           
            
            services.AddScoped<IEjemplo, Ejemplo>();

            services.AddCors(options =>
            {
                options.AddPolicy(origins,
                    builder => builder
                    .WithOrigins("*")
                    .AllowAnyMethod()
                    .AllowAnyHeader());
            });

            services.AddHttpContextAccessor();
            //ConfigureContainer(containerBuiler);

            services.AddAuthorization();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //if (env.IsDevelopment()) app.UseDeveloperExceptionPage();
            //else app.UseHsts();

            app.UseRouting();

            app.UseCors(origins);
            app.UseAuthentication();
            app.UseAuthorization();

            //app.UseMiddleware(typeof(AuthenticationMiddleware));
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(AppDomain.CurrentDomain.GetAssemblies())
            .Where(x => x?.Namespace != null
            && (x.Namespace.EndsWith("Peticion") || x.Namespace.EndsWith("Conexion") || x.Namespace.EndsWith("DocusignServices")))
             .AsImplementedInterfaces()
       .InstancePerRequest();

            // builder.RegisterAssemblyTypes(Assembly.Load("Services"))
            //.Where(x => x?.Namespace != null && x.Namespace.EndsWith("DocusignServices"))
            //.AsImplementedInterfaces();

        }
    }
}
