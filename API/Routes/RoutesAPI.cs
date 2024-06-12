using API.Routes.MapAddons;
using API.Routes.MapAutodesk;
using API.Routes.MapAutoDesk;
using API.Routes.MapDocusign;
using API.Routes.MapInventarios;
using API.Routes.MapWeatherForecast;

namespace API.Routes
{
    public static class RoutesAPI
    {
        public static void RegisterRoutes(this IEndpointRouteBuilder app)
        {

            app.RegisterDocusign();
            app.RegisterDocusignDS();
            app.RegisterInventarios();
            app.RegisterWeatherForecast();


            app.RegisterDataManagement();
            app.RegisterModelDerivative();
            app.RegisterOAuth();
            app.RegisterProyectos();
            //app.RegisterTrimble();
            app.RegisterAddons();
        }
    }
}
