using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API_Empleados.Models.DTOs
{
    public class DepartamentoDto
    {

        //Ojo con los display

        public int Id { get; set; }

        [Required(ErrorMessage = "Es Obligatorio asignar el Nombre del Departamento en este Campo")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "Es Obligatorio asignar el Numero Telefonico del Departamento en este Campo")]
        [StringLength(14)]  //OJO CON ESTO
        public string Telefono { get; set; }
    }
}
