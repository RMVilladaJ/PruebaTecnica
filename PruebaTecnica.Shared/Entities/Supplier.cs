using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PruebaTecnica.Shared.Entities
{
    public class Supplier
    {
        //  La tabla de proveedores debe tener los siguientes campos: Nombre, Nit, Teléfono,Celular, email.
        public int Id { get; set; }

        [Display(Name = "Name supplier")]
        [MaxLength(50, ErrorMessage = "Este campo no puede tener más de 50 caracteres.")]
        [Required(ErrorMessage = "Este campo es obligatorio")]
        public string NameSupplier { get; set; }

        [Display(Name = "NIT")]
        [MaxLength(50, ErrorMessage = "Este campo no puede tener más de 50 caracteres.")]
        [Required(ErrorMessage = "Este campo es obligatorio")]
        public string NIT { get; set; }


        [Display(Name = "Fixed Phone")]
        [MaxLength(7, ErrorMessage = "Este campo no puede tener más de 7 caracteres.")]
        public string FixedPhone { get; set; }


        [Display(Name = "Cell Phone")]
        [MaxLength(10, ErrorMessage = "Este campo no puede tener más de 10 caracteres.")]
        public string CellPhone { get; set; }



        [Display(Name = "Email")]
        [MaxLength(50, ErrorMessage = "Este campo no puede tener más de 50 caracteres.")]
        public string Email { get; set; }

        [JsonIgnore]

        public ICollection<Product> Products { get; set; }

    }
}
