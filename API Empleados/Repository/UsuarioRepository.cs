using API_Empleados.Models;
using API_Empleados.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_Empleados.Repository
{
    public class UsuarioRepository : IUsuarioRepository
    {
        public readonly ApplicationDbContext _db;

        public UsuarioRepository(ApplicationDbContext _db)
        {
            this._db = _db;
        }

        public bool ExisteUsuario(string usuario)
        {
            if (_db.Usuario.Any(a => a.UsuarioA == usuario))
            {
                return true;
            }

            return false;
        }

        public bool Guardar()
        {
            return _db.SaveChanges() >= 0 ? true : false;
        }

        public Usuario Login(string usuario, string password)
        {
            var user = _db.Usuario.FirstOrDefault(x => x.UsuarioA == usuario);

            if (user == null)
            {
                return null;
            }

            if (!VerificarPasswordHash(password, user.PasswordHash, user.PasswordSalt))
            {
                return null;
            }
            return user;
        }

        public Usuario ObtenerUsuario(int usuarioId)
        {
            return _db.Usuario.FirstOrDefault(a => a.Id == usuarioId);
        }

        public ICollection<Usuario> ObtenerUsuarios()
        {
            return _db.Usuario.OrderBy(a => a.UsuarioA).ToList();
        }

        public Usuario Registrarse(Usuario usuario, string password)
        {
            byte[] passwordHash, passwordSalt;

            CrearPasswordHash(password, out passwordHash, out passwordSalt);

            usuario.PasswordHash = passwordHash;
            usuario.PasswordSalt = passwordSalt;

            _db.Usuario.Add(usuario);
            Guardar();
            return usuario;

        }






        private bool VerificarPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var hashComputado = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

                for (int i = 0; i < hashComputado.Length; i++)
                {
                    if (hashComputado[i] != passwordHash[i]) return false;

                }

            }
            return true;
        }

        private void CrearPasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

    }
}
