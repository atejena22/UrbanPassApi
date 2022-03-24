using inTouchApi.Model;
using inTouchApi.Response;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace inTouchApi.Interface
{
    public interface UrbanizationInterface
    {
        Task<Respuesta> PresentarUrbanizacion();
        Task<Respuesta> PresentarUrbanizacionActual(int id);
        Task<Respuesta> InsertarNuevaUrbanizacion([FromBody] Urbanization urbanization);
        Task<Respuesta> ActualizarUrbanizacion([FromBody] Urbanization urbanization);

        /*
          UserResponse Auth(AuthRequest model);
        Task<Respuesta> PresntarUsuario();
        Task<Respuesta> GenerarUsuario([FromBody] User usuario);
        Task<Respuesta> ActualizarPasswordUsuario([FromBody] User usuario);
        Task<Respuesta> ActualizarUsuarioTrueAdmin([FromBody] User usuario);
        Task<Respuesta> PutUsuarioCompletoActualizar([FromBody] User usuario);

         */
    }
}
