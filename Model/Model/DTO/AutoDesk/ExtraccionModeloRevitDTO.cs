using System;

namespace Model.DTO.Autodesk
{
    public class ExtraccionModeloRevitDTO
    {

        public ExtraccionModeloRevitDTO()
        {            
            this.IdElemento = String.Empty;
            this.Nombre = String.Empty;
            this.Categoria = String.Empty;
            this.Tipo = String.Empty;
            this.AssemblyCode = String.Empty;
            this.KeyCode = String.Empty;
            this.TypeMark = String.Empty;
            this.Descripcion = String.Empty;
            this.Comentarios = String.Empty;
            this.Nivel = String.Empty;
            this.Area = "0";
            this.Altura = "0";
            this.Longitud = "0";
            this.volumen = "0";
            this.Densidad = "0";
            this.Ancho = "0";
            this.SubCapitulo = String.Empty;
            this.Avance = String.Empty;
            this.Ubicacion = String.Empty;
            this.Ejecucion = String.Empty;
            this.Estado = String.Empty;
            this.archivo = String.Empty;
            this.urn = String.Empty;
            this.guid = String.Empty;
        }

        public string IdElemento { get; set; }
        public string Nombre { get; set; }
        public string Categoria { get; set; }
        public string Tipo { get; set; }
        public string AssemblyCode { get; set; }
        public string KeyCode { get; set; }
        public string TypeMark { get; set; }
        public string Descripcion { get; set; }
        public string Comentarios { get; set; }
        public string Nivel { get; set; }
        public string Area { get; set; }
        public string Altura { get; set; }
        public string Longitud { get; set; }
        public string volumen { get; set; }
        public string Densidad { get; set; }
        public string Ancho { get; set; }
        public string SubCapitulo { get; set; }
        public string Avance { get; set; }
        public string Ubicacion { get; set; }
        public string Ejecucion { get; set; }
        public string Estado { get; set; }
        public string archivo { get; set; }
        public string urn { get; set; }
        public string guid { get; set; }
    }
}
