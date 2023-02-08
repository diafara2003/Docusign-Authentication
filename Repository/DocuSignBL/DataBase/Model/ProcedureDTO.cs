

using System.Collections.Generic;

namespace Repository.DataBase.Model
{
    public class ProcedureDTO
    {
        public ProcedureDTO()
        {
            this.parametros = new Dictionary<string, object>();
        }
        public string commandText { get; set; }
        public Dictionary<string, object> parametros { get; set; }

    }
}
