using API_Empleados.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_Empleados.Repository.IRepository
{
    public interface IDepartamentoRepository
    {
        ICollection<Departamento> ObtenerDepartamentos();
        Departamento ObtenerDepartamento(int departamentoId);
        bool ExisteDepartamento(int departamentoId);
        bool ExisteDepartamento(string NombreDepartamento);
        bool CrearDepartamento(Departamento departamento);
        bool ActualizarDepartamento(Departamento departamento);
        bool BorrarDepartamento(Departamento departamento);
        bool Guardar();


    }
}
