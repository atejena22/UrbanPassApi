using inTouchApi.Model;
using inTouchApi.Request;
using inTouchApi.Response;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Threading.Tasks;

namespace inTouchApi.Service
{
    public interface UserInterface
    {

        UserResponse Auth(AuthRequest model);
        Task<Respuesta> PresntarUsuario();
        Task<Respuesta> PresEntarUsuarioActual(int id);
        Task<Respuesta> GenerarUsuario([FromBody] User usuario);
        Task<Respuesta> ActualizarPasswordUsuario([FromBody] User usuario);
        Task<Respuesta> ActualizarUsuarioTrueAdmin([FromBody] User usuario);
        Task<Respuesta> PutUsuarioCompletoActualizar([FromBody] User usuario);

        Task<Respuesta> GetRol();
        Task<Respuesta> ResetearPasswordUserAdmin([FromBody] User usuario);

    }


}
