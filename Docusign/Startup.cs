using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Model.DTO;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;

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
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

           // services.AddAuthentication(options =>
           // {
           //     options.DefaultSignOutScheme = CookieAuthenticationDefaults.AuthenticationScheme;
           //     options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
           //     options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
           //     options.DefaultChallengeScheme = "DocuSign";
           // })
           //.AddCookie()
           //.AddOAuth("DocuSign", options =>
           //{
           //    options.ClientId = Configuration["DocuSign:ClientId"];
           //    options.ClientSecret = Configuration["DocuSign:ClientSecret"];
           //    options.CallbackPath = new PathString("/ds/callback");

           //    options.AuthorizationEndpoint = Configuration["DocuSign:AuthorizationEndpoint"];
           //    options.TokenEndpoint = Configuration["DocuSign:TokenEndpoint"];
           //    options.UserInformationEndpoint = Configuration["DocuSign:UserInformationEndpoint"];
           //    options.Scope.Add("signature");

           //    options.SaveTokens = true;
           //    options.ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "sub");
           //    options.ClaimActions.MapJsonKey(ClaimTypes.Name, "name");
           //    options.ClaimActions.MapJsonKey("accounts", "accounts");

           //    options.ClaimActions.MapCustomJson("account_id", obj => ExtractDefaultAccountValue(obj, "account_id"));
           //    options.ClaimActions.MapCustomJson("account_name", obj => ExtractDefaultAccountValue(obj, "account_name"));
           //    options.ClaimActions.MapCustomJson("base_uri", obj => ExtractDefaultAccountValue(obj, "base_uri"));
           //    options.ClaimActions.MapJsonKey("access_token", "access_token");
           //    options.ClaimActions.MapJsonKey("refresh_token", "refresh_token");
           //    options.ClaimActions.MapJsonKey("expires_in", "expires_in");

             


           //    options.Events = new OAuthEvents
           //    {
           //        OnCreatingTicket = async context =>
           //        {
           //            var request = new HttpRequestMessage(HttpMethod.Get, context.Options.UserInformationEndpoint);
           //            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
           //            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", context.AccessToken);

           //            HttpResponseMessage response = await context.Backchannel.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, context.HttpContext.RequestAborted);
           //            response.EnsureSuccessStatusCode();
           //            var userJObject = JObject.Parse(await response.Content.ReadAsStringAsync());

           //            userJObject.Add("access_token", context.AccessToken);
           //            userJObject.Add("refresh_token", context.RefreshToken);
           //            userJObject.Add("expires_in", DateTime.Now.Add(context.ExpiresIn.Value).ToString());

           //            using (JsonDocument payload = JsonDocument.Parse(userJObject.ToString()))
           //            {
           //                context.RunClaimActions(payload.RootElement);
           //            }
           //        }
           //    };
           //});



            services.AddControllers();


            services.AddAuthorization();

        }

#nullable enable
        private string? ExtractDefaultAccountValue(JsonElement obj, string key)
        {
            if (!obj.TryGetProperty("accounts", out var accounts))
            {
                return null;
            }

            string? keyValue = null;

            foreach (var account in accounts.EnumerateArray())
            {
                if (account.TryGetProperty("is_default", out var defaultAccount) && defaultAccount.GetBoolean())
                {
                    if (account.TryGetProperty(key, out var value))
                    {
                        keyValue = value.GetString();
                    }
                }
            }

            return keyValue;
        }
#nullable disable

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
        }
    }
}
