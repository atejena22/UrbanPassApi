using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;

namespace inTouchApi.Models
{
    public class Houses
    {
        string sConn = ConfigurationManager.AppSettings("connectionStrings:SQLConn");

        public HouseList getHousesByUser(int userID, int houseID = 0) 
        {

            HouseList objResponse = new HouseList();

            List<House> objList = new List<House>();

            try
            {

                System.Text.StringBuilder sb = new System.Text.StringBuilder();
               
                if (houseID > 0)
                { 
                    sb.Append(" SELECT  * from intouch.houses where userID =" + userID + " and houseID = " + houseID) ;
                }
                else
                {
                    sb.Append(" SELECT  * from intouch.houses where userID =" + userID);
                }

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
                                House objItem = new House();
                                objItem.urbanizationID = SharedFunctions.ValidateInteger(r["urbanizationID"]);
                                objItem.houseID = SharedFunctions.ValidateInteger(r["houseID"]);
                                objItem.userID = SharedFunctions.ValidateInteger(r["userID"]);
                                objItem.mz = SharedFunctions.ValidateString(r["mz"]);
                                objItem.villa = SharedFunctions.ValidateString(r["villa"]); 
                                objItem.phoneNumber = SharedFunctions.ValidateString(r["phoneNumber"]); 
                                objItem.notes = SharedFunctions.ValidateString(r["notes"]);
                                objItem.fullAddress = SharedFunctions.ValidateString(r["fullAddress"]);
                                if (objItem.urbanizationID > 0) 
                                {
                                    Urbanizations objHouses = new Urbanizations();
                                    objItem.urbanization = objHouses.getUrbanization(objItem.urbanizationID);
                                } 
                                objList.Add(objItem);
                            }
                            objResponse.houseList = objList;
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

        public HouseList getHousesByUurbanization(int urbanizationID)
        {

            HouseList objResponse = new HouseList();

            List<House> objList = new List<House>();

            try
            {

                System.Text.StringBuilder sb = new System.Text.StringBuilder();


                sb.Append(" SELECT  * from intouch.houses where urbanizationID =" + urbanizationID);


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
                                House objItem = new House();
                                objItem.urbanizationID = SharedFunctions.ValidateInteger(r["urbanizationID"]);
                                objItem.houseID = SharedFunctions.ValidateInteger(r["houseID"]);
                                objItem.userID = SharedFunctions.ValidateInteger(r["userID"]);
                                objItem.mz = SharedFunctions.ValidateString(r["mz"]);
                                objItem.villa = SharedFunctions.ValidateString(r["villa"]);
                                objItem.phoneNumber = SharedFunctions.ValidateString(r["phoneNumber"]);
                                objItem.notes = SharedFunctions.ValidateString(r["notes"]);
                                objItem.fullAddress = SharedFunctions.ValidateString(r["fullAddress"]);
                                objList.Add(objItem);
                            }
                            objResponse.houseList = objList;
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

    public class House
    {

        public int houseID { get; set; }
        public int urbanizationID { get; set; }
        public int userID { get; set; }
        public string mz { get; set; }
        public string villa { get; set; }
        public string phoneNumber { get; set; }
        public string notes { get; set; }
        public string fullAddress { get; set; }

        public Urbanization urbanization { get; set; }

        public House()
        {
            houseID = 0;
            urbanizationID = 0;
            userID = 0;
            mz = "";
            villa = "";
            phoneNumber = "";
            notes = "";
            fullAddress = "";
            urbanization = new Urbanization();
        }
    }

    public class HouseList : ErrorInfo
    {
       public List<House> houseList { get; set; }

        public HouseList()
        {
            houseList = new List<House>();
        }

    }

}
