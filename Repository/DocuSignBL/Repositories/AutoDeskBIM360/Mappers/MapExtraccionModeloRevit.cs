using Autodesk.Forge.Model;
using Model.DTO.Autodesk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Repository.AutoDesk.Mappers
{
    public static class MapExtraccionModeloRevit
    {
        public static IList<ExtraccionModeloRevitDTO> Map(dynamic values, string nameFile, string guid, string urn, dynamic properties)
        {
            IList<ExtraccionModeloRevitDTO> objresult = new List<ExtraccionModeloRevitDTO>();

            foreach (var item in ((Autodesk.Forge.Model.DynamicDictionary)values).Dictionary)
            {
                if (item.Value == null) continue;

                ExtraccionModeloRevitDTO _obj = new ExtraccionModeloRevitDTO();

                foreach (var _data in ((Autodesk.Forge.Model.DynamicDictionary)item.Value).Dictionary)
                {
                    var _value = _data.Value;
                    if (_value == null) continue;

                    Type b2 = _value.GetType();

                    if (b2 == typeof(DynamicDictionary))
                    {
                        foreach (var _properties in ((Autodesk.Forge.Model.DynamicDictionary)_value).Dictionary)
                        {
                            var _valueProperty = _properties.Value;

                            if (_valueProperty == null) continue;

                            switch (_properties.Key)
                            {
                                case "Identity Data":

                                    if (((Autodesk.Forge.Model.DynamicDictionary)_properties.Value).Dictionary.Where(c => c.Key == "Keynote").Count() == 0)
                                        continue;

                                    var _AssemblyCode = ((Autodesk.Forge.Model.DynamicDictionary)_properties.Value).Dictionary.Where(c => c.Key == "Assembly Code");
                                    if (_AssemblyCode.Count() > 0)
                                        _obj.AssemblyCode = (string)_AssemblyCode.FirstOrDefault().Value;

                                    var _TypeName = ((Autodesk.Forge.Model.DynamicDictionary)_properties.Value).Dictionary.Where(c => c.Key == "Type Name");
                                    if (_TypeName.Count() > 0)
                                        _obj.Nombre = (string)_TypeName.FirstOrDefault().Value;


                                    var _Keynote = ((Autodesk.Forge.Model.DynamicDictionary)_properties.Value).Dictionary.Where(c => c.Key == "Keynote");
                                    if (_Keynote.Count() > 0)
                                        _obj.KeyCode = (string)_Keynote.FirstOrDefault().Value;


                                    var _TypeMark = ((Autodesk.Forge.Model.DynamicDictionary)_properties.Value).Dictionary.Where(c => c.Key == "Type Mark");
                                    if (_TypeMark.Count() > 0)
                                        _obj.TypeMark = (string)_TypeMark.FirstOrDefault().Value;
                                    else
                                    {
                                        _TypeMark = ((Autodesk.Forge.Model.DynamicDictionary)_properties.Value).Dictionary.Where(c => c.Key == "TypeMark");
                                        if (_TypeMark.Count() > 0)
                                            _obj.TypeMark = (string)_TypeMark.FirstOrDefault().Value;
                                    }

                                    var _Description = ((Autodesk.Forge.Model.DynamicDictionary)_properties.Value).Dictionary.Where(c => c.Key == "Description");
                                    if (_Description.Count() > 0)
                                        _obj.Descripcion = (string)_Description.FirstOrDefault().Value;


                                    var _SubCapitulo = ((Autodesk.Forge.Model.DynamicDictionary)_properties.Value).Dictionary.Where(c => c.Key == "SubCapitulo");
                                    if (_SubCapitulo.Count() > 0)
                                        _obj.SubCapitulo = (string)_TypeMark.FirstOrDefault().Value;
                                    else
                                    {
                                        _SubCapitulo = ((Autodesk.Forge.Model.DynamicDictionary)_properties.Value).Dictionary.Where(c => c.Key == "SubCapítulo");
                                        if (_SubCapitulo.Count() > 0)
                                            _obj.SubCapitulo = (string)_SubCapitulo.FirstOrDefault().Value;
                                    }

                                    var _Avance = ((Autodesk.Forge.Model.DynamicDictionary)_properties.Value).Dictionary.Where(c => c.Key == "Avance");
                                    if (_Avance.Count() > 0)
                                        _obj.Avance = (string)_Avance.FirstOrDefault().Value;

                                    var _Ubicacion = ((Autodesk.Forge.Model.DynamicDictionary)_properties.Value).Dictionary.Where(c => c.Key == "Ubicacion");
                                    if (_Ubicacion.Count() > 0)
                                        _obj.Ubicacion = (string)_Ubicacion.FirstOrDefault().Value;

                                    var _Comentarios = ((Autodesk.Forge.Model.DynamicDictionary)_properties.Value).Dictionary.Where(c => c.Key == "Comments");
                                    if (_Comentarios.Count() > 0)
                                        _obj.Comentarios = (string)_Comentarios.FirstOrDefault().Value;
                                    else
                                    {
                                        _Comentarios = ((Autodesk.Forge.Model.DynamicDictionary)_properties.Value).Dictionary.Where(c => c.Key == "Comentarios");
                                        if (_Comentarios.Count() > 0)
                                            _obj.Comentarios = (string)_Comentarios.FirstOrDefault().Value;
                                    }

                                    var _Ejecucion = ((Autodesk.Forge.Model.DynamicDictionary)_properties.Value).Dictionary.Where(c => c.Key == "Ejecucion");
                                    if (_Ejecucion.Count() > 0)
                                        _obj.Ejecucion = (string)_Ejecucion.FirstOrDefault().Value;

                                    var _Estado = ((Autodesk.Forge.Model.DynamicDictionary)_properties.Value).Dictionary.Where(c => c.Key == "Estado");
                                    if (_Estado.Count() > 0)
                                        _obj.Estado = (string)_Estado.FirstOrDefault().Value;

                                    break;

                                case "Dimensions":


                                    var _Area = ((Autodesk.Forge.Model.DynamicDictionary)_properties.Value).Dictionary.Where(c => c.Key == "Area");
                                    if (_Area.Count() > 0)
                                        _obj.Area = ((string)_Area.FirstOrDefault().Value).Replace("m", "").Replace("^2", "");

                                    var _volumen = ((Autodesk.Forge.Model.DynamicDictionary)_properties.Value).Dictionary.Where(c => c.Key == "Volume");
                                    if (_volumen.Count() > 0)
                                        _obj.volumen = ((string)_volumen.FirstOrDefault().Value).Replace("m^3", "").Replace("^2", ""); ;

                                    var _Longitud = ((Autodesk.Forge.Model.DynamicDictionary)_properties.Value).Dictionary.Where(c => c.Key == "Length");
                                    if (_Longitud.Count() > 0)
                                        _obj.Longitud = ((string)_Longitud.FirstOrDefault().Value).Replace("m", "").Replace("^2", ""); ;

                                    var _Altura = ((Autodesk.Forge.Model.DynamicDictionary)_properties.Value).Dictionary.Where(c => c.Key == "Height");
                                    if (_Altura.Count() > 0)
                                        _obj.Altura = ((string)_Altura.FirstOrDefault().Value).Replace("m", "").Replace("^2", ""); ;

                                    var _Ancho = ((Autodesk.Forge.Model.DynamicDictionary)_properties.Value).Dictionary.Where(c => c.Key == "Width");
                                    if (_Ancho.Count() > 0)
                                        _obj.Ancho = ((string)_Ancho.FirstOrDefault().Value).Replace("m", "").Replace("^2", ""); ;



                                    break;
                                case "Constraints":

                                    var _Altura1 = ((Autodesk.Forge.Model.DynamicDictionary)_properties.Value).Dictionary.Where(c => c.Key == "Unconnected Height");
                                    if (_Altura1.Count() > 0)
                                        _obj.Altura = ((string)_Altura1.FirstOrDefault().Value).Replace("m", "");


                                    var _Nivel = ((Autodesk.Forge.Model.DynamicDictionary)_properties.Value).Dictionary.Where(c => c.Key == "Base Constraint");
                                    if (_Nivel.Count() > 0)
                                        _obj.Nivel = ((string)_Nivel.FirstOrDefault().Value).Replace("m", "");

                                    break;
                                case "Text":

                                    var _SubCapituloTexto = ((Autodesk.Forge.Model.DynamicDictionary)_properties.Value).Dictionary.Where(c => c.Key == "SubCapitulo");
                                    if (_SubCapituloTexto.Count() > 0)
                                        _obj.SubCapitulo = (string)_SubCapituloTexto.FirstOrDefault().Value;
                                    else
                                    {
                                        _SubCapituloTexto = ((Autodesk.Forge.Model.DynamicDictionary)_properties.Value).Dictionary.Where(c => c.Key == "SubCapítulo");
                                        if (_SubCapituloTexto.Count() > 0)
                                            _obj.SubCapitulo = (string)_SubCapituloTexto.FirstOrDefault().Value;
                                    }

                                    break;
                            }
                            

                        }
                    }
                    else
                    {
                        switch (_data.Key)
                        {
                            case "name":

                                var name_id = ((string)_value).Split("[");

                                if (name_id.Length == 1) continue;

                                _obj.IdElemento = name_id[1].Replace("]", "");
                                _obj.Categoria = name_id[0];
                                _obj.Tipo = name_id[0];

                                break;
                        }
                    }


                    /*calculo para area*/
                    if (string.IsNullOrEmpty(_obj.Area) && string.IsNullOrEmpty(_obj.Longitud) && string.IsNullOrEmpty(_obj.Altura))
                    {
                        decimal _altura = 0, _ancho = 0;

                        decimal.TryParse(_obj.Altura.Replace(".", ","), out _altura);
                        decimal.TryParse(_obj.Ancho.Replace(".", ","), out _ancho);

                        _obj.Area = (_altura * _ancho).ToString().Replace(",", ".");
                    }

                    if (string.IsNullOrEmpty(_obj.Longitud) && !string.IsNullOrEmpty(_obj.Ancho))
                    {
                        _obj.Longitud = _obj.Ancho;
                    }

                }
                _obj.archivo = nameFile;
                _obj.urn = urn;
                _obj.guid = guid;
                objresult.Add(_obj);

            }
            return objresult;
        }


        private static Dictionary<string, object> MapProperties(long id, dynamic properties)
        {
            Dictionary<string, object> returnProps = new Dictionary<string, object>();
            foreach (KeyValuePair<string, dynamic> objectProps in new DynamicDictionaryItems(properties.data.collection))
            {
                if (objectProps.Value.objectid != id) continue;
                string name = objectProps.Value.name;
                long elementId = long.Parse(Regex.Match(name, @"\d+").Value);
                returnProps.Add("ID", elementId);
                returnProps.Add("Name", name.Replace("[" + elementId.ToString() + "]", string.Empty));
                foreach (KeyValuePair<string, dynamic> objectPropsGroup in new DynamicDictionaryItems(objectProps.Value.properties))
                {
                    if (objectPropsGroup.Key.StartsWith("__")) continue;
                    foreach (KeyValuePair<string, dynamic> objectProp in new DynamicDictionaryItems(objectPropsGroup.Value))
                    {
                        if (!returnProps.ContainsKey(objectProp.Key))
                            returnProps.Add(objectProp.Key, objectProp.Value);

                    }
                }
            }
            return returnProps;
        }
    }
}
