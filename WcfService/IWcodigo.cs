using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace WcfService
{
    // NOTA: puede usar el comando "Rename" del menú "Refactorizar" para cambiar el nombre de interfaz "IService1" en el código y en el archivo de configuración a la vez.
    [ServiceContract]
    public interface IWcodigo
    {

        [OperationContract]
        Codigo buscarCodigo(string codigo);

        // TODO: agregue aquí sus operaciones de servicio
    }


    // Utilice un contrato de datos, como se ilustra en el ejemplo siguiente, para agregar tipos compuestos a las operaciones de servicio.
    [DataContract]
    public class Codigo:BaseRespuesta
    {
       [DataMember]
        public string codigo { get; set; }
    }


    [DataContract]
    public class BaseRespuesta
    {
        [DataMember]
        public string mensaje { get; set; }
    }




}
