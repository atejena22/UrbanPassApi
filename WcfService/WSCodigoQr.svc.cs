using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace WcfService
{
    // NOTA: puede usar el comando "Rename" del menú "Refactorizar" para cambiar el nombre de clase "Service1" en el código, en svc y en el archivo de configuración.
    // NOTE: para iniciar el Cliente de prueba WCF para probar este servicio, seleccione Service1.svc o Service1.svc.cs en el Explorador de soluciones e inicie la depuración.
    public class WSCodigoQr : IWcodigo
    {
        public Codigo buscarCodigo(string codigo)
        {
            Codigo codigo1 = new Codigo();

            using (MySqlConnection conn = BDComun.ObtenerConexion())
            {
                string query = "select *from intouch.guestacces where accessCode='"+codigo+"'    ";
                MySqlCommand cmd =new MySqlCommand(query,conn);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    codigo1.codigo = reader.GetString(0);
                }

                if (codigo1.codigo== "QR-56-65-1-1")
                {
                    codigo1.mensaje = "encontrado";
                }

            }
            return codigo1;

        }




    }
}

