using API_Empleados.Models;
using API_Empleados.Models.DTOs;
using API_Empleados.Repository;
using API_Empleados.Repository.IRepository;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace API_Empleados.Controllers
{
    [Authorize]
    [Route("api/Usuarios")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "ApiUsuarios")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]

    public class UsuarioController : Controller
    {

        public readonly IUsuarioRepository _RepoUsuario;
        public IMapper _mapper;
        private readonly IConfiguration _config;

        public UsuarioController(IUsuarioRepository RepoUsuario, IMapper mapper, IConfiguration config)
        {
            _RepoUsuario = RepoUsuario;
            _mapper = mapper;
            _config = config;
        }

        /// <summary>
        /// Obtener Listado de Usuarios Con Roles
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<UsuarioDto>))]
        [ProducesResponseType(400)]
        public IActionResult ObtenerUsuarios()
        {
            var listaUsuarios = _RepoUsuario.ObtenerUsuarios();

            var listaUsuariosDto = new List<UsuarioDto>();

            foreach (var lista in listaUsuarios)
            {
                listaUsuariosDto.Add(_mapper.Map<UsuarioDto>(lista));
            }

            return Ok(listaUsuariosDto);
        }

        /// <summary>
        /// Obtener Listado de Usuarios por ID
        /// </summary>
        /// <param name="usuarioId"></param>
        /// <returns></returns>
        [HttpGet("{usuarioId:Int}", Name = "ObtenerUsuario")]
        [ProducesResponseType(200, Type = typeof(UsuarioDto))]
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]

        public IActionResult ObtenerUsuario(int usuarioId)
        {
            var usuario = _RepoUsuario.ObtenerUsuario(usuarioId);

            if (usuario == null)
            {
                return NotFound();
            }

            var usuarioDto = _mapper.Map<UsuarioDto>(usuario);
            return Ok(usuarioDto);
        }

        /// <summary>
        /// Registrar nuevo Usuario
        /// </summary>
        /// <param name="usuarioRegistrarDto"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("Registrarse")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public IActionResult Registrarse(UsuarioRegistrarDto usuarioRegistrarDto)
        {
            usuarioRegistrarDto.Usuario = usuarioRegistrarDto.Usuario.ToLower();

            if (_RepoUsuario.ExisteUsuario(usuarioRegistrarDto.Usuario))
            {
                return BadRequest("El usuario existe");
            }

            var usuarioACrear = new Usuario
            {
                UsuarioA = usuarioRegistrarDto.Usuario
            };

            var usuarioCreado = _RepoUsuario.Registrarse(usuarioACrear, usuarioRegistrarDto.Password);

            return Ok(usuarioCreado);
        }


        /// <summary>
        /// Acceso/Autenticación de Usuario
        /// </summary>
        /// <param name="usuarioLoginDto"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("Login")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public IActionResult Login(UsuarioLoginDto usuarioLoginDto)
        {

            var usuarioDesdeRepo = _RepoUsuario.Login(usuarioLoginDto.Usuario, usuarioLoginDto.Password);

            if (usuarioDesdeRepo == null)
            {
                return Unauthorized();
            }

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, usuarioDesdeRepo.Id.ToString()),
                new Claim(ClaimTypes.Name, usuarioDesdeRepo.UsuarioA.ToString())
            };

            //Generacion de TOKEN

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value));
            var credenciales = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = credenciales
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return Ok(new
            {
                token = tokenHandler.WriteToken(token)
            });
        }



    }
}
