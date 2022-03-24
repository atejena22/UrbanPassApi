using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WcfService
{
    public class BDComun
    {
        public static MySqlConnection ObtenerConexion()
        {
            MySqlConnection conn = new MySqlConnection("Server=localhost;Database=intouch;Uid=root;Pwd=root;");

            conn.Open();
            return conn;

        }

    }
}

