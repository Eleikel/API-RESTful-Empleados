using API_Empleados.Models;
using API_Empleados.Models.DTOs;
using API_Empleados.Repository.IRepository;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace API_Empleados.Controllers
{
    [Authorize]
    [Route("api/Empleados")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "ApiEmpleados")]

    public class EmpleadosController : Controller
    {
        public readonly IMapper _mapper;
        public readonly IEmpleadoRepository _RepoEmpleado;
        public readonly IWebHostEnvironment _hostingEnvironment;

        public EmpleadosController(IEmpleadoRepository _RepoEmpleado, IMapper _mapper, IWebHostEnvironment _hostingEnvironment)
        {
            this._mapper = _mapper;
            this._RepoEmpleado = _RepoEmpleado;
            this._hostingEnvironment = _hostingEnvironment;
        }


        /// <summary>
        /// Obtener el listado de los Empleados
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        public IActionResult ObtenerEmpleados()
        {
            var Listaempleados = _RepoEmpleado.ObtenerEmpleados();

            var empleadosDto = new List<EmpleadoDto>();

            foreach (var lista in Listaempleados)
            {
                empleadosDto.Add(_mapper.Map<EmpleadoDto>(lista));
            }
            return Ok(empleadosDto);

        }

        /// <summary>
        /// Obtener Empleado por ID
        /// </summary>
        /// <param name="empleadoId"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("{empleadoId:int}", Name = "ObtenerEmpleado")]
        public IActionResult ObtenerEmpleado(int empleadoId)
        {
            var empleado = _RepoEmpleado.ObtenerEmpleado(empleadoId);

            if (empleado == null)
            {
                return NotFound();
            }

            var empleadoDto = _mapper.Map<EmpleadoDto>(empleado);
            return Ok(empleadoDto);
        }

        /// <summary>
        /// Buscar Empleado por nombre
        /// </summary>
        /// <param name="nombre"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("Buscar")]
        public IActionResult BuscarEmpleado(string nombre)
        {
            try
            {
                var resultado = _RepoEmpleado.BuscarEmpleado(nombre);
                if (resultado.Any())
                {
                    return Ok(resultado);
                }
                return NotFound();

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error recuperando datos");
            }
        }


        /// <summary>
        /// Obtener Empleados por Cargos
        /// </summary>
        /// <param name="cargoId"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("ObtenerEmpleadosPorCargos/{cargoId:int}")]
        [ProducesResponseType(200, Type = typeof(List<EmpleadoDto>))]
        public IActionResult ObtenerEmpleadosPorCargos(int cargoId)
        {
            var listaEmpleado = _RepoEmpleado.ObtenerEmpleadosPorCargos(cargoId);

            if (listaEmpleado == null)
            {
                return NotFound();
            }

            var itemEmpleado = new List<EmpleadoDto>();

            foreach (var item in listaEmpleado)
            {
                itemEmpleado.Add(_mapper.Map<EmpleadoDto>(item));
            }

            return Ok(itemEmpleado);
        }


        /// <summary>
        /// Crear nuevo Empleado
        /// </summary>
        /// <param name="empleadoCrearDto"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult CrearEmpleado([FromForm] EmpleadoCrearDto empleadoCrearDto)
        {

            if (empleadoCrearDto == null)
            {
                return BadRequest(ModelState);
            }

            if (_RepoEmpleado.ExisteEmpleado(empleadoCrearDto.Nombre, empleadoCrearDto.Apellido))
            {
                ModelState.AddModelError("", $"El Empleado {empleadoCrearDto.Nombre} {empleadoCrearDto.Apellido} existe. Intente crear uno nuevo");
                return StatusCode(404, ModelState);
            }

            /******************************************************************************/
            /* Subida de archivos */

            var archivo = empleadoCrearDto.Foto;
            string rutaPrincipal = _hostingEnvironment.WebRootPath;
            var archivos = HttpContext.Request.Form.Files;

            if (archivo.Length > 0)
            {
                //Nueva imagen

                var nombreFoto = Guid.NewGuid().ToString();
                var subidas = Path.Combine(rutaPrincipal, @"fotos");
                var extension = Path.GetExtension(archivos[0].FileName);

                using (var fileStream = new FileStream(Path.Combine(subidas, nombreFoto + extension), FileMode.Create))
                {
                    archivos[0].CopyTo(fileStream);
                }

                empleadoCrearDto.RutaImagen = @"\fotos\" + nombreFoto + extension;
            }


            /******************************************************************************/
            var empleado = _mapper.Map<Empleado>(empleadoCrearDto);

            if (!_RepoEmpleado.CrearEmpleado(empleado))
            {
                ModelState.AddModelError("", $"No se pudo Crear el registro de empleado {empleado.Nombre}");
                StatusCode(500, ModelState);
            }

            return CreatedAtRoute("ObtenerEmpleado", new { empleado = empleado.Id }, empleado);
        }



        /// <summary>
        /// Actualizar Empleado existente
        /// </summary>
        /// <param name="empleadoId"></param>
        /// <param name="empleadoActualizarDto"></param>
        /// <returns></returns>
        [HttpPatch("{empleadoId:int}", Name = "ActualizarEmpleado")]
        public IActionResult ActualizarEmpleado(int empleadoId, [FromBody] EmpleadoActualizarDto empleadoActualizarDto)
        {
            if (empleadoActualizarDto == null || empleadoId != empleadoActualizarDto.Id)
            {
                return BadRequest(ModelState);
            }

            var empleado = _mapper.Map<Empleado>(empleadoActualizarDto);

            if (!_RepoEmpleado.ActualizarEmpleado(empleado))
            {
                ModelState.AddModelError("", $"No se pudo actualizar el empleado {empleado.Nombre}");
                return StatusCode(500, ModelState);
            }

            return Content("Datos actualizados Sastifactoriamente");
        }

        /// <summary>
        /// Borrar Empleado existente
        /// </summary>
        /// <param name="empleadoId"></param>
        /// <returns></returns>
        [HttpDelete("{empleadoId:int}", Name = "BorrarEmpleado")]
        public IActionResult BorrarEmpleado(int empleadoId)
        {
            if (!_RepoEmpleado.ExisteEmpleado(empleadoId))
            {
                return NotFound();
            }

            var empleado = _RepoEmpleado.ObtenerEmpleado(empleadoId);

            if (!_RepoEmpleado.BorrarEmpleado(empleado))
            {
                ModelState.AddModelError("", $"No se pudo eliminar el registro {empleado.Nombre}");
                return StatusCode(500, ModelState);
            }
            return Content("Usuario Eliminado Sastifactoriamente");
        }

    }
}
