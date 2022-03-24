using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;

namespace inTouchApi.Models
{
    public class Urbanizations
    {
        string sConn = ConfigurationManager.AppSettings("connectionStrings:SQLConn");

        public Urbanization getUrbanization(int urbanizationID)
        {

            Urbanization objResponse = new Urbanization();

            try
            {

                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append(" SELECT  * from intouch.urbanizations where urbanizationID =" + urbanizationID);

                // Open connection
                using (MySqlConnection c = new MySqlConnection(sConn))
                {
                    c.Open();
                    // Create new DataAdapter
                    using (MySqlDataAdapter a = new MySqlDataAdapter(sb.ToString(), c))
                    {
                        // Use DataAdapter to fill DataTable
                        DataTable t = new DataTable();
                        a.Fill(t);
                        if (t.Rows.Count > 0)
                        {
                            foreach (DataRow r in t.Rows)
                            {
                                objResponse.urbanizationID = SharedFunctions.ValidateInteger(r["urbanizationID"]);
                                objResponse.urbanization = SharedFunctions.ValidateString(r["urbanization"]);
                                objResponse.contactNumber = SharedFunctions.ValidateString(r["contactNumber"]);
                                objResponse.contactEmail = SharedFunctions.ValidateString(r["contactEmail"]);
                                objResponse.contactName = SharedFunctions.ValidateString(r["contactName"]);
                                objResponse.active = SharedFunctions.ValidateBoolean(r["active"]);
                                objResponse.activeFrom = SharedFunctions.ValidateDatetime(r["activeFrom"]);
                                objResponse.activeTo = SharedFunctions.ValidateDatetime(r["activeTo"]);
                                objResponse.ruc = SharedFunctions.ValidateString(r["ruc"]);
                                objResponse.city = SharedFunctions.ValidateString(r["city"]);
                                objResponse.country = SharedFunctions.ValidateString(r["country"]);
                                objResponse.image = SharedFunctions.ValidateString(r["image"]);
                                

                            }
                        }
                    }
                    c.Close();
                }
            }
            catch (Exception ex)
            {
                objResponse.hasError = true;
                objResponse.errorDesc = ex.Message.ToString();
                objResponse.errorInter = ex.Message.ToString();
                objResponse.errorNum = 201;
            }

            return objResponse;

        }

        public UrbanizationsList getUrbanizations(int urbanizationID)
        {

            UrbanizationsList objResponse = new UrbanizationsList();

            List<Urbanization> objList = new List<Urbanization>();

            try
            {

                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append(" SELECT  * from users intouch.urbanizations urbanizationID =" + urbanizationID);

                // Open connection
                using (MySqlConnection c = new MySqlConnection(sConn))
                {
                    c.Open();
                    // Create new DataAdapter
                    using (MySqlDataAdapter a = new MySqlDataAdapter(sb.ToString(), c))
                    {
                        // Use DataAdapter to fill DataTable
                        DataTable t = new DataTable();
                        a.Fill(t);
                        if (t.Rows.Count > 0)
                        {
                            foreach (DataRow r in t.Rows)
                            {
                                Urbanization objItem = new Urbanization();
                                objItem.urbanizationID = SharedFunctions.ValidateInteger(r["urbanizationID"]);
                                objItem.urbanization = SharedFunctions.ValidateString(r["urbanization"]);
                                objItem.contactNumber = SharedFunctions.ValidateString(r["contactNumber"]);
                                objItem.contactEmail = SharedFunctions.ValidateString(r["contactEmail"]);
                                objItem.contactName = SharedFunctions.ValidateString(r["contactName"]);
                                objItem.active = SharedFunctions.ValidateBoolean(r["active"]);
                                objItem.activeFrom = SharedFunctions.ValidateDatetime(r["activeFrom"]);
                                objItem.activeTo = SharedFunctions.ValidateDatetime(r["activeTo"]);
                                objItem.ruc = SharedFunctions.ValidateString(r["ruc"]);
                                objItem.city = SharedFunctions.ValidateString(r["city"]);
                                objItem.country = SharedFunctions.ValidateString(r["country"]);
                                objItem.image = SharedFunctions.ValidateString(r["image"]);
                                objList.Add(objItem);
                            }

                            objResponse.objList = objList;
                        }

                    }
                    c.Close();
                }
            }
            catch (Exception ex)
            {
                objResponse.hasError = true;
                objResponse.errorDesc = ex.Message.ToString();
                objResponse.errorInter = ex.Message.ToString();
                objResponse.errorNum = 201;
            }

            return objResponse;

        }

    }

    public class UrbanizationsList : ErrorInfo
    { 
        public List<Urbanization> objList { get; set; }
        public UrbanizationsList()
        {
            objList = new List<Urbanization>();
        }
    }

    public class Urbanization : ErrorInfo
    {

        public int urbanizationID { get; set; }
        public string urbanization { get; set; }
        public string contactNumber { get; set; }
        public string contactEmail { get; set; }
        public string contactName { get; set; }
        public Boolean active { get; set; }
        public DateTime activeFrom { get; set; }
        public DateTime activeTo { get; set; }
        public string ruc { get; set; }
        public string city { get; set; }
        public string country { get; set; }
        public string image { get; set; }

        

        public Urbanization()
        {
            urbanizationID = 0;
            urbanization = "";
            contactNumber = ""; 
            contactEmail = "";
            contactName = "";
            active = false; 
            ruc = "";
            city = ""; 
            country = "";
            image = "";
        }

    }


}
