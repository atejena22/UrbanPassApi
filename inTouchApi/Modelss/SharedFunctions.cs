using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace inTouchApi.Models
{
    public class SharedFunctions
    {

        public static Boolean validateEmail(string str)
        {
            Regex ObjAlphaPattern = new Regex(@"^([0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,3})$");
            return ObjAlphaPattern.IsMatch(str);
        }

        public static Decimal ValidateDecimal(Object value)
        {
            Decimal dReturn = 0;
            try
            {

                if ((value != null) && (value != DBNull.Value))
                {
                    if (value.ToString().Contains("CR"))
                    {
                        value = "0";
                    }

                    dReturn = Convert.ToDecimal(value.ToString());
                }
            }
            catch
            { }
            return dReturn;
        }

        public static Decimal ValidateDouble(Object value)
        {
            Decimal dReturn = 0;
            try
            {
                if ((value != null) && (value != DBNull.Value))
                {
                    dReturn = Convert.ToDecimal(value.ToString());
                }
            }
            catch
            { }
            return dReturn;
        }

        public static String ValidateString(Object value, string defaultResponse = "")
        {
            String SReturn = defaultResponse;
            try
            {
                if ((value != null) && (value != DBNull.Value))
                {
                    SReturn = value.ToString().Trim();
                }
            }
            catch
            { }
            return SReturn;
        }

        public static String ValidateStringToNull(Object value, string defaultResponse = "")
        {
            String SReturn = null;
            try
            {
                if ((value != null) && (value != DBNull.Value) && (value.ToString() != ""))
                {
                    SReturn = value.ToString().Trim();
                }
            }
            catch
            { }
            return SReturn;
        }


        public static int ValidateInteger(Object value)
        {
            int iReturn = 0;
            try
            {
                if ((value != null) && (value != DBNull.Value))
                {
                    iReturn = Convert.ToInt32(value.ToString());
                }
            }
            catch
            { }
            return iReturn;
        }

        public static Boolean ValidateBoolean(Object value)
        {
            return ValidateBoolean(value, false);
        }

        public static Boolean ValidateBoolean(Object value, Boolean bitValue)
        {
            Boolean iReturn = bitValue;
            try
            {
                if ((value != null) && (value != DBNull.Value))
                {
                    iReturn = Convert.ToBoolean(value.ToString());
                }
            }
            catch
            { }
            return iReturn;
        }

        public static DateTime ValidateDatetime(string value)
        {
            return ValidateDatetime((object)value);
        }
        public static DateTime ValidateDatetime(Object value)
        {
            DateTime iReturn = new DateTime();
            try
            {
                if ((value != null) && (value != DBNull.Value))
                {
                    iReturn = Convert.ToDateTime(value);
                }
            }
            catch
            { }
            return iReturn;
        }

        public static int ValidateBooleanToInt(Object value)
        {
            int iReturn = 0;
            try
            {
                if ((value != null) && (value != DBNull.Value))
                {
                    if (Convert.ToBoolean(value) == true)
                    {
                        iReturn = 1;
                    }
                }
            }
            catch
            { }
            return iReturn;
        }

        public static String ValidateIntegerToString(Object value)
        {
            String iReturn = "";
            try
            {
                if ((value != null) && (value != DBNull.Value))
                {
                    iReturn = Convert.ToInt64(value.ToString()).ToString();
                }
            }
            catch
            { }
            return iReturn;
        }

        public static Boolean ValidateDomain()
        {
            Boolean validDomain = true;
            try
            {
                String strUrl = "";

                //if (HttpContext.Current.Request.UrlReferrer != null)
                //{
                //    strUrl = HttpContext.Current.Request.UrlReferrer.ToString();
                //}
                //else
                //{
                //    strUrl = HttpContext.Current.Request.Url.ToString();
                //}



                if (strUrl != "")
                {
                    if (!System.String.IsNullOrEmpty(ConfigurationManager.AppSettings("appSettings:allowedDomains")))
                    {
                        String allowedDomains = ConfigurationManager.AppSettings("appSettings:allowedDomains").ToString().Trim();

                        if (allowedDomains == "*")
                        {
                            // ak all domains.
                        }
                        else
                        {
                            string[] domains = allowedDomains.Split(',');

                            foreach (string strdomain in domains)
                            {
                                if (strUrl.StartsWith(strdomain))
                                {
                                    // ok
                                    validDomain = true;
                                    break;
                                }
                                else
                                {
                                    validDomain = false;
                                }
                            }
                        }
                    }
                }
            }
            catch
            {
                validDomain = false;
            }

            return validDomain;
        }

        public static Boolean validateNotValidChars(string str)
        {
            Boolean allow = true;
            if (!System.String.IsNullOrEmpty(str))
            {
                if (str.Contains("'") || str.Contains("*") || str.Contains("''") || str.Contains("\"") || str.Contains("/") || str.Contains("+") || str.Contains("=") || str.ToLower().Contains("select ") ||
                    str.ToLower().Contains(" or ") || str.ToLower().Contains(" and ") || str.ToLower().Contains(" from ") || str.ToLower().Contains(" join ") || str.ToLower().Contains("insert ") || str.ToLower().Contains("update "))
                {
                    allow = false;
                }
            }
            return allow;

        }

        public static Boolean validateNumbers(string str)
        {
            Regex ObjAlphaPattern = new Regex(@"^[0-9]*$");
            return ObjAlphaPattern.IsMatch(str);
        }

        public static Boolean validateToken(string str)
        {
            Boolean allow = true;
            if (!System.String.IsNullOrEmpty(str))
            {

                return validateNotValidCharsToken(str);

            }
            return allow;

        }

        public static Boolean validateNotValidCharsToken(string str)
        {

            Boolean allow = true;

            if ((str.Length < 200) || (str.Length > 300))
            {
                allow = false;
            }
            else
            {
                if (!System.String.IsNullOrEmpty(str))
                {
                    if (str.Contains("'") || str.Contains("\"") || str.Contains("/") || str.ToLower().Contains("select ") ||
                        str.ToLower().Contains(" or ") || str.ToLower().Contains(" and ") || str.ToLower().Contains(" from ") || str.ToLower().Contains(" join ") || str.ToLower().Contains("insert ") || str.ToLower().Contains("update "))
                    {
                        allow = false;
                    }
                }
            }
            return allow;

        }
  



    }

    public class ErrorInfo
    {
        public Boolean hasError { get; set; }
        public string errorDesc { get; set; }
        public string errorInter { get; set; }
        public int errorNum { get; set; }
        public ErrorInfo()
        {
            hasError = false;
            errorDesc = "";
            errorInter = "";
            errorNum = 0;
        }

    }

    public class simpleBool : ErrorInfo
    {
        public Boolean response { get; set; }
    }

    public class simpleString : ErrorInfo
    {
        public string response { get; set; }
    }

    public class simpleDate : ErrorInfo
    {
        public DateTime response { get; set; }
    }


    public class simpleStringNotToCall : ErrorInfo
    {
        public string status { get; set; }
        public string response { get; set; }
    }

    public class simpleInteger : ErrorInfo
    {
        public int response { get; set; }
    }

    public class simpleDecimal : ErrorInfo
    {
        public decimal response { get; set; }

    }

    public class simpleDataset : ErrorInfo
    {
        public System.Data.DataSet dt { get; set; }

        public simpleDataset()
        {
            dt = new System.Data.DataSet();
        }
    }

}
