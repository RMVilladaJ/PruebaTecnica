using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PruebaTecnica.Shared.Entities
{
    public class Product
    {
        //La pantalla productos debe solicitar la siguiente información: Nombre,Codigo, Valor,Fecha Creación,
        //proveedor (Información se debe traer desde una tabla) y foto delproducto.
        public int Id { get; set; }

        [Display(Name = "Code")]
        [MaxLength(50, ErrorMessage = "Este campo no puede tener más de 50 caracteres.")]
        [Required(ErrorMessage = "Este campo es obligatorio")]
        public string Code { get; set; }

        [Display(Name = "Name product")]
        [MaxLength(50, ErrorMessage = "Este campo no puede tener más de 50 caracteres.")]
        [Required(ErrorMessage = "Este campo es obligatorio")]
        public string NameProduct { get; set; }

        [Display(Name = "product photo")]
        public string PhotoUrl { get; set; }
        public string PhotoKey { get; set; }



        [Display(Name = "Price")]
        [Required(ErrorMessage = "Este campo es obligatorio")]
        public double Price { get; set; }

        [Display(Name = "Create date")]
        [Required(ErrorMessage = "Esta fecha es obligatoria")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public DateTime CreateDate { get; set; }


        [ForeignKey("SupplierId")]
        public Supplier Supplier { get; set; }
        public int SupplierId { get; set; }

        [JsonIgnore]
        public ICollection<Sale> Sales { get; set; }


    }
}
