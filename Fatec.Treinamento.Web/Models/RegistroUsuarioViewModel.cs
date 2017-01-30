using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Fatec.Treinamento.Model;

namespace Fatec.Treinamento.Web.Models
{
    /// <summary>
    /// Um viewmodel serve para trabalhar com os dados na view.
    /// Esse viewmodel é usado para obter os dados de registro de usuario
    /// </summary>
    public class RegistroUsuarioViewModel
    {
        [Required(ErrorMessage ="O campo {0} é Obrigatório.")]
        [Display(Name = "Nome")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O campo {0} é Obrigatório.")]
        [EmailAddress(ErrorMessage = "O campo Email deve conter um endereço de email válido.")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "O campo {0} é Obrigatório.")]
        [StringLength(100, ErrorMessage = "A Senha deve ter ao menos 6 caracteres.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Senha")]
        public string Senha { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmar Senha")]
        [System.ComponentModel.DataAnnotations.Compare("Senha", ErrorMessage = "A senha e confirmação não coincidem")]
        public string ConfirmacaoSenha { get; set; }

        public IEnumerable<SelectListItem> ListaPerfil { get; set; }

        public int IdPerfil { get; set; }
        

    }
}