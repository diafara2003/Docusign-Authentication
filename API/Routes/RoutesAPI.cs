using API.Routes.MapAutodesk;
using API.Routes.MapAutoDesk;
using API.Routes.MapDocusign;
using API.Routes.MapWeatherForecast;

namespace API.Routes
{
    public static class RoutesAPI
    {
        public static void RegisterRoutes(this IEndpointRouteBuilder app)
        {

            app.RegisterDocusign();
            app.RegisterDocusignDS();
            app.RegisterWeatherForecast();


            app.RegisterDataManagement();
            app.RegisterModelDerivative();
            app.RegisterOAuth();
            app.RegisterProyectos();
            app.RegisterTrimble();
        }
    }
}
