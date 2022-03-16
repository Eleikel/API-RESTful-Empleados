using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace API_Empleados.Models
{
#pragma warning disable CS1591 // Falta el comentario XML para el tipo o miembro visible públicamente
    public class Cargo
    {
        [Key]
        public int Id { get; set; }
        public string NombreCargo { get; set; }

        //Foreign con departamento
        public int departamentoId { get; set; }

        [ForeignKey("departamentoId")]
        public Departamento Departamento { get; set; }


    }

#pragma warning restore CS1591 // Falta el comentario XML para el tipo o miembro visible públicamente

}
