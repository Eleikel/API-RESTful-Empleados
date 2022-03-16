using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_Empleados.Models.DTOs
{
    public class UsuarioDto
    {
        public string UsuarioA { get; set; }
        public byte[] PasswordHash { get; set; }
    }
}
