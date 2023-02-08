namespace Docusign.Middleware
{

    public interface IEjemplo
    {
        string name();
    }
    public class Ejemplo : IEjemplo
    {
        public string name()
        {
            return "JAU";
        }
    }
}
