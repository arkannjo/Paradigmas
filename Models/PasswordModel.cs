using System.ComponentModel.DataAnnotations;

namespace Paradigmas.Models
{
    public class PasswordModel
    {
        [Required(ErrorMessage = "Por favor, informe o tamanho da senha")]
        [Range(6, 20, ErrorMessage = "O tamanho da senha deve estar entre 6 e 20 caracteres")]
        public int Tamanho { get; set; }

        [Display(Name = "Incluir Letras Minúsculas")]
        public bool IncluirLetrasMinusculas { get; set; }

        [Display(Name = "Incluir Letras Maiúsculas")]
        public bool IncluirLetrasMaiusculas { get; set; }

        [Display(Name = "Incluir Números")]
        public bool IncluirNumeros { get; set; }

        [Display(Name = "Incluir Caracteres Especiais")]
        public bool IncluirCaracteresEspeciais { get; set; }

        [Display(Name = "Senha Gerada")]
        public string? Senha { get; set; }

        [Display(Name = "Força da Senha")]
        public string? ForcaSenha { get; set; }
    }
}
