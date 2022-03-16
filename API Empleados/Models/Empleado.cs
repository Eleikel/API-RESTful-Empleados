using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace API_Empleados.Models
{
    public class Empleado
    {
        [Key]
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string RutaImagen { get; set; }
        public DateTime FechaIngreso { get; set; }

        //Foreing
        public int CargoId { get; set; }
        [ForeignKey("CargoId")] // Esto es CargoId

        public Cargo Cargo { get; set; }
    }
}
