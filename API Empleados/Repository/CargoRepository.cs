using API_Empleados.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_Empleados.Repository.IRepository
{
    public class CargoRepository : ICargoRepository
    {
        // Ojo con el readonly
        public readonly ApplicationDbContext _db;
        public CargoRepository(ApplicationDbContext _db)
        {
            this._db = _db;
        }
        public bool ActualizarCargo(Cargo cargo)
        {
            _db.Cargo.Update(cargo);
            return Guardar();
        }

        public bool BorrarCargo(Cargo cargo)
        {
            _db.Cargo.Remove(cargo);
            return Guardar();
        }

        public bool CrearCargo(Cargo cargo)
        {
            _db.Cargo.Add(cargo);
            return Guardar();
        }

        public bool ExisteCargo(int cargoId)
        {
            bool existe = _db.Cargo.Any(a => a.Id == cargoId);
            return existe;
        }

        public bool ExisteCargo(string NombreCargo)
        {
            bool existe = _db.Cargo.Any(a => a.NombreCargo.ToLower().Trim() == NombreCargo.ToLower().Trim());
            return existe;
        }

        public bool Guardar()
        {
            return _db.SaveChanges() >= 0 ? true : false;
        }

        public ICollection<Cargo> ObtenerCargos()
        {
            return _db.Cargo.OrderBy(a => a.NombreCargo).ToList();
        }

        public Cargo ObtenerCargo(int cargoId)
        {
            return _db.Cargo.FirstOrDefault(a => a.Id == cargoId);
        }

        public ICollection<Cargo> ObtenerCargosPorDepartamentos(int deptId)
        {
            return _db.Cargo.Include(dept => dept.Departamento).Where(dept => dept.departamentoId == deptId).ToList();
        }

        public IEnumerable<Cargo> BuscarCargo(string NombreCargo)
        {
            IQueryable<Cargo> query = _db.Cargo;

            if (!string.IsNullOrEmpty(NombreCargo))
            {
                query = query.Where(e => e.NombreCargo.Contains(NombreCargo));
            }
            return query.ToList();
        }
    }
}
