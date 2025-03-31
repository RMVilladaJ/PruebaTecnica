using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PruebaTecnica.Shared.Entities
{
    public class Sale
    {
        /*La pantalla de ventas debe solicitar los siguientes campos: Nombre del producto,cantidad, valor del impuesto(0%, 15% 19%), 
         fecha de la venta y calcular el precio final(cantidad* precio). Al momento de guardar la información en la base de datos,
        también debe guardar el Id del vendedor que realizo dicha venta.*/


        public int Id { get; set; }

        [Display(Name = "Product quantity")]
        [Required(ErrorMessage = "Este campo es obligatorio")]
        public int Quantity { get; set; }


        [Display(Name = "tax")]
        [Required(ErrorMessage = "Este campo es obligatorio")]
        public double tax { get; set; }

        [Display(Name = "Final price")]
        [Required(ErrorMessage = "Este campo es obligatorio")]
        public double FinalPrice { get; set; }


        [Display(Name = "Sale date")]
        [Required(ErrorMessage = "Esta fecha es obligatoria")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public DateTime SaleDate { get; set; }


        [ForeignKey("ProductId")]
        public Product Products { get; set; }
        public int ProductId { get; set; }

        [JsonIgnore]
        public User Users { get; set; }
        public string UserId { get; set; }
        

    }
}
