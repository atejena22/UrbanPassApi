using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Drawing.Imaging;
using QRCoder;
using System.Drawing;

namespace inTouchApi.Models
{
    public class QRS
    {
        
        string imagesQRS = ConfigurationManager.AppSettings("appSettings:imagesQRS");
        string imagesQRSURL = ConfigurationManager.AppSettings("appSettings:imagesQRSURL");
        string imagesQRSAccess = ConfigurationManager.AppSettings("appSettings:imagesQRSAccess");


        public simpleString getQR(int userID)
        {
            return getQR(userID.ToString(), userID + ".png");
        }

        public simpleString getQR(string txtCode, string imageName) 
        {
            simpleString objResp = new simpleString();

            try
            {
                // string qrURL = imagesQRSAccess + txtCode; 
                // string imageName = txtCode + ".png"; 
                QRCodeGenerator qrGenerator = new QRCodeGenerator(); 
                QRCodeData data = qrGenerator.CreateQrCode(txtCode, QRCodeGenerator.ECCLevel.Q);
                QRCode code = new QRCode(data);
                Bitmap imageQA = code.GetGraphic(5); 
                imageQA.Save(imagesQRS + imageName, ImageFormat.Png);
                objResp.response = imagesQRSURL + imageName;
            }
            catch (Exception ex)
            {
                objResp.hasError = true;
                objResp.errorDesc = ex.Message.ToString();
            } 

            return objResp;
        }

    }



}
