using API_Empleados.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_Empleados.Repository.IRepository
{
    public interface IUsuarioRepository
    {
        ICollection<Usuario> ObtenerUsuarios();
        Usuario ObtenerUsuario(int UsuarioId);
        bool ExisteUsuario(string usuario);
        Usuario Registrarse(Usuario usuario, string password);
        Usuario Login(string usuario, string password);
        bool Guardar();


        
    }
}
