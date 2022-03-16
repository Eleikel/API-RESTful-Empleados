using API_Empleados.Models;
using API_Empleados.Models.DTOs;
using API_Empleados.Repository.IRepository;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_Empleados.Controllers
{
    [Authorize]
    [Route("api/Departamentos")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "ApiDepartamentos")]

    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public class DepartamentosController : Controller
    {
        public IDepartamentoRepository _repoDept;
        public IMapper _mapper;

        public DepartamentosController(IDepartamentoRepository repoDept, IMapper mapper)
        {
            _repoDept = repoDept;
            _mapper = mapper;
        }

        /// <summary>
        /// Obtener Lista de Departamentos
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        public IActionResult ObtenerDepartamentos()
        {
            var listaDepartamentos = _repoDept.ObtenerDepartamentos();

            //DTO
            var listaDepartamentosDto = new List<DepartamentoDto>();

            foreach (var item in listaDepartamentos)
            {
                listaDepartamentosDto.Add(_mapper.Map<DepartamentoDto>(item));
            }

            return Ok(listaDepartamentosDto);

        }

        /// <summary>
        /// Obtener Departamento por ID
        /// </summary>
        /// <param name="departamentoId"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("{departamentoId:int}", Name = "ObtenerDepartamento")]
        public IActionResult ObtenerDepartamento(int departamentoId)
        {
            var departamentoPorId = _repoDept.ObtenerDepartamento(departamentoId);

            if (departamentoPorId == null)
            {
                return NotFound();
            }

            //DTO
            var departamentoDto = _mapper.Map<DepartamentoDto>(departamentoPorId);
            return Ok(departamentoDto);
        }

        /// <summary>
        /// Crear nuevo Departamento
        /// </summary>
        /// <param name="departamentoDto"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult CrearDepartamento([FromBody] DepartamentoDto departamentoDto)
        {
            if (departamentoDto == null)
            {
                return BadRequest(ModelState);
            }

            if (_repoDept.ExisteDepartamento(departamentoDto.Nombre) && _repoDept.ExisteDepartamento(departamentoDto.Telefono))
            {
                ModelState.AddModelError("", "El Nombre o el Numero telefonico del departamento ya existen");
                return StatusCode(404, ModelState);
            }

            var departamento = _mapper.Map<Departamento>(departamentoDto);

            if (!_repoDept.CrearDepartamento(departamento))
            {
                ModelState.AddModelError("", $"Algo salio mal intentando Crear el Departamento {departamento.Nombre}");
                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("ObtenerDepartamento", new { departamento = departamento.Id },departamento);
        }


        /// <summary>
        /// Actualizar Departamento Existente
        /// </summary>
        /// <param name="departamentoId"></param>
        /// <param name="departamentoDto"></param>
        /// <returns></returns>
        [HttpPatch("{departamentoId:int}", Name = "ActualizarDepartamento")]
        public IActionResult ActualizarDepartamento(int departamentoId, [FromBody] DepartamentoDto departamentoDto)
        {
            if (departamentoDto == null || departamentoId != departamentoDto.Id)
            {
                return BadRequest(ModelState);
            }

            var departamento = _mapper.Map<Departamento>(departamentoDto);

            if (!_repoDept.ActualizarDepartamento(departamento))
            {
                ModelState.AddModelError("", $"Algo salio mal intentando Actualizar el Departamento {departamento.Nombre}");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }

        /// <summary>
        /// Eliminar Departamento existente
        /// </summary>
        /// <param name="departamentoId"></param>
        /// <returns></returns>
        [HttpDelete("{departamentoId:int}", Name ="BorrarDepartamento")] //En delete no se jode con DTO
        public IActionResult BorrarDepartamento(int departamentoId)
        {
            if (!_repoDept.ExisteDepartamento(departamentoId))
            {
                return NotFound(ModelState);
            }

            var departamento = _repoDept.ObtenerDepartamento(departamentoId);

            if (!_repoDept.BorrarDepartamento(departamento))
            {
                ModelState.AddModelError("", $"Algo salio mal no se pudo Borrar el departamento {departamento.Nombre}");
            }

            return NoContent();
        }


    }
}
