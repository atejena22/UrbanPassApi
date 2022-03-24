using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using WcfQr.Conexion;

namespace WcfQr
{
    // NOTA: puede usar el comando "Rename" del menú "Refactorizar" para cambiar el nombre de clase "Service1" en el código, en svc y en el archivo de configuración.
    // NOTE: para iniciar el Cliente de prueba WCF para probar este servicio, seleccione Service1.svc o Service1.svc.cs en el Explorador de soluciones e inicie la depuración.
    public class Service1 : IServiceQr
    {

        public CodigoQr buscarCodigo(string codigo)
        {
            CodigoQr codigo1 = new CodigoQr();

            using (MySqlConnection conn = BDcomun.ObtenerConexion())
            {
                string query = "select *from intouch.guestacces where accessCode='" + codigo + "'    ";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    codigo1.codigo = reader.GetString(6);
                }

                if (codigo1.codigo == codigo)
                {
                    codigo1.mensaje = "encontrado";
                    codigo1.codigo = codigo;
                    return codigo1;
                }
                else
                {
                    codigo1.mensaje = "no encontrado";
                    return codigo1;
                }

            }
           

        }



    }
}
