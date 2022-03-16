using API_Empleados.Models;
using System.Collections.Generic;

namespace API_Empleados.Repository.IRepository
{
    public interface IEmpleadoRepository
    {

        ICollection<Empleado> ObtenerEmpleados();
        Empleado ObtenerEmpleado(int empleadoId);        
        ICollection<Empleado> ObtenerEmpleadosPorCargos(int cargoId);
        bool ExisteEmpleado(int empleadoId);
        bool ExisteEmpleado(string nombreEmpleado, string apellidoEmpleado);
        IEnumerable<Empleado> BuscarEmpleado(string nombre);
        bool CrearEmpleado(Empleado empleado);
        bool ActualizarEmpleado(Empleado empleado);
        bool BorrarEmpleado(Empleado empleado);
        bool Guardar();



    }
}
