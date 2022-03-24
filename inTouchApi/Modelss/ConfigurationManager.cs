using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using System.IO;

namespace inTouchApi.Models
{
    public class ConfigurationManager
    {

        public static string AppSettings(string _property)
        {
            var json = File.ReadAllText("appsettings.json");
            var appSettings = JsonDocument.Parse(json, new JsonDocumentOptions { CommentHandling = JsonCommentHandling.Skip });

            var objParse = appSettings.RootElement;
            foreach (string _prop in _property.Split(':'))
            {
                objParse = objParse.GetProperty(_prop);
            }

            return objParse.GetString();
        }

    }
}
