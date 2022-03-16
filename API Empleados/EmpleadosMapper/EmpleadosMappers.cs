using API_Empleados.Models;
using API_Empleados.Models.DTOs;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_Empleados.EmpleadosMapper
{
    public class EmpleadosMappers : Profile
    {
        public EmpleadosMappers()
        {
            CreateMap<Departamento, DepartamentoDto>().ReverseMap();
            CreateMap<Cargo, CargoDto>().ReverseMap();
            CreateMap<Empleado, EmpleadoDto>().ReverseMap();
            CreateMap<Empleado, EmpleadoCrearDto>().ReverseMap();
            CreateMap<Empleado, EmpleadoActualizarDto>().ReverseMap();
            CreateMap<Usuario, UsuarioDto>().ReverseMap();
            CreateMap<Usuario, UsuarioLoginDto>().ReverseMap();
            CreateMap<Usuario, UsuarioRegistrarDto>().ReverseMap();


        }

    }
}
