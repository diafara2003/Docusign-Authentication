namespace Docusign.Services
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
