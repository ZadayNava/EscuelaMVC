using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace EscuelaMVC.WCF
{
    // NOTA: puede usar el comando "Rename" del menú "Refactorizar" para cambiar el nombre de interfaz "IAsignaturaService" en el código y en el archivo de configuración a la vez.
    [ServiceContract]
    public interface IAsignaturaService
    {
        [OperationContract]
        string create_Asignatura(
            string Nombre);


        [OperationContract]
        List<Asignatura_DTO> list_asignatura(int id);


        [OperationContract]
        string update_Asignatura(
            int id,
            string Nombre);

        [OperationContract]
        string delete_Asignatura(int id);
    }
}
