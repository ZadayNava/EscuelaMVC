//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EscuelaMVC.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Tutor
    {
        public int ID_Tutor { get; set; }
        public string Nombre { get; set; }
        public string APaterno { get; set; }
        public string AMaterno { get; set; }
        public string CURP { get; set; }
        public string Sexo { get; set; }
        public string Telefono { get; set; }
        public string Parentesco { get; set; }
        public string Direccion { get; set; }
        public System.DateTime FechaNacimiento { get; set; }
        public int ID_Estudiante { get; set; }
    
        public virtual Estudiante Estudiante { get; set; }
    }
}
