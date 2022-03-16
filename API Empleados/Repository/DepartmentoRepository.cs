using API_Empleados.Models;
using API_Empleados.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_Empleados.Repository
{
    public class DepartmentoRepository : IDepartamentoRepository
    {
        public readonly ApplicationDbContext _db;

        public DepartmentoRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public bool ActualizarDepartamento(Departamento departamento)
        {
            _db.Departamento.Update(departamento);
            return Guardar();
        }

        public bool BorrarDepartamento(Departamento departamento)
        {
            _db.Departamento.Remove(departamento);
            return Guardar();
        }

        public bool CrearDepartamento(Departamento departamento)
        {
            _db.Departamento.Add(departamento);
            return Guardar();
        }

        public bool ExisteDepartamento(int departamentoId)
        {
            bool value = _db.Departamento.Any(a => a.Id == departamentoId);
            return value;
        }

        public bool ExisteDepartamento(string NombreDepartamento)
        {
            bool value = _db.Departamento.Any(a => a.Nombre.ToLower().Trim() == NombreDepartamento);
            return value;
        }

        public bool Guardar()
        {
            return _db.SaveChanges() >= 0 ? true : false;
        }

        public Departamento ObtenerDepartamento(int departamentoId)
        {
            return _db.Departamento.FirstOrDefault(a => a.Id == departamentoId);
        }

        public ICollection<Departamento> ObtenerDepartamentos()
        {
            return _db.Departamento.OrderBy(a => a.Nombre).ToList();
        }
    }
}
