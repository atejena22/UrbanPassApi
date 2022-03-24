using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace inTouchApi.Models
{
    public class Login
    {

        string sConn = ConfigurationManager.AppSettings("connectionStrings:SQLConn");

        string secretKey = ConfigurationManager.AppSettings("appSettings:secretKey");
     
        public UserLogin getLogin(LoginRequest inObj)
        {

            UserLogin objLoginResponse = new UserLogin();

            try
            {
                Users objUser = new Users();
                User objLogin =  objUser.getUser(inObj);
                if (objLogin.hasError)
                {
                    objLoginResponse.hasError = objLogin.hasError;
                    objLoginResponse.errorDesc = objLogin.errorDesc;
                    objLoginResponse.errorInter = objLogin.errorInter;
                    objLoginResponse.errorNum = objLogin.errorNum;
                }
                else
                {
                    tokenRequest objTokenResponse  = buildToken(objLogin.userID);
                    if (objTokenResponse.hasError)
                    {
                        objLoginResponse.hasError = objTokenResponse.hasError;
                        objLoginResponse.errorDesc = objTokenResponse.errorDesc;
                        objLoginResponse.errorInter = objTokenResponse.errorInter;
                        objLoginResponse.errorNum = objTokenResponse.errorNum;
                    }
                    else
                    { 
                        // objLoginResponse.token = objTokenResponse.token;
                        objLoginResponse = objUser.getUser(objLogin.userID, objTokenResponse.token); 
                    }
                }
            }
            catch (Exception ex)
            {
                objLoginResponse.hasError = true;
                objLoginResponse.errorDesc = ex.Message.ToString();
                objLoginResponse.errorInter = ex.Message.ToString();
                objLoginResponse.errorNum = 101;
            }

            return objLoginResponse;
             
        }

        public User getValidateToken(tokenRequest objIn)
        {

            User objUser = new User();

            try
            {
                Users objUsers = new Users();

                tokenResponse objUserValidate = validateToken(objIn);
                if (objUserValidate.hasError)
                {
                    objUser.hasError = objUserValidate.hasError;
                    objUser.errorDesc = objUserValidate.errorDesc;
                    objUser.errorInter = objUserValidate.errorInter;
                    objUser.errorNum = objUserValidate.errorNum;
                }
                else
                {
                    objUser = objUsers.getUser(objUserValidate.userId);
                }
            } 
            catch (Exception ex)
            {
                objUser.hasError = true;
                objUser.errorDesc = ex.Message.ToString();
                objUser.errorInter = ex.Message.ToString();
                objUser.errorNum = 102;
            }
            return objUser;

        }




        public tokenResponse validateToken(tokenRequest objIn)
        {

            tokenResponse objResponse = new tokenResponse();

            if (objIn.token == null)
                return null;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secretKey);
            try
            {
                tokenHandler.ValidateToken(objIn.token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var userId = int.Parse(jwtToken.Claims.First(x => x.Type == "userId").Value);

                // return user id from JWT token if validation successful
                objResponse.userId = userId;
            }
            catch ( Exception ex)
            {
                objResponse.hasError = true;
                objResponse.errorDesc = ex.Message.ToString();
                objResponse.errorNum = 3001;
            }

            return objResponse;

        }

        private tokenRequest buildToken(int userId)
        {
            tokenRequest objTokenResponse = new tokenRequest();
            var claims = new[]{
                new Claim("userId",userId.ToString()),
                //new Claim("browser",objRequest.browser),
                //new Claim(ClaimTypes.NameIdentifier , "jesuscoronado"),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
            };
            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(this.secretKey));
            var creds = new SigningCredentials(key, Microsoft.IdentityModel.Tokens.SecurityAlgorithms.HmacSha256);
            var expiration = DateTime.UtcNow.AddHours(24);
            JwtSecurityToken token = new JwtSecurityToken(
                issuer: "store.net",
                audience: "store.net",
                claims: claims,
                expires: expiration,
                signingCredentials: creds
             );
            objTokenResponse.token = new JwtSecurityTokenHandler().WriteToken(token);



            return objTokenResponse;
        }


    }

    public class LoginRequest
    {
        public string userName { get; set; }
        public string password { get; set; }


        public LoginRequest()
        {
            userName = "";
            password = "";

        }

    }

    public class LoginResponse : ErrorInfo
    {
        public string token { get; set; }
    }


    public class tokenRequest  : ErrorInfo
    {
        public string token { get; set; }

        public tokenRequest()
        {
            token = "";
        }
    }

    public class tokenResponse : ErrorInfo
    {
        public int userId { get; set; }
      
        public tokenResponse()
        {
            userId = 0;           
        }
    }


}
