using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CrudMVc.Models
{
    public class Usuario
    {
        [Display(Name = "Nombre")]
        public string nombre { get; set; }

        [Display(Name = "Apellido")]
        public string apellido { get; set; }

        [Display(Name = "Correo")]
        public string correo { get; set; }

        [Display(Name = "Contraseña")]
        public string contraseña { get; set; }
    }
}
