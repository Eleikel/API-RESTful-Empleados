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
    [Route("api/cargos")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "ApiCargos")]

    public class CargosController : Controller
    {
        public readonly ICargoRepository _cargoRepo;
        public readonly IMapper _mapper;

        public CargosController(ICargoRepository _cargoRepo, IMapper _mapper)
        {
            this._cargoRepo = _cargoRepo;
            this._mapper = _mapper;
        }

        /// <summary>
        /// Obtener listado de Cargos
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<CargoDto>))]
        [ProducesResponseType(400)]
        public IActionResult ObtenerCargos()
        {
            var listaCargos = _cargoRepo.ObtenerCargos();

            var listaCargosDto = new List<CargoDto>();

            foreach (var item in listaCargos)
            {
                listaCargosDto.Add(_mapper.Map<CargoDto>(item));
            }
            return Ok(listaCargosDto);

        }


        /// <summary>
        /// Obtener cargos individual por ID
        /// </summary>
        /// <param name="cargoId"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("{cargoId:int}")]
        [ProducesResponseType(200, Type = typeof(CargoDto))]  // El 'ProducesResponseType' es importante ponerlo
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
        public IActionResult ObtenerCargo(int cargoId)
        {
            var cargoPorId = _cargoRepo.ObtenerCargo(cargoId);

            if (cargoPorId == null)
            {
                return NotFound();
            }

            var cargoDto = _mapper.Map<CargoDto>(cargoPorId);
            return Ok(cargoDto);
        }



        /// <summary>
        /// Obtener Cargo por su departamento
        /// </summary>
        /// <param name="departamentoId"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("ObtenerCargoPorDepartamento/{departamentoId:int}")]
        [ProducesResponseType(200, Type = typeof(List<CargoDto>))]
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
        public IActionResult ObtenerCargoPorDepartamento(int departamentoId)
        {
            var listaCargo = _cargoRepo.ObtenerCargosPorDepartamentos(departamentoId);

            if (listaCargo == null)
            {
                return NotFound();
            }

            var CargoPorDepartamento = new List<CargoDto>();

            foreach (var cargo in listaCargo)
            {
                CargoPorDepartamento.Add(_mapper.Map<CargoDto>(cargo));
            }

            return Ok(CargoPorDepartamento);
        }

        /// <summary>
        /// Crear un nuevo Cargo
        /// </summary>
        /// <param name="cargoDto"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(200, Type = typeof(CargoDto))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CrearCargo([FromBody] CargoDto cargoDto)
        {
            if (cargoDto == null)
            {
                return BadRequest(ModelState);
            }

            if (_cargoRepo.ExisteCargo(cargoDto.NombreCargo))
            {
                ModelState.AddModelError("", $"Ya existe un cargo con el nombre {cargoDto.NombreCargo}");
                return StatusCode(404, ModelState);
            }

            var cargo = _mapper.Map<Cargo>(cargoDto);

            if (!_cargoRepo.CrearCargo(cargo))
            {
                ModelState.AddModelError("", $"Algo salio mal intentando Crear el Cargo {cargo.NombreCargo}");
                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("ObtenerCargo", new { cargo = cargo.Id }, cargo);
        }
        /// <summary>
        /// Actualizar Cargo existente
        /// </summary>
        /// <param name="cargoId"></param>
        /// <param name="cargoDto"></param>
        /// <returns></returns>
        [HttpPatch("{cargoId:int}", Name = "ActualizarCargo")]
        [ProducesResponseType(204)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public IActionResult ActualizarCargo(int cargoId, [FromBody] CargoDto cargoDto)
        {
            if (cargoDto == null || cargoDto.Id != cargoId)
            {
                return BadRequest(ModelState);
            }

            var cargo = _mapper.Map<Cargo>(cargoDto);

            if (!_cargoRepo.ActualizarCargo(cargo))
            {
                ModelState.AddModelError("", $"No se pudo Actualizar el Cargo {cargo.NombreCargo}");
                return StatusCode(500, ModelState);
            }

            return NoContent();

        }

        /// <summary>
        /// Borrar Cargo existente
        /// </summary>
        /// <param name="cargoId"></param>
        /// <returns></returns>
        [HttpDelete("{cargoId:int}", Name = "BorrarCargo")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult BorrarCargo(int cargoId)
        {
            if (!_cargoRepo.ExisteCargo(cargoId))
            {
                ModelState.AddModelError("", "Este cargo No existe");
                return NotFound();
            }

            var cargo = _cargoRepo.ObtenerCargo(cargoId);

            if (!_cargoRepo.BorrarCargo(cargo))
            {
                ModelState.AddModelError("", $"No se pudo borrar el Cargo {cargo.NombreCargo}");
                return StatusCode(500, ModelState);
            }

            return Content("Cargo Eliminado Sastifactoriamente");
        }

        /// <summary>
        /// Buscar cargo por nombre
        /// </summary>
        /// <param name="NombreCargo"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("BuscarCargo")] // El request de esto se hace por Params no por Body
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public IActionResult BuscarCargo(string NombreCargo)
        {
            try
            {
                var resultadoDeBusqueda = _cargoRepo.BuscarCargo(NombreCargo);

                if (resultadoDeBusqueda.Any())
                {
                    return Ok(resultadoDeBusqueda);
                }

                return NotFound();

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error recuperando datos");
            }
        }


    }
}
