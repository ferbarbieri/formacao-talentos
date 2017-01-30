using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fatec.Treinamento.Model.DTO;

namespace Fatec.Treinamento.Model
{
    public class Trilha
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O campo Nome é obrigatório.")]
        [StringLength(100, ErrorMessage = "O Nome deve ter ao menos 6 caracteres.", MinimumLength = 6)]
        public string Nome { get; set; }

        [Display(Name = "Trilha ativa?")]
        public bool Ativa { get; set; }
        
        /// <summary>
        /// Essa propriedade lista os cursos que fazem parte dessa trilha.
        /// Provavelmente em todas as telas onde é necessário exibir uma trilha, só fará
        /// sentido se vier os cursos.
        /// </summary>
        public IList<DetalhesCurso> Cursos { get; set; }

        // Construtor. 
        public Trilha()
        {
            //Inicializa a lista de cursos para não dar erro de nullexception
            Cursos = new List<DetalhesCurso>();
        }
    }
}
