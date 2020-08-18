using System.ComponentModel.DataAnnotations;

namespace DesafioCSharpDotNetCore.Models
{
    public class Category
    {
        public Category() { }        

        public Category(string title)
        {            
            Title = title;
        }

        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Este campo é obrigatório")]
        [MaxLength(60, ErrorMessage = "Este campo deve conter entre 3 e 60 caracteres")]
        [MinLength(3, ErrorMessage = "Este campo deve conter entre 3 e 60 caracteres")]
        public string Title { get; set; }
    }
}