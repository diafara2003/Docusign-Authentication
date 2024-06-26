
using System;
using System.Collections.Generic;

namespace Model.DTO.Addons
{
    public class AddonsDTO
    {
        public int Id { get; set; }
        public string Numero { get; set; }
        public string Nombre { get; set; }
        public bool Critico { get; set; }
        public bool Obsoleto { get; set; }
        public bool IsEstandar { get; set; }
        public string ModuloRequisito { get; set; }
        public string addonRequisito { get; set; }
        public bool publicado { get; set;}

    }


    public class EncabezadoAddonDTO
    {
        public int id { get; set; }
        public int addonNo { get; set; }
        public string addonNombre { get; set; }
        public DateTime fechaPublicacion { get; set; }
        public short horasInstalacion { get; set; }
        public decimal horasBdAdicional { get; set; }
        public string obs { get; set; }
        public string rutaPDF { get; set; }
        public bool obsoleto { get; set; }
        public string requisito { get; set; }
        public string requisitoModulo { get; set; }
        public bool critico { get; set; }
        public decimal costo { get; set; }
        public int modulo { get; set; }
        public bool estandar { get; set; }
        public bool instalable { get; set; }
        public bool visible { get; set; }
    }

    public class DetalleAddonDTO
    {
        public int id { get; set; }
        public int addonNo { get; set; }
        public string codigoNombre { get; set; }
        public string codigoValor { get; set; }
        public string valorNombre { get; set; }
        public string valorValor { get; set; }
        public string obs { get; set; }
        public bool anclaValor { get; set; }
        public string menuOld { get; set; }
        public string MenuOldDesc { get; set; }
        public string tabla { get; set; }
    }


    public class AddonConfigDTO
    {
        public EncabezadoAddonDTO encabezado { get; set; }
        public List<DetalleAddonDTO> detalle { get; set; }
    }


}
