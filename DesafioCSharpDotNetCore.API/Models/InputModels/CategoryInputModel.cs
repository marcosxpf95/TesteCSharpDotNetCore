using System.ComponentModel.DataAnnotations;

namespace DesafioCSharpDotNetCore.Models.InputModels
{
    public class CategoryInputModel
    {
        [Required(ErrorMessage = "Este campo é obrigatório")]
        [MaxLength(60, ErrorMessage = "Este campo deve conter entre 3 e 60 caracteres")]
        [MinLength(3, ErrorMessage = "Este campo deve conter entre 3 e 60 caracteres")]
        public string Title { get; set; }
    }
}
