using System.ComponentModel.DataAnnotations;

namespace inTouchApi.Request
{
    public class AuthRequest
    {
        //[Required]
        public string userName { get; set; }
        //[Required]
        public string password { get; set; }
    }
}
