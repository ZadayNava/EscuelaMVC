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
    
    public partial class Grado
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Grado()
        {
            this.Estudiante = new HashSet<Estudiante>();
        }
    
        public int ID_Grado { get; set; }
        public string Grado1 { get; set; }
        public string Grupo { get; set; }
        public int ID_Profesor { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Estudiante> Estudiante { get; set; }
        public virtual Profesor Profesor { get; set; }
    }
}
