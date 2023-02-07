using DocuSignBL.DataBase.Conexion;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Model.DTO;

namespace Docusign
{
    public class Startup
    {



        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        const string origins = "CorsPolicy";

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {


            DSConfigurationDTO config = new DSConfigurationDTO();
            
            Configuration.Bind("DocuSign", config);

            services.AddSingleton(config);
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddCors(options =>
            {
                options.AddPolicy(origins,
                    //builder => builder.WithOrigins("http://10.1.10.31")
                    builder => builder.WithOrigins("*")
                    .AllowAnyMethod()
                    .AllowAnyHeader());
            });

        

            //  services.AddMemoryCache();
            //services.AddSession();
            services.AddHttpContextAccessor();
       

            services.AddControllers();



            services.AddAuthorization();

        }



        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

            else app.UseHsts();


            //  app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(origins);
            app.UseAuthentication();
            app.UseAuthorization();

            //   app.UseSession();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });


            //new Inicializer().UpgradeDatabase(app);
        }



    }
}
