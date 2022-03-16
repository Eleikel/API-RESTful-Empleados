using API_Empleados.Models;
using API_Empleados.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace API_Empleados.Repository
{
    public class EmpleadoRepository : IEmpleadoRepository
    {
        public ApplicationDbContext _db;

        public EmpleadoRepository(ApplicationDbContext _db)
        {
            this._db = _db;
        }

        public bool ActualizarEmpleado(Empleado empleado)
        {
            _db.Empleado.Update(empleado);
            return Guardar();
        }


        public bool BorrarEmpleado(Empleado empleado)
        {
            _db.Empleado.Remove(empleado);
            return Guardar();
        }

        public IEnumerable<Empleado> BuscarEmpleado(string nombre)
        {
            //var query = _db.Empleado;
            IQueryable<Empleado> query = _db.Empleado;

            if (!string.IsNullOrEmpty(nombre))
            {
                //Cambiar contains por == para prueba
                query = query.Where(a => a.Nombre.Contains(nombre));
            }

            return query.ToList();
        }


        public bool CrearEmpleado(Empleado empleado)
        {
            _db.Empleado.Add(empleado);
            return Guardar();
        }

        public bool ExisteEmpleado(int empleadoId)
        {
            var value = _db.Empleado.Any(a => a.Id == empleadoId);
            return value;

        }

        public bool ExisteEmpleado(string nombreEmpleado, string apellidoEmpleado)
        {
            var value = _db.Empleado.Any(a => a.Nombre.ToLower().Trim() == nombreEmpleado.ToLower().Trim() && a.Apellido.ToLower().Trim() == apellidoEmpleado.ToLower().Trim());
            return value;
        }

        public bool Guardar()
        {
            return _db.SaveChanges() > 0 ? true : false;
        }

        public Empleado ObtenerEmpleado(int empleadoId)
        {
            return _db.Empleado.FirstOrDefault(empl => empl.Id == empleadoId);
        }

        public ICollection<Empleado> ObtenerEmpleados()
        {
            return _db.Empleado.OrderBy(a => a.Id).ToList();
        }

        public ICollection<Empleado> ObtenerEmpleadosPorCargos(int cargosId)
        {
            return _db.Empleado.Include(Ca => Ca.Cargo).Where(a => a.CargoId == cargosId).ToList();
        }
    }
}
