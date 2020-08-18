using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DesafioCSharpDotNetCore.Models
{
    public class Order
    {
        public Order() { }
        
        public Order(string title, string description, List<Product> products)
        {            
            Title = title;
            Description = description;
            Products = products;
        }

        [Key]
        public int Id { get; set; }       

        [Required(ErrorMessage = "Este campo é obrigatório")]
        [MaxLength(60, ErrorMessage = "Este campo deve conter 3 e 60 caracteres")]
        [MinLength(3, ErrorMessage = "Este campo deve conter entre 3 e 60 caracteres")]
        public string Title { get; set; }

        [MaxLength(1024, ErrorMessage = "Este campo deve conter no máximo 1024 caracteres")]
        public string Description { get; set; }
        
        [NotMapped]
        [Required(ErrorMessage = "Este campo é obrigatório")]        
        public List<Int32> ProductsId { get; set; }
        
        [ForeignKey("Products")]
        public List<Product> Products { get; set; }
    }
}
