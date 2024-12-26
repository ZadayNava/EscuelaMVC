using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace EscuelaMVC.WCF
{
    // NOTA: puede usar el comando "Rename" del menú "Refactorizar" para cambiar el nombre de interfaz "IEscuelaService" en el código y en el archivo de configuración a la vez.
    [ServiceContract]
    public interface IEscuelaService
    {
        [OperationContract]
        string create_Escuela(
            string Nombre,
            string Clave,
            string Telefono,
            string Nivel,
            string Direccion,
            DateTime FechaFundacion);


        [OperationContract]
        List<Escuela_DTO> list_escuela(int id);


        [OperationContract]
        string update_escuela(
            int id,
            string Nombre,
            string Clave,
            string Telefono,
            string Nivel,
            string Direccion,
            DateTime FechaFundacion);

        [OperationContract]
        string delete_Escuela(int id);
    }
}
