using API_Empleados.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_Empleados.Repository.IRepository
{
    public interface ICargoRepository
    {
        ICollection<Cargo> ObtenerCargos();
        ICollection<Cargo> ObtenerCargosPorDepartamentos(int departamentoId);
        Cargo ObtenerCargo(int cargoId);
        bool ExisteCargo(int cargoId);
        bool ExisteCargo(string NombreCargo);
        IEnumerable<Cargo> BuscarCargo(string NombreCargo);
        bool CrearCargo(Cargo cargo);
        bool ActualizarCargo(Cargo cargo);
        bool BorrarCargo(Cargo cargo);
        bool Guardar();
    }
}
