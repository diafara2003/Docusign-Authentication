using Repository.DataBase.Conexion;
using Microsoft.AspNetCore.Http;
using Model.DTO;
using Model.DTO.Autodesk;
using Repository.DataBase.Conexion;
using Repository.DataBase.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

using System.Xml;
using System.Xml.Serialization;


namespace Services.BIM360Services
{
    public interface IBIM360Services
    {
        ResponseV3DTO MigracionRevit(List<MigracionRevitDTO> data);
    }

    public class BIM360Services : IBIM360Services
    {
       // private DB_ADPRO _contexto;
        private IConstruirSession _session;
        private IHttpContextAccessor _httpContextAccessor { get; }
        public BIM360Services(IHttpContextAccessor httpContextAccessor, IConstruirSession session)
        {
         //   this._contexto = contexto;
            this._session = session;
            this._httpContextAccessor= httpContextAccessor;
        }

        public ResponseV3DTO MigracionRevit(List<MigracionRevitDTO> data)
        {

            data= ValidarData(data);


            string xml = "";
            var encoding = Encoding.GetEncoding("ISO-8859-1");
            XmlSerializer DetalleImport = new XmlSerializer(data.GetType());
            XmlWriterSettings xmlWriterSettings = new XmlWriterSettings
            {
                Indent = true,
                OmitXmlDeclaration = false,
                Encoding = encoding
            };


            using (var stream = new MemoryStream())
            {
                using (var xmlWriter = XmlWriter.Create(stream, xmlWriterSettings))
                {
                    DetalleImport.Serialize(xmlWriter, data);
                }
                xml = encoding.GetString(stream.ToArray());
            }
            
            return RealizarMigracionRevit( xml);
        }


        public ResponseV3DTO RealizarMigracionRevit( string xml)
        {
            Dictionary<string, object> parametros = new Dictionary<string, object>();
            ResponseV3DTO objresultado_ = new ResponseV3DTO();
            try
            {

                var _datosSession = _session.ObtenerSession();

                parametros.Add("IdObra", _datosSession.IdProyecto);
                parametros.Add("IdUsuario", _datosSession.IdUsuario);
                parametros.Add("xmlInput", xml);

                var resultado = new DB_Execute().ExecuteStoreQuery(_httpContextAccessor,new Repository.DataBase.Model.ProcedureDTO()
                {
                    commandText = "[ADP_ARB].[ValidaMigracionRevit]",
                    parametros = parametros
                });

                var datos = (from dataLiq in resultado.AsEnumerable()
                             select new ResponseV3DTO()
                             {
                                 id = (string)dataLiq["Codigo"],
                                 mensaje = (string)dataLiq["Mensaje"]
                             }).FirstOrDefault();


                objresultado_ = GuardarDatos(datos);
            }
            catch (Exception e)
            {

                objresultado_.codigo = 1;
                objresultado_.mensaje = e.Message;
            }
            return objresultado_;
        }

        public ResponseV3DTO GuardarDatos(ResponseV3DTO info)
        {
            Dictionary<string, object> parametros = new Dictionary<string, object>();
            ResponseV3DTO objresultado_ = new ResponseV3DTO();


            parametros.Add("IdMigracion", info.id);
            parametros.Add("Accion", 1);

            var result = new DB_Execute().ExecuteStoreQuery(_httpContextAccessor,new Repository.DataBase.Model.ProcedureDTO()
            {
                commandText = "[ADP_ARB].[MigrarRevit]",
                parametros = parametros
            });


            objresultado_.codigo = (Convert.ToInt32(result.Rows[0]["COD"]));
            objresultado_.mensaje = ((string)result.Rows[0]["MSN"]);
            objresultado_.OtroValor = ((string)result.Rows[0]["idInserted"]);

            return objresultado_;
        }


        List<MigracionRevitDTO> ValidarData(List<MigracionRevitDTO> data)
        {

            foreach (var item in data)
            {

                if (item.Nombre == null) item.Nombre = "";
                if (item.Descripcion == null) item.Descripcion = "";
                if (item.Categoria == null) item.Categoria = "";
                if (item.Tipo == null) item.Tipo = "";
                if (item.Nivel == null) item.Nivel = "";
                if (item.TypeMark == null) item.TypeMark = "";
                if (item.Comentarios == null) item.Comentarios = "";
                if (item.Objeto == null) item.Objeto = "";


                if (item.Nombre.ToString().Contains("#"))
                    item.Nombre = item.Nombre.ToString().Replace("#", "");
                if (item.Nombre.ToString().Contains(@"\"))
                    item.Nombre = item.Nombre.ToString().Replace(@"\", "");
                if (item.Nombre.ToString().Contains(@"'"))
                    item.Nombre = item.Nombre.ToString().Replace(@"'", "");
                if (item.Nombre.ToString().Contains('"'))
                    item.Nombre = item.Nombre.ToString().Replace('"', ' ');

                item.Nombre = RemoveControlCharacters(item.Nombre.ToString());


                if (item.Descripcion != null && item.Descripcion.ToString().Contains("#"))
                    item.Descripcion = item.Descripcion.ToString().Replace("#", "");
                if (item.Descripcion.ToString().Contains(@"\"))
                    item.Descripcion = item.Descripcion.ToString().Replace(@"\", "");
                if (item.Descripcion.ToString().Contains('"'))
                    item.Descripcion = item.Descripcion.ToString().Replace('"', ' ');
                if (item.Descripcion.ToString().Contains(@"'"))
                    item.Descripcion = item.Descripcion.ToString().Replace(@"'", "");

                item.Descripcion = RemoveControlCharacters(item.Descripcion.ToString());


                if (item.Categoria.ToString().Contains("#"))
                    item.Categoria = item.Categoria.ToString().Replace("#", "");
                if (item.Categoria.ToString().Contains(@"\"))
                    item.Categoria = item.Categoria.ToString().Replace(@"\", "");
                if (item.Categoria.ToString().Contains('"'))
                    item.Categoria = item.Categoria.ToString().Replace('"', ' ');
                if (item.Categoria.ToString().Contains(@"'"))
                    item.Categoria = item.Categoria.ToString().Replace(@"'", "");

                item.Categoria = RemoveControlCharacters(item.Categoria.ToString());


                if (item.Tipo.ToString().Contains("#"))
                    item.Tipo = item.Tipo.ToString().Replace("#", "");
                if (item.Tipo.ToString().Contains(@"\"))
                    item.Tipo = item.Tipo.ToString().Replace(@"\", "");
                if (item.Tipo.ToString().Contains('"'))
                    item.Tipo = item.Tipo.ToString().Replace('"', ' ');
                if (item.Tipo.ToString().Contains(@"'"))
                    item.Tipo = item.Tipo.ToString().Replace(@"'", "");

                item.Tipo = RemoveControlCharacters(item.Tipo.ToString());

                if (item.Nivel.ToString().Contains("#"))
                    item.Nivel = item.Nivel.ToString().Replace("#", "");
                if (item.Nivel.ToString().Contains(@"\"))
                    item.Nivel = item.Nivel.ToString().Replace(@"\", "");
                if (item.Nivel.ToString().Contains('"'))
                    item.Nivel = item.Nivel.ToString().Replace('"', ' ');
                if (item.Nivel.ToString().Contains(@"'"))
                    item.Nivel = item.Nivel.ToString().Replace(@"'", "");

                item.Nivel = RemoveControlCharacters(item.Nivel.ToString());


                if (item.TypeMark.ToString().Contains("#"))
                    item.TypeMark = item.TypeMark.ToString().Replace("#", "");
                if (item.TypeMark.ToString().Contains(@"\"))
                    item.TypeMark = item.TypeMark.ToString().Replace(@"\", "");
                if (item.TypeMark.ToString().Contains('"'))
                    item.TypeMark = item.TypeMark.ToString().Replace('"', ' ');
                if (item.TypeMark.ToString().Contains(@"'"))
                    item.TypeMark = item.TypeMark.ToString().Replace(@"'", "");

                item.TypeMark = RemoveControlCharacters(item.TypeMark.ToString());



                if (item.Comentarios.ToString().Contains("#"))
                    item.Comentarios = item.Comentarios.ToString().Replace("#", "");
                if (item.Comentarios.ToString().Contains(@"\"))
                    item.Comentarios = item.Comentarios.ToString().Replace(@"\", "");
                if (item.Comentarios.ToString().Contains('"'))
                    item.Comentarios = item.Comentarios.ToString().Replace('"', ' ');
                if (item.Comentarios.ToString().Contains(@"'"))
                    item.Comentarios = item.Comentarios.ToString().Replace(@"'", "");

                item.Comentarios = RemoveControlCharacters(item.Comentarios.ToString());



                if (item.Objeto.ToString().Contains("#"))
                    item.Objeto = item.Objeto.ToString().Replace("#", "");
                if (item.Objeto.ToString().Contains(@"\"))
                    item.Objeto = item.Objeto.ToString().Replace(@"\", "");
                if (item.Objeto.ToString().Contains('"'))
                    item.Objeto = item.Objeto.ToString().Replace('"', ' ');
                if (item.Objeto.ToString().Contains(@"'"))
                    item.Objeto = item.Objeto.ToString().Replace(@"'", "");

                item.Objeto = RemoveControlCharacters(item.Objeto.ToString());
            }

            return data;
        }

        string RemoveControlCharacters(string inString)
        {
            if (inString == null) return null;
            StringBuilder newString = new StringBuilder();
            char ch;
            for (int i = 0; i < inString.Length; i++)
            {
                ch = inString[i];
                if (!char.IsControl(ch))
                {
                    newString.Append(ch);
                }
            }
            return newString.ToString();
        }
    }
}
