using inTouchApi.Model;
using inTouchApi.Request;
using inTouchApi.Response;
using inTouchApi.Service;
using Microsoft.AspNetCore.Authorization;
//using inTouchApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Threading.Tasks;


namespace inTouchApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
    
        private UserInterface _icontext;
        public UserController(UserInterface icontext)
        {
            _icontext = icontext;
        }

        [HttpGet("rol")]
        public async Task<ActionResult> GetRoles()
        {
            Respuesta usuarioResponse = await _icontext.GetRol();
            if (usuarioResponse.Exito == 1)
            {
                return Ok(usuarioResponse);
            }
            else
            {
                return Ok(usuarioResponse.Mensaje);
            }
        }

        /*-----------------------------------Presentar lista de Usuario --------------------------------------*/
        [HttpGet]
        public async Task<ActionResult> GetUsuario()
        {
            Respuesta usuarioResponse = await _icontext.PresntarUsuario();
            if (usuarioResponse.Exito == 1)
            {
                return Ok(usuarioResponse);
            }
            else
            {
                return Ok(usuarioResponse.Mensaje);
            }
        }

        /*-----------------------------------Presentar Usuario Actual --------------------------------------*/
        [HttpGet("{id}")]
        public async Task<ActionResult> GetUsuarioActual(int id)
        {
            Respuesta usuarioResponse = await _icontext.PresEntarUsuarioActual(id);
            if (usuarioResponse.Exito == 1)
            {
                return Ok(usuarioResponse);
            }
            else
            {
                return Ok(usuarioResponse.Mensaje);
            }
        }

 

        /*-----------------------------------Para Loguearse con Token --------------------------------------*/
        [HttpPost("login")]
        public IActionResult Autentificar([FromBody] AuthRequest model)
        {
            Respuesta respuesta = new Respuesta();
            var userresponse = _icontext.Auth(model);

            if (userresponse == null)
            {
                respuesta.Exito = 0;
                respuesta.Mensaje = "Usuario o Contraseña Incorrecta";
                return Ok(respuesta);
            }else if (userresponse.roleID==3)
            {
                respuesta.Exito = 3;
                respuesta.Mensaje = "Su usuario no tiene acceso a esta cuenta";
                return Ok(respuesta);
            }
            else
            {
                respuesta.Exito = 1;
                respuesta.Mensaje = "Usuario Ingresado con éxito";
                respuesta.Data = userresponse;
                return Ok(respuesta);
            }

        }

        /*-----------------------------------Aactualizar Contraseña en la app Movil --------------------------------------*/
        [HttpPut]
        public async Task<ActionResult> PutUsuario([FromBody] User usuario)
        {
            Respuesta usuarioResponse = await _icontext.ActualizarPasswordUsuario(usuario);
            if (usuarioResponse.Exito == 1)
            {
                return Ok(usuarioResponse);
            }
            else
            {
                return Ok(usuarioResponse.Mensaje);
            }
        }

        /*-----------------------------------Activar o desactivar Usuario --------------------------------------*/
        [HttpPut("activar")]
        public async Task<ActionResult> PutUsuarioAdmin([FromBody] User usuario)
        {
            Respuesta usuarioResponse = await _icontext.ActualizarUsuarioTrueAdmin(usuario);
            if (usuarioResponse.Exito == 1)
            {
                return Ok(usuarioResponse);
            }
            else
            {
                return Ok(usuarioResponse.Mensaje);
            }
        }

        /*-----------------------------------Actualizar Usuario Completo --------------------------------------*/
        [HttpPut("actualizarUsuario")]
        public async Task<ActionResult> PutUsuarioActualizar([FromBody] User usuario)
        {
            Respuesta usuarioResponse = await _icontext.PutUsuarioCompletoActualizar(usuario);
            if (usuarioResponse.Exito == 1)
            {
                return Ok(usuarioResponse);
            }
            else
            {
                return Ok(usuarioResponse.Mensaje);
            }
        }

        /*-----------------------------------generarUsuarioo --------------------------------------*/

        [HttpPost("generarUsuario")]
        public async Task<ActionResult> PostUsuarioNuevo([FromBody] User usuario)
        {
            Respuesta usuarioResponse = await _icontext.GenerarUsuario(usuario);
            if (usuarioResponse.Exito == 1)
            {
                return Ok(usuarioResponse);
            }
            else
            {
                return Ok(usuarioResponse.Mensaje);
            }
        }


        /*----------------------------------- RESETEAR CONTRASEÑA DEL USUARIO --------------------------------------*/
        [HttpPut("resetPassword")]
        public async Task<ActionResult> PutResetPasswordAdminUser([FromBody] User usuario)
        {
            Respuesta usuarioResponse = await _icontext.ResetearPasswordUserAdmin(usuario);
            if (usuarioResponse.Exito == 1)
            {
                return Ok(usuarioResponse);
            }
            else
            {
                return Ok(usuarioResponse.Mensaje);
            }
        }



    }
}
